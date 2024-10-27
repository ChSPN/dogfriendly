# Stage 1: Build the ASP.NET application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ./src ./
WORKDIR /src/DogFriendly.Admin

RUN dotnet build "./DogFriendly.Admin.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "./DogFriendly.Admin.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

COPY ./src/DogFriendly.Admin/wwwroot /app/publish/wwwroot

# Stage 2: Setup the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose ports
EXPOSE 8282

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8282

# Start ASP.NET application
CMD ["dotnet", "DogFriendly.Admin.dll"]