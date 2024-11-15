# DogFriendly

## Introduction
Welcome to Dog Friendly – a web application designed to help dog owners find pet-friendly places with ease. Whether for a weekend getaway or a longer vacation, Dog Friendly makes it simple to discover locations that welcome dogs, ensuring a stress-free experience for you and your furry friends.

🌐 Access the deployed site: Dog Friendly - https://dogfriendly.delpierre.me/

📖 Read the final project blog article: Dog Friendly Blog Article - https://medium.com/@8766_21533/creating-dog-friendly-my-journey-to-building-a-web-application-for-dog-lovers-6d034f024939 

👩 Author: Charlène Scomparin - https://www.linkedin.com/in/charlène-scomparin/ 


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

### DogFriendly.Admin
A **Blazor** backoffice project that interacts with database.

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

In projects (**DogFriendly.API**, **DogFriendly.Web** and **DogFriendly.Admin**), the Firebase configuration will be automatically retrieved from the `appsettings.json` file.
Ensure that the Firebase service is properly set up to use these parameters in your application's code.

### FileStorage Configuration for Cloudflare R2

The `FileStorage` section in the `appsettings.json` is designed to configure the connection to Cloudflare R2 storage using the AWS S3 SDK. Each setting is crucial for secure access and proper functionality. Below is a breakdown of the key properties:

- **AccountId**: Your Cloudflare account identifier. It uniquely identifies your Cloudflare account for all API interactions.
- **AccessKey**: The access key provided by Cloudflare, which acts as the public credential for authenticating API requests.
- **SecretKey**: The corresponding secret key, used together with the access key to securely authenticate with Cloudflare R2.
- **DomainUri**: The base URL for accessing the R2 storage domain, usually formatted as `https://<account_id>.r2.cloudflarestorage.com/`.
- **UsersUri**: The URI for accessing the users’ storage bucket or endpoint, which may hold profile images or user-specific files.
- **PlacesUri**: The URI for accessing place-related files, such as images and media associated with locations in your application.
- **NewsUri**: The URI used to store or retrieve files related to news content in your application.
- **AmenitiesUri**: The URI for storing files related to amenities, such as images or additional media associated with your entities.
- **BucketName**: The name of the bucket where your application’s files are stored within the R2 service.

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
    
### Backoffice (DogFriendly.Admin)
1. Build the Docker image:
    ```bash
    docker build -f Admin.Dockerfile -t dogfriendly-admin .
    ```
2. Run the container:
    ```bash
    docker run -d -p 8082:8082 dogfriendly-admin
    ```

## How to Deploy on Sliplane

### Step 1: Set up the Project on Sliplane
1. Sign up on [Sliplane](https://sliplane.io) and create a new project.
2. Connect your GitHub repository to Sliplane, allowing access to the **DogFriendly** repo.

### Step 2: Configure Docker Deployment
1. In Sliplane, navigate to the **Deployments** section and create a new deployment.
2. For the front-end, choose `Front.Dockerfile`. For the back-end, select `Back.Dockerfile`. For backoffice, select `Admin.Dockerfile`.

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
