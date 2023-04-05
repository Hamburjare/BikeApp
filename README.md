# Bike App
## Description
This is a web app for Solita Dev Academy 2023.

## Technologies
* Docker
* MySQL
* Dotnet Core 6.0
* xUnit

## Installation
  1. Clone the repository
  2. Make sure you have .NET Core SDK 6.0 installed. You can find the installation instructions [here](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).
  3. Run the following command in the csvimport folder of the project and wait for the script to finish:
  
    dotnet run
    
  4. Make sure you have docker installed. You can find the installation instructions [here](https://docs.docker.com/get-docker/).
  5. Make sure you have docker-compose installed. You can find the installation instructions [here](https://docs.docker.com/compose/install/).
  6. Run the following command in the root folder of the project:
  
    docker-compose up -d
    
  7. Wait for the docker container(s) to start. It can take a while the first time you run it.
  8. Open the browser and go to [http://localhost:5000/swagger](http://localhost:5000/swagger)

## API 
  You can see all the API endpoints and documentation in the [Swagger UI](http://localhost:5000/swagger/index.html). You can also play with the API directly from the Swagger UI.


## Tests
  You can run the tests by running the following command after you have successfully started docker container(s) [(step 6)](#installation):
  ```bash
  docker exec backend dotnet test Backend-BikeApp.dll
  ```
  NOTE: You must have the database running and populated in order to run the tests.
