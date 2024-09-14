# DogFriendly

## Description of the Project Structure

### DogFriendly.API
This project manages the **API** layer built with **ASP.NET Core**, which handles HTTP requests and connects to the data layer in **DogFriendly.Infrastructure**. It provides services and endpoints for client communication.

### DogFriendly.Application
Contains the core business logic and services. It interacts between the **API** and **Infrastructure** layers, implementing the application's functional requirements.

### DogFriendly.Infrastructure
This layer handles data access using **Entity Framework**. It manages the database context, migrations, and repository patterns for the application.

### DogFriendly.Domain
Defines the core business models and domain entities that represent the application's key data objects.

### DogFriendly.Web.Client
The front-end project, built with **Blazor**, provides an interactive user interface and communicates with the **DogFriendly.API** to display data related to dog-friendly locations and user information.

## How to Build and Run Docker Containers

### Front-End (DogFriendly.Web.Client)

1. Ensure you're in the project root where the `Front.Dockerfile` is located.
2. Build the Docker image for the front-end:
    ```bash
    docker build -f Front.Dockerfile -t dogfriendly-client .
    ```
3. Run the front-end container:
    ```bash
    docker run -d -p 8080:80 dogfriendly-client
    ```

### Back-End (DogFriendly.API)

1. Ensure you're in the project root where the `Back.Dockerfile` is located.
2. Build the Docker image for the back-end:
    ```bash
    docker build -f Back.Dockerfile -t dogfriendly-api .
    ```
3. Run the back-end container:
    ```bash
    docker run -d -p 5000:80 dogfriendly-api
    ```

## How to Run the Test Project DogFriendly.Tests

1. Navigate to the **DogFriendly.Tests** project:
    ```bash
    cd DogFriendly.Tests
    ```
2. Run the tests using the **.NET CLI**:
    ```bash
    dotnet test
    ```
