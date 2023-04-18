# Bike App
## Description
This is a web app that displays bike journeys and bike station information in Helsinki. The app is built with React and .NET 6.0. The backend is a REST API that uses MariaDB as a database. The frontend is a single page application that uses Vite as a build tool and TailwindCSS as a CSS framework. The app is dockerized and can be run with a couple commands. The app is deployed to Hetzner Cloud and can be accessed [here](https://bikeapp.hamburjare.tech/). The app is also protected with Cloudflare. If you wish to play with the backend, you can find the API documentation [here](https://backend.hamburjare.tech/swagger/index.html).

## Data
The data that is used in this web app is owned by City Bike Finland.
* <https://dev.hsl.fi/citybikes/od-trips-2021/2021-05.csv>
* <https://dev.hsl.fi/citybikes/od-trips-2021/2021-06.csv>
* <https://dev.hsl.fi/citybikes/od-trips-2021/2021-07.csv>

Dataset for bike stations:
* <https://opendata.arcgis.com/datasets/726277c507ef4914b0aec3cbcfcbfafc_0.csv>
* License and information: <https://www.avoindata.fi/data/en/dataset/hsl-n-kaupunkipyoraasemat/resource/a23eef3a-cc40-4608-8aa2-c730d17e8902>

## Technologies

### Cloud
* Hetzner Cloud
* Cloudflare Zero Trust

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

### Frontend 
  You can change the values of the environment variables in the ``docker-compose.yml`` file.

  * ``VITE_API_URL``: The URL of the backend API. Default value is ``http://localhost:5000/api``.

### Backend
  You can change the values of the environment variables in the ``docker-compose.yml`` file. And you can change the database connection string in the ``backend/Services/MySQLHelper.cs`` file.

  * ``connectionString``: The variable that holds the database connection string. Default value is ``Server=host.docker.internal;Port=3306;User ID=root;Password=Abc123;Database=bikeapp;``.
  * You can change the database root password in the ``docker-compose.yml`` file. Under the ``db`` service, change the ``MARIADB_ROOT_PASSWORD`` variable. Default value is ``Abc123``. Remember to change the password in the ``connectionString`` variable as well.
  * You can change the database name in the ``docker-compose.yml`` file. Under the ``db`` service, change the ``MARIADB_DATABASE`` variable. Default value is ``bikeapp``. Remember to change the database name in the ``connectionString`` variable as well.
  * All the ports etc. are configured in the ``docker-compose.yml`` file.

## Installation
  1. Clone the repository
  2. Make sure you have .NET Core SDK 6.0 installed. You can find the installation instructions [here](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).
  3. Run the following command in the ``csvimport`` folder of the project and wait for the script to finish:
  
    dotnet run
    
  4. Make sure you have docker installed. You can find the installation instructions [here](https://docs.docker.com/get-docker/).
  5. Make sure you have docker-compose installed. You can find the installation instructions [here](https://docs.docker.com/compose/install/).
  6. Run the following command in the root folder of the project:
  
    docker-compose up -d
    
  7. Wait for the docker container(s) to start. It can take a while the first time you run it.
  8. Open the browser and go to [http://localhost:5000/swagger](http://localhost:5000/swagger) to play with the API.
  9. Open the browser and go to [http://localhost:8000](http://localhost:8000) to play with the web app.

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
  You can run the tests by running the following command after you have successfully started docker container(s) [(step 6)](#installation):
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