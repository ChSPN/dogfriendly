# DogFriendly

## Description of the Project Structure

### DogFriendly.API
Handles the **API** logic built with **ASP.NET Core**. It processes HTTP requests and connects to the data layer via **DogFriendly.Infrastructure**.

### DogFriendly.Application
Contains core business logic and services between the **API** and **Infrastructure**.

### DogFriendly.Infrastructure
Manages data access using **Entity Framework**.

### DogFriendly.Domain
Defines core business models and domain entities.

### DogFriendly.Web.Client
A **Blazor** front-end project that interacts with **DogFriendly.API**.

### DogFriendly.Web
This project is responsible for hosting the **DogFriendly.Web.Client** with **server-side prerendering**. It delivers a faster initial page load by rendering the front-end on the server and then passing control to the client-side **Blazor** application.

### Add a Migration to **DogFriendly.Infrastructure** project

1. **Define the Connection String**:
   Make sure the connection string for your database is correctly configured in the `appsettings.json` file or your `DbContext` constructor.

2. **Add a Migration**:
   Open the **Package Manager Console** or a terminal and navigate to the **Infrastructure** project directory. Run the following command to add a migration:
   ```bash
   Add-Migration <MigrationName> -Project DogFriendly.Infrastructure -StartupProject DogFriendly.API
   ```

3. **Update the Database**:
   After creating the migration, apply the changes to the database with the following command:
   ```bash
   Update-Database -Project DogFriendly.Infrastructure -StartupProject DogFriendly.API
   ```

### Firebase Configuration

To use Firebase in the **DogFriendly.API** and **DogFriendly.Web** projects, you need to add the Firebase configurations in the `appsettings.json` files of both projects. Follow these steps:

#### Step 1: Retrieve Your Firebase Keys

1. Go to the Firebase console at the following address: [https://console.firebase.google.com/](https://console.firebase.google.com/).
2. Select your project or create a new one.
3. Go to the **project settings** in Firebase, and find the following information in the **Firebase SDK** section:
   - **ApiKey**
   - **AuthDomain**
   - **ProjectId**
   - **AppId**

#### Step 2: Add the Configuration to `appsettings.json`

In the **DogFriendly.API** and **DogFriendly.Web** projects, open the `appsettings.json` file and add the following section with your own Firebase values.

##### Section to Add in `appsettings.json`:

```json
"Firebase": {
  "ApiKey": "YOUR_API_KEY",
  "AuthDomain": "YOUR_AUTH_DOMAIN",
  "ProjectId": "YOUR_PROJECT_ID",
  "AppId": "YOUR_APP_ID"
}
```

##### Example:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Firebase": {
    "ApiKey": "AIzaSyD...1234",
    "AuthDomain": "myapp.firebaseapp.com",
    "ProjectId": "myapp",
    "AppId": "1:1234567890:web:abcdef123456"
  }
}
```

#### Step 3: Use in Your Application

In both projects (**DogFriendly.API** and **DogFriendly.Web**), the Firebase configuration will be automatically retrieved from the `appsettings.json` file.
Ensure that the Firebase service is properly set up to use these parameters in your application's code.

## How to Build and Run Docker Containers

### Front-End (DogFriendly.Web)
1. Build the Docker image:
    ```bash
    docker build -f Front.Dockerfile -t dogfriendly-client .
    ```
2. Run the container:
    ```bash
    docker run -d -p 8080:8080 dogfriendly-client
    ```

### Back-End (DogFriendly.API)
1. Build the Docker image:
    ```bash
    docker build -f Back.Dockerfile -t dogfriendly-api .
    ```
2. Run the container:
    ```bash
    docker run -d -p 8181:8181 dogfriendly-api
    ```

## How to Deploy on Sliplane

### Step 1: Set up the Project on Sliplane
1. Sign up on [Sliplane](https://sliplane.io) and create a new project.
2. Connect your GitHub repository to Sliplane, allowing access to the **DogFriendly** repo.

### Step 2: Configure Docker Deployment
1. In Sliplane, navigate to the **Deployments** section and create a new deployment.
2. For the front-end, choose `Front.Dockerfile`. For the back-end, select `Back.Dockerfile`. For **DogFriendly.Web**, select `Web.Dockerfile`.

### Step 3: Automated Build and Deploy
1. Sliplane will automatically build your Docker images from the provided Dockerfiles.
2. After building, Sliplane will deploy the containers, handling scaling and monitoring automatically.

### Step 4: Continuous Deployment (CI/CD)
1. Enable automatic builds in Sliplane so that each push to GitHub triggers a new deployment.
2. This ensures that any updates to `develop` or `main` branches are automatically compiled, built, and deployed.

## GitFlow and CI/CD Pipeline

This project follows the **GitFlow** workflow for source control management. In the CI/CD pipeline, GitHub Actions are configured to build and compile Docker images automatically for every **pull request**. This ensures that code is thoroughly tested and images are ready for deployment after review.

## How to Run the Test Project DogFriendly.Tests

1. Navigate to the **DogFriendly.Tests** project:
    ```bash
    cd DogFriendly.Tests
    ```
2. Run the tests using the **.NET CLI**:
    ```bash
    dotnet test
    ```

## License

This project is licensed under the Apache License 2.0 - see the [LICENSE](./LICENSE) file for details.
