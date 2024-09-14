# Stage 1: Build the ASP.NET application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ./src ./
WORKDIR /src/DogFriendly.Web

RUN dotnet build "./DogFriendly.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "./DogFriendly.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

COPY ./src/DogFriendly.Web.Client/wwwroot /app/publish/wwwroot

# Stage 2: Setup the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose ports
EXPOSE 8080

# Set environment variables
ENV ApiUrl=http://localhost:8181/
ENV ASPNETCORE_URLS=http://+:8080

# Start ASP.NET application
CMD ["dotnet", "DogFriendly.Web.dll"]