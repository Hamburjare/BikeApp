# Bike App
## Description
This is a web app for Solita Dev Academy 2023.

## Technologies
* Docker
* MySQL
* Dotnet Core 7.0
* xUnit

## Installation
  1. Clone the repository
  2. Make sure you have docker installed. You can find the installation instructions [here](https://docs.docker.com/get-docker/).
  3. Make sure you have docker-compose installed. You can find the installation instructions [here](https://docs.docker.com/compose/install/).
  4. Run the following command in the root folder of the project:
  ```bash
  docker-compose up -d
  ```
  5. Wait for the docker container to start. It can take a while the first time you run it.
  6. Open the browser and go to [http://localhost:5000](http://localhost:5000)

## API 
  You can see all the API endpoints and documentation in the [Swagger UI](http://localhost:5000/swagger/index.html). You can also play with the API directly from the Swagger UI.


## Tests
  When you run the docker container, the tests are run automatically. You can also run the tests manually by running the following command in the backend folder of the project:
  ```bash
  dotnet test
  ```  
  NOTE: You need to have the database running in order to run the tests. And you need to have installed the dotnet core 7.0. You can find the installation instructions [here](https://dotnet.microsoft.com/en-us/download/dotnet/7.0).