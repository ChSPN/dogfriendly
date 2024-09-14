# Stage 1: Build the ASP.NET application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ./src ./

WORKDIR /src/DogFriendly.Tests

RUN dotnet test

WORKDIR /src/DogFriendly.API

RUN dotnet build "./DogFriendly.API.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "./DogFriendly.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 2: Setup the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose ports
EXPOSE 8181

# Set environment variables
ENV ConnectionStrings__DataContext=Server
ENV ASPNETCORE_URLS=http://+:8181

# Start ASP.NET application
CMD ["dotnet", "DogFriendly.API.dll"]