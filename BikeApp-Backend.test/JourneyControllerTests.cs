using Backend_BikeApp.Controllers;
using Backend_BikeApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BikeApp_Backend.test
{
    public class JourneyControllerTests
    {
        // GET: api/Journeys
        [Fact]
        public void GetJourneysReturnsCorrectNumberOfJourneysByDefault()
        {
            // Arrange
            var controller = new JourneysController();

            // Act

            var result = controller.GetJourneyItems(
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            );

            // Assert
            var okResult = result.Result.Result as OkObjectResult;

            // okResult.Value is anonymous type
            var journeys =
                okResult?.Value?.GetType().GetProperty("journeys")?.GetValue(okResult.Value)
                as List<Journey>;

            Assert.IsNotType<NotFoundResult>(result.Result.Result);
            Assert.IsNotType<BadRequestResult>(result.Result.Result);
            Assert.NotNull(journeys);

            Assert.Equal(10, journeys.Count);
        }

        [Fact]
        public void GetJourneysReturnsCorrectNumberOfJourneysWhenPageIsSet()
        {
            // Arrange
            var controller = new JourneysController();

            // Act

            var result = controller.GetJourneyItems(
                2,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            );

            // Assert
            var okResult = result.Result.Result as OkObjectResult;

            // okResult.Value is anonymous type
            var journeys =
                okResult?.Value?.GetType().GetProperty("journeys")?.GetValue(okResult.Value)
                as List<Journey>;

            Assert.IsNotType<NotFoundResult>(result.Result.Result);
            Assert.IsNotType<BadRequestResult>(result.Result.Result);
            Assert.NotNull(journeys);

            Assert.Equal(10, journeys.Count);
            Assert.Equal(11, journeys[0].Id);
        }

        [Fact]
        public void GetJourneysReturnsCorrectNumberOfJourneysWhenLimitIsSet()
        {
            // Arrange
            var controller = new JourneysController();

            // Act

            var result = controller.GetJourneyItems(
                null,
                5,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            );

            // Assert
            var okResult = result.Result.Result as OkObjectResult;

            // okResult.Value is anonymous type
            var journeys =
                okResult?.Value?.GetType().GetProperty("journeys")?.GetValue(okResult.Value)
                as List<Journey>;

            Assert.IsNotType<NotFoundResult>(result.Result.Result);
            Assert.IsNotType<BadRequestResult>(result.Result.Result);
            Assert.NotNull(journeys);

            Assert.Equal(5, journeys.Count);
        }

        [Fact]
        public void GetJourneysReturnsCorrectNumberOfJourneysWhenSearchIsSet()
        {
            // Arrange
            var controller = new JourneysController();

            // Act

            var result = controller.GetJourneyItems(
                null,
                null,
                "Pasilan asema",
                null,
                null,
                null,
                null,
                null,
                null
            );

            // Assert
            var okResult = result.Result.Result as OkObjectResult;

            // okResult.Value is anonymous type
            var journeys =
                okResult?.Value?.GetType().GetProperty("journeys")?.GetValue(okResult.Value)
                as List<Journey>;

            Assert.IsNotType<NotFoundResult>(result.Result.Result);
            Assert.IsNotType<BadRequestResult>(result.Result.Result);
            Assert.NotNull(journeys);

            Assert.Contains(
                journeys,
                j =>
                    j.DepartureStationName!.Contains("Pasilan asema")
                    || j.ReturnStationName!.Contains("Pasilan asema")
            );
        }

        [Fact]
        public void GetJourneysReturnsCorrectNumberOfJourneysWhenOrderByIsSet()
        {
            // Arrange
            var controller = new JourneysController();

            // Act

            var result = controller.GetJourneyItems(
                null,
                null,
                null,
                "DepartureStationId",
                null,
                null,
                null,
                null,
                null
            );

            // Assert
            var okResult = result.Result.Result as OkObjectResult;

            // okResult.Value is anonymous type
            var journeys =
                okResult?.Value?.GetType().GetProperty("journeys")?.GetValue(okResult.Value)
                as List<Journey>;

            Assert.IsNotType<NotFoundResult>(result.Result.Result);
            Assert.IsNotType<BadRequestResult>(result.Result.Result);
            Assert.NotNull(journeys);

            Assert.Equal("001", journeys[0].DepartureStationId);
        }

        [Fact]
        public void GetJourneysReturnsCorrectNumberOfJourneysWhenOrderByDescIsSet()
        {
            // Arrange
            var controller = new JourneysController();

            // Act

            var result = controller.GetJourneyItems(
                null,
                null,
                null,
                "DepartureStationName",
                "desc",
                null,
                null,
                null,
                null
            );

            // Assert
            var okResult = result.Result.Result as OkObjectResult;

            // okResult.Value is anonymous type
            var journeys =
                okResult?.Value?.GetType().GetProperty("journeys")?.GetValue(okResult.Value)
                as List<Journey>;

            Assert.IsNotType<NotFoundResult>(result.Result.Result);
            Assert.IsNotType<BadRequestResult>(result.Result.Result);
            Assert.NotNull(journeys);

            Assert.Equal("041", journeys[1].DepartureStationId);
        }

        [Fact]
        public void GetJourneysReturnsCorrectNumberOfJourneysWhenMinDurationIsSet()
        {
            // Arrange
            var controller = new JourneysController();

            // Act

            var result = controller.GetJourneyItems(
                null,
                null,
                null,
                null,
                "asc",
                3954628,
                null,
                null,
                null
            );

            // Assert
            var okResult = result.Result.Result as OkObjectResult;

            // okResult.Value is anonymous type
            var journeys =
                okResult?.Value?.GetType().GetProperty("journeys")?.GetValue(okResult.Value)
                as List<Journey>;

            Assert.IsNotType<NotFoundResult>(result.Result.Result);
            Assert.IsNotType<BadRequestResult>(result.Result.Result);
            Assert.NotNull(journeys);

            Assert.All(journeys, j => Assert.True(j.Duration >= 3954628));
        }

        [Fact]
        public void GetJourneysReturnsCorrectNumberOfJourneysWhenMaxDurationIsSet()
        {
            // Arrange
            var controller = new JourneysController();

            // Act

            var result = controller.GetJourneyItems(
                null,
                null,
                null,
                null,
                "desc",
                null,
                11,
                null,
                null
            );

            // Assert
            var okResult = result.Result.Result as OkObjectResult;

            // okResult.Value is anonymous type
            var journeys =
                okResult?.Value?.GetType().GetProperty("journeys")?.GetValue(okResult.Value)
                as List<Journey>;

            Assert.IsNotType<NotFoundResult>(result.Result.Result);
            Assert.IsNotType<BadRequestResult>(result.Result.Result);
            Assert.NotNull(journeys);

            Assert.All(journeys, j => Assert.True(j.Duration <= 11));
        }

        [Fact]
        public void GetJourneysReturnsCorrectNumberOfJourneysWhenMinDistanceIsSet()
        {
            // Arrange
            var controller = new JourneysController();

            // Act

            var result = controller.GetJourneyItems(
                null,
                null,
                null,
                null,
                "asc",
                null,
                null,
                1998252,
                null
            );

            // Assert
            var okResult = result.Result.Result as OkObjectResult;

            // okResult.Value is anonymous type
            var journeys =
                okResult?.Value?.GetType().GetProperty("journeys")?.GetValue(okResult.Value)
                as List<Journey>;

            Assert.IsNotType<NotFoundResult>(result.Result.Result);
            Assert.IsNotType<BadRequestResult>(result.Result.Result);
            Assert.NotNull(journeys);

            Assert.All(journeys, j => Assert.True(j.CoveredDistance >= 1998252));
        }

        [Fact]
        public void GetJourneysReturnsCorrectNumberOfJourneysWhenMaxDistanceIsSet()
        {
            // Arrange
            var controller = new JourneysController();

            // Act

            var result = controller.GetJourneyItems(
                null,
                null,
                null,
                null,
                "desc",
                null,
                null,
                null,
                11
            );

            // Assert
            var okResult = result.Result.Result as OkObjectResult;

            // okResult.Value is anonymous type
            var journeys =
                okResult?.Value?.GetType().GetProperty("journeys")?.GetValue(okResult.Value)
                as List<Journey>;

            Assert.IsNotType<NotFoundResult>(result.Result.Result);
            Assert.IsNotType<BadRequestResult>(result.Result.Result);
            Assert.NotNull(journeys);

            Assert.All(journeys, j => Assert.True(j.CoveredDistance <= 11));
        }

        [Fact]
        public void GetJourneysReturnsCorrectNumberOfJourneysWhenMinDurationAndMaxDurationAreSet()
        {
            // Arrange
            var controller = new JourneysController();

            // Act

            var result = controller.GetJourneyItems(
                null,
                null,
                null,
                null,
                "asc",
                11,
                14,
                null,
                null
            );

            // Assert
            var okResult = result.Result.Result as OkObjectResult;

            // okResult.Value is anonymous type
            var journeys =
                okResult?.Value?.GetType().GetProperty("journeys")?.GetValue(okResult.Value)
                as List<Journey>;

            Assert.IsNotType<NotFoundResult>(result.Result.Result);
            Assert.IsNotType<BadRequestResult>(result.Result.Result);
            Assert.NotNull(journeys);

            Assert.All(journeys, j => Assert.True(j.Duration >= 11 && j.Duration <= 14));
        }

        [Fact]
        public void GetJourneysReturnsCorrectNumberOfJourneysWhenMinDistanceAndMaxDistanceAreSet()
        {
            // Arrange
            var controller = new JourneysController();

            // Act

            var result = controller.GetJourneyItems(
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                11,
                14
            );

            // Assert
            var okResult = result.Result.Result as OkObjectResult;

            // okResult.Value is anonymous type
            var journeys =
                okResult?.Value?.GetType().GetProperty("journeys")?.GetValue(okResult.Value)
                as List<Journey>;

            Assert.IsNotType<NotFoundResult>(result.Result.Result);
            Assert.IsNotType<BadRequestResult>(result.Result.Result);
            Assert.NotNull(journeys);

            Assert.All(
                journeys,
                j => Assert.True(j.CoveredDistance >= 11 && j.CoveredDistance <= 14)
            );
        }

        [Fact]
        public void GetJourneyById()
        {
            // Arrange
            var controller = new JourneysController();

            // Act
            var result = controller.GetJourney(1);

            // Assert
            var okResult = result.Result.Result as OkObjectResult;

            Assert.IsNotType<NotFoundResult>(result.Result);
            Assert.IsNotType<BadRequestResult>(result.Result);
            Assert.NotNull(okResult);

            var journey = okResult.Value as Journey;

            Assert.Equal(1, journey!.Id);
        }

        [Fact]
        public void GetJourneyByIdReturnsNotFound()
        {
            // Arrange
            var controller = new JourneysController();

            // Act
            var result = controller.GetJourney(0);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result.Result);
        }




        // POST: api/Journeys
        [Fact]
        public async void PostJourney()
        {
            // Arrange
            var controller = new JourneysController();

            // Act
            Journey journey = new Journey();

            journey.DepartureTime = DateTime.Now;
            journey.ReturnTime = DateTime.Now;
            journey.DepartureStationId = "001";
            journey.DepartureStationName = "Test";
            journey.ReturnStationId = "002";
            journey.ReturnStationName = "Test";
            journey.CoveredDistance = 100;
            journey.Duration = 10;

            var result = controller.PostJourney(journey);

            // Assert
            var okResult = result.Result.Result as CreatedAtActionResult;

            Assert.IsNotType<NotFoundResult>(result.Result.Result);
            Assert.IsNotType<BadRequestResult>(result.Result.Result);
            Assert.NotNull(okResult);

            var journeyResult = okResult.Value as Journey;

            Assert.Equal(100, journeyResult!.CoveredDistance);

            // Clean up
            await controller.DeleteJourney(journeyResult!.Id);
        }

        [Fact]
        public void PostJourneyBadStationId()
        {
            // Arrange
            var controller = new JourneysController();

            // Act
            Journey journey = new Journey();

            journey.DepartureTime = DateTime.Now;
            journey.ReturnTime = DateTime.Now;
            journey.DepartureStationId = "000";
            journey.DepartureStationName = "Test";
            journey.ReturnStationId = "000";
            journey.ReturnStationName = "Test";
            journey.CoveredDistance = 100;
            journey.Duration = 10;

            var result = controller.PostJourney(journey);

            // Assert
            Assert.IsNotType<NotFoundResult>(result.Result.Result);
            Assert.IsNotType<CreatedAtActionResult>(result.Result.Result);
            Assert.IsType<BadRequestResult>(result.Result.Result);
        }

        [Fact]
        public void PostJourneyBadDistance() {    
            // Arrange
            var controller = new JourneysController();

            // Act
            Journey journey = new Journey();

            journey.DepartureTime = DateTime.Now;
            journey.ReturnTime = DateTime.Now;
            journey.DepartureStationId = "001";
            journey.DepartureStationName = "Test";
            journey.ReturnStationId = "002";
            journey.ReturnStationName = "Test";
            journey.CoveredDistance = 0;
            journey.Duration = 10;

            var result = controller.PostJourney(journey);

            // Assert
            Assert.IsNotType<NotFoundResult>(result.Result.Result);
            Assert.IsNotType<CreatedAtActionResult>(result.Result.Result);
            Assert.IsType<BadRequestResult>(result.Result.Result);
        }

        [Fact]
        public void PostJourneyBadDuration() {    
            // Arrange
            var controller = new JourneysController();

            // Act
            Journey journey = new Journey();

            journey.DepartureTime = DateTime.Now;
            journey.ReturnTime = DateTime.Now;
            journey.DepartureStationId = "001";
            journey.DepartureStationName = "Test";
            journey.ReturnStationId = "002";
            journey.ReturnStationName = "Test";
            journey.CoveredDistance = 100;
            journey.Duration = 0;

            var result = controller.PostJourney(journey);

            // Assert
            Assert.IsNotType<NotFoundResult>(result.Result.Result);
            Assert.IsNotType<CreatedAtActionResult>(result.Result.Result);
            Assert.IsType<BadRequestResult>(result.Result.Result);
        }



        // PUT: api/Journeys/5
        [Fact]
        public async void PutJourney()
        {
            // Arrange
            var controller = new JourneysController();

            // Act
            Journey journey = new Journey();

            journey.DepartureTime = DateTime.Now;
            journey.ReturnTime = DateTime.Now;
            journey.DepartureStationId = "001";
            journey.DepartureStationName = "Test";
            journey.ReturnStationId = "002";
            journey.ReturnStationName = "Test";
            journey.CoveredDistance = 100;
            journey.Duration = 10;

            var result = controller.PostJourney(journey);

            // Assert
            var okResult = result.Result.Result as CreatedAtActionResult;

            Assert.IsNotType<NotFoundResult>(result.Result.Result);
            Assert.IsNotType<BadRequestResult>(result.Result.Result);
            Assert.NotNull(okResult);

            var journeyResult = okResult.Value as Journey;

            Assert.Equal(100, journeyResult!.CoveredDistance);

            // Act
            journeyResult.CoveredDistance = 200;

            var result2 = controller.PutJourneyAsync(journeyResult.Id, journeyResult);

            // Assert
            var okResult2 = result2.Result as OkObjectResult;

            Assert.IsNotType<NotFoundResult>(result2.Result);
            Assert.IsNotType<BadRequestResult>(result2.Result);
            Assert.NotNull(okResult2);

            var journeyResult2 = okResult2.Value as Journey;

            Assert.Equal(200, journeyResult2!.CoveredDistance);

            // Clean up
            await controller.DeleteJourney(journeyResult2!.Id);
        }

        [Fact]
        public async void PutJourneyBadStationId()
        {
            // Arrange
            var controller = new JourneysController();

            // Act
            Journey journey = new Journey();

            journey.DepartureTime = DateTime.Now;
            journey.ReturnTime = DateTime.Now;
            journey.DepartureStationId = "001";
            journey.DepartureStationName = "Test";
            journey.ReturnStationId = "002";
            journey.ReturnStationName = "Test";
            journey.CoveredDistance = 100;
            journey.Duration = 10;

            var result = controller.PostJourney(journey);

            // Assert
            var okResult = result.Result.Result as CreatedAtActionResult;

            Assert.IsNotType<NotFoundResult>(result.Result.Result);
            Assert.IsNotType<BadRequestResult>(result.Result.Result);
            Assert.NotNull(okResult);

            var journeyResult = okResult.Value as Journey;

            Assert.Equal(100, journeyResult!.CoveredDistance);

            // Act
            journeyResult.DepartureStationId = "000";
            journeyResult.ReturnStationId = "000";

            var result2 = controller.PutJourneyAsync(journeyResult.Id, journeyResult);

            // Assert
            Assert.IsNotType<NotFoundResult>(result2.Result);
            Assert.IsNotType<OkObjectResult>(result2.Result);
            Assert.IsType<BadRequestResult>(result2.Result);

            // Clean up
            await controller.DeleteJourney(journeyResult!.Id);
        }

        [Fact]
        public async void PutJourneyBadDistance()
        {
            // Arrange
            var controller = new JourneysController();

            // Act
            Journey journey = new Journey();

            journey.DepartureTime = DateTime.Now;
            journey.ReturnTime = DateTime.Now;
            journey.DepartureStationId = "001";
            journey.DepartureStationName = "Test";
            journey.ReturnStationId = "002";
            journey.ReturnStationName = "Test";
            journey.CoveredDistance = 100;
            journey.Duration = 10;

            var result = controller.PostJourney(journey);

            // Assert
            var okResult = result.Result.Result as CreatedAtActionResult;

            Assert.IsNotType<NotFoundResult>(result.Result.Result);
            Assert.IsNotType<BadRequestResult>(result.Result.Result);
            Assert.NotNull(okResult);

            var journeyResult = okResult.Value as Journey;

            Assert.Equal(100, journeyResult!.CoveredDistance);

            // Act
            journeyResult.CoveredDistance = 0;

            var result2 = controller.PutJourneyAsync(journeyResult.Id, journeyResult);

            // Assert
            Assert.IsNotType<NotFoundResult>(result2.Result);
            Assert.IsNotType<OkObjectResult>(result2.Result);
            Assert.IsType<BadRequestResult>(result2.Result);

            // Clean up
            await controller.DeleteJourney(journeyResult!.Id);
        }

        [Fact]
        public async void PutJourneyBadDuration()
        {
            // Arrange
            var controller = new JourneysController();

            // Act
            Journey journey = new Journey();

            journey.DepartureTime = DateTime.Now;
            journey.ReturnTime = DateTime.Now;
            journey.DepartureStationId = "001";
            journey.DepartureStationName = "Test";
            journey.ReturnStationId = "002";
            journey.ReturnStationName = "Test";
            journey.CoveredDistance = 100;
            journey.Duration = 10;

            var result = controller.PostJourney(journey);

            // Assert
            var okResult = result.Result.Result as CreatedAtActionResult;

            Assert.IsNotType<NotFoundResult>(result.Result.Result);
            Assert.IsNotType<BadRequestResult>(result.Result.Result);
            Assert.NotNull(okResult);

            var journeyResult = okResult.Value as Journey;

            Assert.Equal(100, journeyResult!.CoveredDistance);

            // Act
            journeyResult.Duration = 0;

            var result2 = controller.PutJourneyAsync(journeyResult.Id, journeyResult);

            // Assert
            Assert.IsNotType<NotFoundResult>(result2.Result);
            Assert.IsNotType<OkObjectResult>(result2.Result);
            Assert.IsType<BadRequestResult>(result2.Result);

            // Clean up
            await controller.DeleteJourney(journeyResult!.Id);
        }

        [Fact]
        public async void PutJourneyBadId() {
            // Arrange
            var controller = new JourneysController();

            // Act
            Journey journey = new Journey();

            journey.DepartureTime = DateTime.Now;
            journey.ReturnTime = DateTime.Now;
            journey.DepartureStationId = "001";
            journey.DepartureStationName = "Test";
            journey.ReturnStationId = "002";
            journey.ReturnStationName = "Test";
            journey.CoveredDistance = 100;
            journey.Duration = 10;

            var result = controller.PostJourney(journey);

            // Assert
            var okResult = result.Result.Result as CreatedAtActionResult;

            Assert.IsNotType<NotFoundResult>(result.Result.Result);
            Assert.IsNotType<BadRequestResult>(result.Result.Result);
            Assert.NotNull(okResult);

            var journeyResult = okResult.Value as Journey;

            Assert.Equal(100, journeyResult!.CoveredDistance);

            // Act

            var result2 = controller.PutJourneyAsync(-45612, journeyResult);

            // Assert
            Assert.IsNotType<NotFoundResult>(result2.Result);
            Assert.IsNotType<OkObjectResult>(result2.Result);
            Assert.IsType<BadRequestObjectResult>(result2.Result);

            // Clean up
            await controller.DeleteJourney(journeyResult!.Id);
        }

        // DELETE: api/Journeys/5
        [Fact]
        public void DeleteJourney() {
            // Arrange
            var controller = new JourneysController();

            // Act
            Journey journey = new Journey();

            journey.DepartureTime = DateTime.Now;
            journey.ReturnTime = DateTime.Now;
            journey.DepartureStationId = "001";
            journey.DepartureStationName = "Test";
            journey.ReturnStationId = "002";
            journey.ReturnStationName = "Test";
            journey.CoveredDistance = 100;
            journey.Duration = 10;

            var result = controller.PostJourney(journey);

            // Assert
            var okResult = result.Result.Result as CreatedAtActionResult;

            Assert.IsNotType<NotFoundResult>(result.Result.Result);
            Assert.IsNotType<BadRequestResult>(result.Result.Result);
            Assert.NotNull(okResult);

            var journeyResult = okResult.Value as Journey;

            Assert.Equal(100, journeyResult!.CoveredDistance);

            // Act
            var result2 = controller.DeleteJourney(journeyResult.Id);

            // Assert
            Assert.IsNotType<NotFoundResult>(result2.Result);
            Assert.IsNotType<BadRequestResult>(result2.Result);
            Assert.IsType<OkResult>(result2.Result);
        }
       
    }
}
