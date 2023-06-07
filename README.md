# Bike App

## Contents
- [Bike App](#bike-app)
  - [Contents](#contents)
  - [Description](#description)
  - [Data](#data)
  - [Technologies](#technologies)
    - [Backend](#backend)
    - [Frontend](#frontend)
  - [Configuration](#configuration)
    - [Frontend](#frontend-1)
    - [csvimport](#csvimport)
    - [Backend](#backend-1)
  - [Installation](#installation)
  - [Usage](#usage)
    - [Frontend](#frontend-2)
    - [Backend](#backend-2)
  - [Tests](#tests)
    - [Backend](#backend-3)
    - [Frontend](#frontend-3)
  - [API](#api)
  - [TODO](#todo)
    - [Data import](#data-import)
    - [Journey list view](#journey-list-view)
    - [Station list](#station-list)
    - [Single station view](#single-station-view)
    - [More](#more)

## Description
This is a web app that displays bike journeys and bike station information in Helsinki. The app is built with React and .NET 6.0. The backend is a REST API that uses MariaDB as a database. The frontend is a single page application that uses Vite as a build tool and TailwindCSS as a CSS framework. The app is dockerized and can be run with single command.

## Data
The data that is used in this web app is owned by City Bike Finland.
* <https://dev.hsl.fi/citybikes/od-trips-2021/2021-05.csv>
* <https://dev.hsl.fi/citybikes/od-trips-2021/2021-06.csv>
* <https://dev.hsl.fi/citybikes/od-trips-2021/2021-07.csv>

Dataset for bike stations:
* <https://opendata.arcgis.com/datasets/726277c507ef4914b0aec3cbcfcbfafc_0.csv>
* License and information: <https://www.avoindata.fi/data/en/dataset/hsl-n-kaupunkipyoraasemat/resource/a23eef3a-cc40-4608-8aa2-c730d17e8902>

## Technologies

### Backend
* Docker - Containerization
* MariaDB - Database
* Dotnet 6.0 - Backend framework
* xUnit - Testing framework

### Frontend
* React - Frontend framework
* Vite - Build tool
* Playwright - Testing framework
* material-ui - UI framework
* TailwindCSS - CSS framework
* PostCSS - CSS preprocessor
* Leaflet - Map library
* React Router - Routing
* Axios - HTTP client
* React-table - Table component
* React-paginate - Pagination component


## Configuration
You don't need to configure anything to run the app. The app is dockerized and can be run with single command. However, if you wish to change the configuration, you can do so by changing the environment variables.

### Frontend 
  You can change the values of the environment variables in the ``docker-compose.yml`` file.

  * ``VITE_API_URL``: The URL of the backend API. Default value is ``http://localhost:5000/api``.

### csvimport
  You can change the database connection string in the ``csvimport/src/ImportData.cs`` file.


### Backend
  You can change the values of the environment variables in the ``docker-compose.yml`` file. And you can change the database connection string in the ``backend/Services/MySQLHelper.cs`` file.

  * ``connectionString``: The variable that holds the database connection string. Default value is ``Server=host.docker.internal;Port=3306;User ID=root;Password=Abc123;Database=bikeapp;``.
  * You can change the database root password in the ``docker-compose.yml`` file. Under the ``db`` service, change the ``MARIADB_ROOT_PASSWORD`` variable. Default value is ``Abc123``. Remember to change the password in the ``connectionString`` variable as well.
  * You can change the database name in the ``docker-compose.yml`` file. Under the ``db`` service, change the ``MARIADB_DATABASE`` variable. Default value is ``bikeapp``. Remember to change the database name in the ``connectionString`` variable as well.
  * All the ports etc. are configured in the ``docker-compose.yml`` file.
  * After you have changed the database connection string, you have to rebuild the docker image. You can do so by running the following command in the root folder of the project:
  
    ```bash
    docker-compose up -d --build
    ```

## Installation
  1. Clone the repository
  2. Make sure you have docker installed. You can find the installation instructions [here](https://docs.docker.com/get-docker/).
  3. Make sure you have docker-compose installed. You can find the installation instructions [here](https://docs.docker.com/compose/install/).
  4. Run the following command in the root folder of the project:
  
    docker-compose up -d
    
  5. Wait for the docker container(s) to start. It can take a while the first time you run it.
  6. Database population can take a while. You can check the progress by running the following command:
  
    docker logs import

  7. Open the browser and go to [http://localhost:5000/swagger](http://localhost:5000/swagger) to play with the API.
  8. Open the browser and go to [http://localhost:8000](http://localhost:8000) to play with the web app.

## Usage
  ### Frontend
  You can use the web app by going to [http://localhost:8000](http://localhost:8000) in your browser.

  ### Backend
  You can play with the API directly from the Swagger UI [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html).


## Tests
### Backend
  You can run the tests by running the following command after you have successfully started docker container(s) [(step 6)](#installation) and populated the database [(step 3)](#installation):
  ```bash
  docker exec backend dotnet test Backend-BikeApp.dll
  ```
  NOTE: You must have the database running and populated in order to run the tests.

### Frontend
  You can run the tests by running the following command outside of the docker container in the root directory after you have successfully started docker container(s) [(step 6)](#installation):
  Make sure you have Node.js installed. You can find the installation instructions [here](https://nodejs.org/en/download/). Then run the following commands:
  ```bash
  cd frontend
  npx playwright install
  npx playwright install-deps
  npm run test
  ```
  NOTE: You must have the backend, frontend containers running in order to run the tests.


## API 
  You can see all the API endpoints and documentation in the [Swagger UI](http://localhost:5000/swagger/index.html). You can also play with the API directly from the Swagger UI.

## TODO
### Data import
* [x] Import data from the CSV files to a database or in-memory storage
* [x] Validate data before importing
* [x] Don't import journeys that lasted for less than ten seconds
* [x] Don't import journeys that covered distances shorter than 10 meters

### Journey list view

* [x] List journeys
  * If you don't implement pagination, use some hard-coded limit for the list length because showing several million rows would make any browser choke
* [x] For each journey show departure and return stations, covered distance in kilometers and duration in minutes
* [x] Show the total number of journeys in the list
* [x] Pagination
* [x] Ordering per column
* [x] Searching
* [x] Filtering

### Station list

* [x] List all the stations
* [x] Pagination
* [x] Searching

### Single station view
* [x] Station name
* [x] Station address
* [x] Total number of journeys starting from the station
* [x] Total number of journeys ending at the station
* [x] Station location on the map
* [x] The average distance of a journey starting from the station
* [x] The average distance of a journey ending at the station
* [x] Top 5 most popular return stations for journeys starting from the station
* [x] Top 5 most popular departure stations for journeys ending at the station
* [x] Ability to filter all the calculations per month

### More
* [x] Endpoints to store new journeys data or new bicycle stations
* [x] Running backend in Docker
* [x] Running backend in Cloud
* [x] Implement E2E tests
* [x] Create UI for adding journeys or bicycle stations
