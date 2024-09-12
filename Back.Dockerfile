# Stage 1: Build the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base-builder
WORKDIR /app

# Install Nginx, Certbot, gettext and Cron
RUN apt-get update && \
    apt-get install -y nginx certbot python3-certbot-nginx cron gettext && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Stage 2: Build the ASP.NET application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ./src ./
WORKDIR /src/DogFriendly.API

RUN dotnet build "./DogFriendly.API.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "./DogFriendly.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 3: Setup Nginx, Certbot, and the final image
FROM base-builder AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY ./build ./build

# Copy Nginx configuration file
COPY ./build/back/nginx.conf /etc/nginx/nginx.conf.template

# Copy the script to renew certificates
COPY ./build/renew_certs.sh /usr/local/bin/renew_certs.sh
RUN chmod +x /usr/local/bin/renew_certs.sh

# Add cron job for certificate renewal
COPY ./build/crontab /etc/cron.d/certbot-renew
RUN chmod 0644 /etc/cron.d/certbot-renew
RUN crontab /etc/cron.d/certbot-renew

# Expose ports
EXPOSE 80
EXPOSE 443

# Set environment variables
ENV DOMAIN_NAME=example.com
ENV EMAIL=example@example.com
ENV ConnectionStrings__DataContext=Server

# Copy and set entrypoint script
COPY ./build/back/entrypoint.sh /usr/local/bin/entrypoint.sh
RUN chmod +x /usr/local/bin/entrypoint.sh

# Copy temp certs
COPY ./build/fullchain.pem /etc/letsencrypt/live/domain/fullchain.pem
COPY ./build/privkey.pem /etc/letsencrypt/live/domain/privkey.pem

# Start Nginx, Certbot, and the ASP.NET application
ENTRYPOINT ["/usr/local/bin/entrypoint.sh"]