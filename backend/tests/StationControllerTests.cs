using System.Collections.Generic;
using System.Threading.Tasks;
using Backend_BikeApp.Controllers;
using Backend_BikeApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BikeApp_Backend.test;

public class StationControllerTests
{
    // GET: api/Stations
    [Fact]
    public async Task GetStationItemsReturnsCorrectNumberOfJourneysByDefault()
    {
        // Arrange
        var controller = new StationsController();

        // Act
        var result = await controller.GetStationItems(null, null, null);

        // Assert
        // okResult.Value is anonymous type
        var okResult = result.Result as OkObjectResult;

        var stations =
            okResult?.Value?.GetType().GetProperty("stations")?.GetValue(okResult.Value)
            as List<Station>;

        Assert.IsNotType<NotFoundResult>(result.Result);
        Assert.IsNotType<BadRequestResult>(result.Result);
        Assert.NotNull(stations);

        Assert.Equal(10, stations.Count);
    }

    [Fact]
    public async Task GetStationItemsReturnsCorrectNumberOfJourneysWhenLimitIsSet()
    {
        // Arrange
        var controller = new StationsController();

        // Act
        var result = await controller.GetStationItems(null, 5, null);

        // Assert
        // okResult.Value is anonymous type
        var okResult = result.Result as OkObjectResult;

        var stations =
            okResult?.Value?.GetType().GetProperty("stations")?.GetValue(okResult.Value)
            as List<Station>;

        Assert.IsNotType<NotFoundResult>(result.Result);
        Assert.IsNotType<BadRequestResult>(result.Result);
        Assert.NotNull(stations);

        Assert.Equal(5, stations.Count);
    }

    [Fact]
    public async Task GetStationItemsReturnsCorrectNumberOfJourneysWhenPageIsSet()
    {
        // Arrange
        var controller = new StationsController();

        // Act
        var result = await controller.GetStationItems(2, null, null);

        // Assert
        // okResult.Value is anonymous type
        var okResult = result.Result as OkObjectResult;

        var stations =
            okResult?.Value?.GetType().GetProperty("stations")?.GetValue(okResult.Value)
            as List<Station>;

        Assert.IsNotType<NotFoundResult>(result.Result);
        Assert.IsNotType<BadRequestResult>(result.Result);
        Assert.NotNull(stations);

        Assert.Equal(10, stations.Count);
        Assert.Equal(11, stations[0].FID);
    }

    [Fact]
    public async Task GetStationItemsReturnsCorrectNumberOfJourneysWhenSearchIsSet()
    {
        // Arrange
        var controller = new StationsController();

        // Act
        var result = await controller.GetStationItems(null, null, "Pasilan asema");

        // Assert
        // okResult.Value is anonymous type
        var okResult = result.Result as OkObjectResult;

        var stations =
            okResult?.Value?.GetType().GetProperty("stations")?.GetValue(okResult.Value)
            as List<Station>;

        Assert.IsNotType<NotFoundResult>(result.Result);
        Assert.IsNotType<BadRequestResult>(result.Result);
        Assert.NotNull(stations);

        Assert.Equal(1, stations.Count);
        Assert.Equal("Pasilan asema", stations[0].NameFIN);
        Assert.Equal("113", stations[0].ID);
    }

    // GET: api/Stations/5
    [Fact]
    public async Task GetStationReturnsCorrectStation()
    {
        // Arrange
        var controller = new StationsController();

        // Act
        var result = await controller.GetStation(111, null);

        // Assert
        // okResult.Value is anonymous type
        var okResult = result.Result as OkObjectResult;

        var station =
            okResult?.Value?.GetType().GetProperty("station")?.GetValue(okResult.Value) as Station;

        Assert.IsNotType<NotFoundResult>(result.Result);
        Assert.IsNotType<BadRequestResult>(result.Result);
        Assert.NotNull(station);

        Assert.Equal("001", station.ID);
        Assert.Equal("Kaivopuisto", station.NameFIN);
    }

    [Fact]
    public async Task GetStationReturnsCorrectNumberOfJourneysWhenMonthIsSet()
    {
        // Arrange
        var controller = new StationsController();

        // Act
        var result = await controller.GetStation(111, "may");

        // Assert
        // okResult.Value is anonymous type
        var okResult = result.Result as OkObjectResult;

        var station =
            okResult?.Value?.GetType().GetProperty("station")?.GetValue(okResult.Value) as Station;

        Assert.IsNotType<NotFoundResult>(result.Result);
        Assert.IsNotType<BadRequestResult>(result.Result);
        Assert.NotNull(station);

        Assert.Equal("001", station.ID);
        Assert.Equal("Kaivopuisto", station.NameFIN);
    }

    [Fact]
    public async Task GetStationReturnsCorrectNumberOfJourneysWhenMonthIsSetAndIsNotValid()
    {
        // Arrange
        var controller = new StationsController();

        // Act
        var result = await controller.GetStation(111, "mayy");

        // Assert
        // okResult.Value is anonymous type
        var okResult = result.Result as OkObjectResult;

        var station =
            okResult?.Value?.GetType().GetProperty("station")?.GetValue(okResult.Value) as Station;

        Assert.IsNotType<NotFoundResult>(result.Result);
        Assert.IsNotType<BadRequestResult>(result.Result);
        Assert.NotNull(station);

        Assert.Equal("001", station.ID);
        Assert.Equal("Kaivopuisto", station.NameFIN);
    }

    [Fact]
    public async Task GetStationReturnsCorrectNumberOfTop5JourneysByDefault()
    {
        // Arrange
        var controller = new StationsController();

        // Act
        var result = await controller.GetStation(111, null);

        // Assert
        // okResult.Value is anonymous type
        var okResult = result.Result as OkObjectResult;

        var station =
            okResult?.Value?.GetType().GetProperty("station")?.GetValue(okResult.Value) as Station;
        var top5DestinationStations =
            okResult?.Value?.GetType().GetProperty("top5ReturnStations")?.GetValue(okResult.Value)
            as List<string>;
        var top5OriginStations =
            okResult
                ?.Value?.GetType()
                .GetProperty("top5DepartureStations")
                ?.GetValue(okResult.Value) as List<string>;

        Assert.IsNotType<NotFoundResult>(result.Result);
        Assert.IsNotType<BadRequestResult>(result.Result);
        Assert.NotNull(station);

        Assert.Equal("001", station.ID);
        Assert.Equal("Kaivopuisto", station.NameFIN);
        Assert.Equal(5, top5DestinationStations!.Count);
        Assert.Equal(5, top5OriginStations!.Count);
    }

    [Fact]
    public void GetStationReturnsNotFoundWhenStationIdIsInvalid()
    {
        // Arrange
        var controller = new StationsController();

        // Act
        var result = controller.GetStation(-011, null);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result.Result);
    }

    // POST: api/Stations
    [Fact]
    public async Task PostStation()
    {
        // Arrange
        var controller = new StationsController();

        var station = new Station();

        station.ID = "000";
        station.NameFIN = "Testi";
        station.NameSWE = "Testi";
        station.NameENG = "Testi";
        station.AddressFIN = "Testi";
        station.AddressSWE = "Testi";
        station.CityFIN = "Testi";
        station.CitySWE = "Testi";
        station.Capacity = 0;
        station.Operator = "Testi";
        station.Latitude = "34";
        station.Longitude = "34";

        // Act
        var result = controller.PostStation(station);

        // Assert
        var okResult = result.Result.Result as CreatedAtActionResult;

        Assert.IsNotType<BadRequestResult>(result.Result.Result);
        Assert.IsType<CreatedAtActionResult>(result.Result.Result);

        var stationResult = okResult!.Value as Station;

        // Clean up
        await controller.DeleteStation(stationResult!.FID);
    }

    [Fact]
    public void PostStationReturnsBadRequestWhenStationIdIsInvalid()
    {
        // Arrange
        var controller = new StationsController();

        var station = new Station();

        station.ID = "-123";
        station.NameFIN = "Testi";
        station.NameSWE = "Testi";
        station.NameENG = "Testi";
        station.AddressFIN = "Testi";
        station.AddressSWE = "Testi";
        station.CityFIN = "Testi";
        station.CitySWE = "Testi";
        station.Capacity = 0;
        station.Operator = "Testi";
        station.Latitude = "34";
        station.Longitude = "34";

        // Act
        var result = controller.PostStation(station);

        // Assert
        Assert.IsType<BadRequestResult>(result.Result.Result);
    }

    [Fact]
    public void PostStationReturnsBadRequestWhenStationIdIsAlreadyInUse()
    {
        // Arrange
        var controller = new StationsController();

        var station = new Station();

        station.ID = "001";
        station.NameFIN = "Testi";
        station.NameSWE = "Testi";
        station.NameENG = "Testi";
        station.AddressFIN = "Testi";
        station.AddressSWE = "Testi";
        station.CityFIN = "Testi";
        station.CitySWE = "Testi";
        station.Capacity = 0;
        station.Operator = "Testi";
        station.Latitude = "34";
        station.Longitude = "34";

        // Act
        var result = controller.PostStation(station);

        // Assert
        Assert.IsType<BadRequestResult>(result.Result.Result);
    }

    [Fact]
    public void PostStationReturnsBadRequestWhenAllInvalid()
    {
        // Arrange
        var controller = new StationsController();

        var station = new Station();

        station.ID = "-223";
        station.NameFIN = "";
        station.NameSWE = "";
        station.NameENG = "";
        station.AddressFIN = "";
        station.AddressSWE = "";
        station.CityFIN = "";
        station.CitySWE = "";
        station.Capacity = -123;
        station.Operator = "";
        station.Latitude = "234234";
        station.Longitude = "3453456";

        // Act
        var result = controller.PostStation(station);

        // Assert
        Assert.IsType<BadRequestResult>(result.Result.Result);
    }

    [Fact]

    public void PostStationReturnsBadRequestWhenNameInvalid() {
        // Arrange
        var controller = new StationsController();

        var station = new Station();

        station.ID = "000";
        station.NameFIN = "";
        station.NameSWE = "";
        station.NameENG = "";
        station.AddressFIN = "Testi";
        station.AddressSWE = "Testi";
        station.CityFIN = "Testi";
        station.CitySWE = "Testi";
        station.Capacity = 0;
        station.Operator = "Testi";
        station.Latitude = "34";
        station.Longitude = "34";

        // Act
        var result = controller.PostStation(station);

        // Assert
        Assert.IsType<BadRequestResult>(result.Result.Result);
    }

    [Fact]
    public void PostStationReturnsBadRequestWhenAddressInvalid()
    {
        // Arrange
        var controller = new StationsController();

        var station = new Station();

        station.ID = "000";
        station.NameFIN = "Testi";
        station.NameSWE = "Testi";
        station.NameENG = "Testi";
        station.AddressFIN = "";
        station.AddressSWE = "";
        station.CityFIN = "Testi";
        station.CitySWE = "Testi";
        station.Capacity = 0;
        station.Operator = "Testi";
        station.Latitude = "34";
        station.Longitude = "34";

        // Act
        var result = controller.PostStation(station);

        // Assert
        Assert.IsType<BadRequestResult>(result.Result.Result);
    }

    [Fact]
    public void PostStationReturnsBadRequestWhenCityInvalid()
    {
        // Arrange
        var controller = new StationsController();

        var station = new Station();

        station.ID = "000";
        station.NameFIN = "Testi";
        station.NameSWE = "Testi";
        station.NameENG = "Testi";
        station.AddressFIN = "Testi";
        station.AddressSWE = "Testi";
        station.CityFIN = "";
        station.CitySWE = "";
        station.Capacity = 0;
        station.Operator = "Testi";
        station.Latitude = "34";
        station.Longitude = "34";

        // Act
        var result = controller.PostStation(station);

        // Assert
        Assert.IsType<BadRequestResult>(result.Result.Result);
    }

    [Fact]
    public void PostStationReturnsBadRequestWhenCapacityInvalid()
    {
        // Arrange
        var controller = new StationsController();

        var station = new Station();

        station.ID = "000";
        station.NameFIN = "Testi";
        station.NameSWE = "Testi";
        station.NameENG = "Testi";
        station.AddressFIN = "Testi";
        station.AddressSWE = "Testi";
        station.CityFIN = "Testi";
        station.CitySWE = "Testi";
        station.Capacity = -123;
        station.Operator = "Testi";
        station.Latitude = "34";
        station.Longitude = "34";

        // Act
        var result = controller.PostStation(station);

        // Assert
        Assert.IsType<BadRequestResult>(result.Result.Result);
    }

    [Fact]
    public void PostStationReturnsBadRequestWhenOperatorInvalid()
    {
        // Arrange
        var controller = new StationsController();

        var station = new Station();

        station.ID = "000";
        station.NameFIN = "Testi";
        station.NameSWE = "Testi";
        station.NameENG = "Testi";
        station.AddressFIN = "Testi";
        station.AddressSWE = "Testi";
        station.CityFIN = "Testi";
        station.CitySWE = "Testi";
        station.Capacity = 0;
        station.Operator = "";
        station.Latitude = "34";
        station.Longitude = "34";

        // Act
        var result = controller.PostStation(station);

        // Assert
        Assert.IsType<BadRequestResult>(result.Result.Result);
    }

    [Fact]
    public void PostStationReturnsBadRequestWhenLatitudeInvalid()
    {
        // Arrange
        var controller = new StationsController();

        var station = new Station();

        station.ID = "000";
        station.NameFIN = "Testi";
        station.NameSWE = "Testi";
        station.NameENG = "Testi";
        station.AddressFIN = "Testi";
        station.AddressSWE = "Testi";
        station.CityFIN = "Testi";
        station.CitySWE = "Testi";
        station.Capacity = 0;
        station.Operator = "Testi";
        station.Latitude = "234234";
        station.Longitude = "34";

        // Act
        var result = controller.PostStation(station);

        // Assert
        Assert.IsType<BadRequestResult>(result.Result.Result);
    }

    [Fact]
    public void PostStationReturnsBadRequestWhenLongitudeInvalid()
    {
        // Arrange
        var controller = new StationsController();

        var station = new Station();

        station.ID = "000";
        station.NameFIN = "Testi";
        station.NameSWE = "Testi";
        station.NameENG = "Testi";
        station.AddressFIN = "Testi";
        station.AddressSWE = "Testi";
        station.CityFIN = "Testi";
        station.CitySWE = "Testi";
        station.Capacity = 0;
        station.Operator = "Testi";
        station.Latitude = "34";
        station.Longitude = "3453456";

        // Act
        var result = controller.PostStation(station);

        // Assert
        Assert.IsType<BadRequestResult>(result.Result.Result);
    }   

    // PUT: api/Stations/5
    [Fact]
    public async Task PutStation()
    {
        // Arrange
        var controller = new StationsController();

        var station = new Station();

        station.ID = "000";
        station.NameFIN = "Testi";
        station.NameSWE = "Testi";
        station.NameENG = "Testi";
        station.AddressFIN = "Testi";
        station.AddressSWE = "Testi";
        station.CityFIN = "Testi";
        station.CitySWE = "Testi";
        station.Capacity = 0;
        station.Operator = "Testi";
        station.Latitude = "34";
        station.Longitude = "34";

        // Act
        var result = controller.PostStation(station);

        // Assert
        Assert.IsNotType<BadRequestResult>(result.Result.Result);
        Assert.IsType<CreatedAtActionResult>(result.Result.Result);

        var okResult = result.Result.Result as CreatedAtActionResult;

        var stationResult = okResult!.Value as Station;

        // Act
        stationResult!.NameFIN = "Testi2";
        stationResult.NameSWE = "Testi2";
        stationResult.NameENG = "Testi2";
        stationResult.AddressFIN = "Testi2";
        stationResult.AddressSWE = "Testi2";
        stationResult.CityFIN = "Testi2";
        stationResult.CitySWE = "Testi2";
        stationResult.Capacity = 1;
        stationResult.Operator = "Testi2";
        stationResult.Latitude = "35";
        stationResult.Longitude = "35";

        var result2 = controller.PutStation(stationResult.FID, stationResult);

        // Assert
        var okResult2 = result2.Result as OkObjectResult;

        Assert.IsNotType<BadRequestResult>(result2.Result);
        Assert.IsType<OkObjectResult>(result2.Result);

        var stationResult2 = okResult2!.Value as Station;

        Assert.Equal("Testi2", stationResult2!.NameFIN);
        Assert.Equal("Testi2", stationResult2.NameSWE);
        Assert.Equal("Testi2", stationResult2.NameENG);
        Assert.Equal("Testi2", stationResult2.AddressFIN);
        Assert.Equal("Testi2", stationResult2.AddressSWE);
        Assert.Equal("Testi2", stationResult2.CityFIN);
        Assert.Equal("Testi2", stationResult2.CitySWE);
        Assert.Equal(1, stationResult2.Capacity);
        Assert.Equal("Testi2", stationResult2.Operator);
        Assert.Equal("35", stationResult2.Latitude);
        Assert.Equal("35", stationResult2.Longitude);

        // Clean up
        await controller.DeleteStation(stationResult.FID);
    }

    [Fact]
    public void PutStationReturnsBadRequestWhenStationIdIsInvalid()
    {
        // Arrange
        var controller = new StationsController();

        var station = new Station();

        station.ID = "-123";
        station.NameFIN = "Testi";
        station.NameSWE = "Testi";
        station.NameENG = "Testi";
        station.AddressFIN = "Testi";
        station.AddressSWE = "Testi";
        station.CityFIN = "Testi";
        station.CitySWE = "Testi";
        station.Capacity = 0;
        station.Operator = "Testi";
        station.Latitude = "34";
        station.Longitude = "34";

        // Act
        var result = controller.PutStation(0, station);

        // Assert
        Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public async Task PutStationReturnsBadRequestWhenNameIsInvalid() {
        // Arrange
        var controller = new StationsController();

        var station = new Station();

        station.ID = "000";
        station.NameFIN = "Testi";
        station.NameSWE = "Testi";
        station.NameENG = "Testi";
        station.AddressFIN = "Testi";
        station.AddressSWE = "Testi";
        station.CityFIN = "Testi";
        station.CitySWE = "Testi";
        station.Capacity = 0;
        station.Operator = "Testi";
        station.Latitude = "34";
        station.Longitude = "34";

        // Act
        var result = controller.PostStation(station);

        // Assert
        Assert.IsNotType<BadRequestResult>(result.Result.Result);
        Assert.IsType<CreatedAtActionResult>(result.Result.Result);

        var okResult = result.Result.Result as CreatedAtActionResult;

        var stationResult = okResult!.Value as Station;

        // Act
        stationResult!.NameFIN = "";
        stationResult.NameSWE = "";
        stationResult.NameENG = "";
        stationResult.AddressFIN = "Testi2";
        stationResult.AddressSWE = "Testi2";
        stationResult.CityFIN = "Testi2";
        stationResult.CitySWE = "Testi2";
        stationResult.Capacity = 1;
        stationResult.Operator = "Testi2";
        stationResult.Latitude = "35";
        stationResult.Longitude = "35";

        var result2 = controller.PutStation(stationResult.FID, stationResult);

        // Assert

        Assert.IsType<BadRequestResult>(result2.Result);
        Assert.IsNotType<OkObjectResult>(result2.Result);

        // Clean up
        await controller.DeleteStation(stationResult.FID);
    }

    [Fact]
    public async Task PutStationReturnsBadRequestWhenAddressIsInvalid() {
        // Arrange
        var controller = new StationsController();

        var station = new Station();

        station.ID = "000";
        station.NameFIN = "Testi";
        station.NameSWE = "Testi";
        station.NameENG = "Testi";
        station.AddressFIN = "Testi";
        station.AddressSWE = "Testi";
        station.CityFIN = "Testi";
        station.CitySWE = "Testi";
        station.Capacity = 0;
        station.Operator = "Testi";
        station.Latitude = "34";
        station.Longitude = "34";

        // Act
        var result = controller.PostStation(station);

        // Assert
        Assert.IsNotType<BadRequestResult>(result.Result.Result);
        Assert.IsType<CreatedAtActionResult>(result.Result.Result);

        var okResult = result.Result.Result as CreatedAtActionResult;

        var stationResult = okResult!.Value as Station;

        // Act
        stationResult!.NameFIN = "Testi2";
        stationResult.NameSWE = "Testi2";
        stationResult.NameENG = "Testi2";
        stationResult.AddressFIN = "";
        stationResult.AddressSWE = "";
        stationResult.CityFIN = "Testi2";
        stationResult.CitySWE = "Testi2";
        stationResult.Capacity = 1;
        stationResult.Operator = "Testi2";
        stationResult.Latitude = "35";
        stationResult.Longitude = "35";

        var result2 = controller.PutStation(stationResult.FID, stationResult);

        // Assert

        Assert.IsType<BadRequestResult>(result2.Result);
        Assert.IsNotType<OkObjectResult>(result2.Result);

        // Clean up
        await controller.DeleteStation(stationResult.FID);
    }

    [Fact]
    public async Task PutStationReturnsBadRequestWhenCityIsInvalid() {
        // Arrange
        var controller = new StationsController();

        var station = new Station();

        station.ID = "000";
        station.NameFIN = "Testi";
        station.NameSWE = "Testi";
        station.NameENG = "Testi";
        station.AddressFIN = "Testi";
        station.AddressSWE = "Testi";
        station.CityFIN = "Testi";
        station.CitySWE = "Testi";
        station.Capacity = 0;
        station.Operator = "Testi";
        station.Latitude = "34";
        station.Longitude = "34";

        // Act
        var result = controller.PostStation(station);

        // Assert
        Assert.IsNotType<BadRequestResult>(result.Result.Result);
        Assert.IsType<CreatedAtActionResult>(result.Result.Result);

        var okResult = result.Result.Result as CreatedAtActionResult;

        var stationResult = okResult!.Value as Station;

        // Act
        stationResult!.NameFIN = "Testi2";
        stationResult.NameSWE = "Testi2";
        stationResult.NameENG = "Testi2";
        stationResult.AddressFIN = "Testi2";
        stationResult.AddressSWE = "Testi2";
        stationResult.CityFIN = "";
        stationResult.CitySWE = "";
        stationResult.Capacity = 1;
        stationResult.Operator = "Testi2";
        stationResult.Latitude = "35";
        stationResult.Longitude = "35";

        var result2 = controller.PutStation(stationResult.FID, stationResult);

        // Assert

        Assert.IsType<BadRequestResult>(result2.Result);
        Assert.IsNotType<OkObjectResult>(result2.Result);

        // Clean up
        await controller.DeleteStation(stationResult.FID);
    }

    [Fact]
    public async Task PutStationReturnsBadRequestWhenCapacityIsInvalid() {
        // Arrange
        var controller = new StationsController();

        var station = new Station();

        station.ID = "000";
        station.NameFIN = "Testi";
        station.NameSWE = "Testi";
        station.NameENG = "Testi";
        station.AddressFIN = "Testi";
        station.AddressSWE = "Testi";
        station.CityFIN = "Testi";
        station.CitySWE = "Testi";
        station.Capacity = 0;
        station.Operator = "Testi";
        station.Latitude = "34";
        station.Longitude = "34";

        // Act
        var result = controller.PostStation(station);

        // Assert
        Assert.IsNotType<BadRequestResult>(result.Result.Result);
        Assert.IsType<CreatedAtActionResult>(result.Result.Result);

        var okResult = result.Result.Result as CreatedAtActionResult;

        var stationResult = okResult!.Value as Station;

        // Act
        stationResult!.NameFIN = "Testi2";
        stationResult.NameSWE = "Testi2";
        stationResult.NameENG = "Testi2";
        stationResult.AddressFIN = "Testi2";
        stationResult.AddressSWE = "Testi2";
        stationResult.CityFIN = "Testi2";
        stationResult.CitySWE = "Testi2";
        stationResult.Capacity = -1;
        stationResult.Operator = "Testi2";
        stationResult.Latitude = "35";
        stationResult.Longitude = "35";

        var result2 = controller.PutStation(stationResult.FID, stationResult);

        // Assert

        Assert.IsType<BadRequestResult>(result2.Result);
        Assert.IsNotType<OkObjectResult>(result2.Result);

        // Clean up
        await controller.DeleteStation(stationResult.FID);
    }

    [Fact]
    public async Task PutStationReturnsBadRequestWhenLatitudeIsInvalid() {
        // Arrange
        var controller = new StationsController();

        var station = new Station();

        station.ID = "000";
        station.NameFIN = "Testi";
        station.NameSWE = "Testi";
        station.NameENG = "Testi";
        station.AddressFIN = "Testi";
        station.AddressSWE = "Testi";
        station.CityFIN = "Testi";
        station.CitySWE = "Testi";
        station.Capacity = 0;
        station.Operator = "Testi";
        station.Latitude = "34";
        station.Longitude = "34";

        // Act
        var result = controller.PostStation(station);

        // Assert
        Assert.IsNotType<BadRequestResult>(result.Result.Result);
        Assert.IsType<CreatedAtActionResult>(result.Result.Result);

        var okResult = result.Result.Result as CreatedAtActionResult;

        var stationResult = okResult!.Value as Station;

        // Act
        stationResult!.NameFIN = "Testi2";
        stationResult.NameSWE = "Testi2";
        stationResult.NameENG = "Testi2";
        stationResult.AddressFIN = "Testi2";
        stationResult.AddressSWE = "Testi2";
        stationResult.CityFIN = "Testi2";
        stationResult.CitySWE = "Testi2";
        stationResult.Capacity = 1;
        stationResult.Operator = "Testi2";
        stationResult.Latitude = "24355";
        stationResult.Longitude = "35";

        var result2 = controller.PutStation(stationResult.FID, stationResult);

        // Assert

        Assert.IsType<BadRequestResult>(result2.Result);
        Assert.IsNotType<OkObjectResult>(result2.Result);

        // Clean up
        await controller.DeleteStation(stationResult.FID);
    }

    [Fact]
    public async Task PutStationReturnsBadRequestWhenLongitudeIsInvalid() {
        // Arrange
        var controller = new StationsController();

        var station = new Station();

        station.ID = "000";
        station.NameFIN = "Testi";
        station.NameSWE = "Testi";
        station.NameENG = "Testi";
        station.AddressFIN = "Testi";
        station.AddressSWE = "Testi";
        station.CityFIN = "Testi";
        station.CitySWE = "Testi";
        station.Capacity = 0;
        station.Operator = "Testi";
        station.Latitude = "34";
        station.Longitude = "34";

        // Act
        var result = controller.PostStation(station);

        // Assert
        Assert.IsNotType<BadRequestResult>(result.Result.Result);
        Assert.IsType<CreatedAtActionResult>(result.Result.Result);

        var okResult = result.Result.Result as CreatedAtActionResult;

        var stationResult = okResult!.Value as Station;

        // Act
        stationResult!.NameFIN = "Testi2";
        stationResult.NameSWE = "Testi2";
        stationResult.NameENG = "Testi2";
        stationResult.AddressFIN = "Testi2";
        stationResult.AddressSWE = "Testi2";
        stationResult.CityFIN = "Testi2";
        stationResult.CitySWE = "Testi2";
        stationResult.Capacity = 1;
        stationResult.Operator = "Testi2";
        stationResult.Latitude = "35";
        stationResult.Longitude = "24355";

        var result2 = controller.PutStation(stationResult.FID, stationResult);

        // Assert

        Assert.IsType<BadRequestResult>(result2.Result);
        Assert.IsNotType<OkObjectResult>(result2.Result);

        // Clean up
        await controller.DeleteStation(stationResult.FID);
    }

    [Fact]
    public async Task PutStationReturnsBadRequestWhenOperatorIsInvalid() {
        // Arrange
        var controller = new StationsController();

        var station = new Station();

        station.ID = "000";
        station.NameFIN = "Testi";
        station.NameSWE = "Testi";
        station.NameENG = "Testi";
        station.AddressFIN = "Testi";
        station.AddressSWE = "Testi";
        station.CityFIN = "Testi";
        station.CitySWE = "Testi";
        station.Capacity = 0;
        station.Operator = "Testi";
        station.Latitude = "34";
        station.Longitude = "34";

        // Act
        var result = controller.PostStation(station);

        // Assert
        Assert.IsNotType<BadRequestResult>(result.Result.Result);
        Assert.IsType<CreatedAtActionResult>(result.Result.Result);

        var okResult = result.Result.Result as CreatedAtActionResult;

        var stationResult = okResult!.Value as Station;

        // Act
        stationResult!.NameFIN = "Testi2";
        stationResult.NameSWE = "Testi2";
        stationResult.NameENG = "Testi2";
        stationResult.AddressFIN = "Testi2";
        stationResult.AddressSWE = "Testi2";
        stationResult.CityFIN = "Testi2";
        stationResult.CitySWE = "Testi2";
        stationResult.Capacity = 1;
        stationResult.Operator = "";
        stationResult.Latitude = "35";
        stationResult.Longitude = "24355";

        var result2 = controller.PutStation(stationResult.FID, stationResult);

        // Assert

        Assert.IsType<BadRequestResult>(result2.Result);
        Assert.IsNotType<OkObjectResult>(result2.Result);

        // Clean up
        await controller.DeleteStation(stationResult.FID);
    }

    // DELETE: api/Stations/5
    [Fact]
    public void DeleteStation()
    {
        // Arrange
        var controller = new StationsController();

        var station = new Station();

        station.ID = "000";
        station.NameFIN = "Testi";
        station.NameSWE = "Testi";
        station.NameENG = "Testi";
        station.AddressFIN = "Testi";
        station.AddressSWE = "Testi";
        station.CityFIN = "Testi";
        station.CitySWE = "Testi";
        station.Capacity = 0;
        station.Operator = "Testi";
        station.Latitude = "34";
        station.Longitude = "34";

        // Act
        var result = controller.PostStation(station);

        // Assert
        Assert.IsNotType<BadRequestResult>(result.Result.Result);
        Assert.IsType<CreatedAtActionResult>(result.Result.Result);

        var okResult = result.Result.Result as CreatedAtActionResult;

        var stationResult = okResult!.Value as Station;

        // Act
        var result2 = controller.DeleteStation(stationResult!.FID);

        // Assert
        Assert.IsNotType<BadRequestResult>(result2.Result);
        Assert.IsType<OkResult>(result2.Result);
    }
}
