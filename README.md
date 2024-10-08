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
