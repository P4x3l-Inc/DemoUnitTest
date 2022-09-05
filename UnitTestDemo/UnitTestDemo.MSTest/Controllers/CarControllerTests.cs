using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using UnitTestDemo.Controllers;
using UnitTestDemo.Models;
using UnitTestDemo.Services;

namespace UnitTestDemo.MSTest.Controllers
{
    [TestClass]
    public class CarControllerTests
    {
        private static ICarService _carService;
        private static CarController _controller;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _carService = Substitute.For<ICarService>();
            _controller = new CarController(_carService);
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnAllCars()
        {
            // Arrange
            var cars = new List<Car>
            {
                Substitute.For<Car>(),
                Substitute.For<Car>(),
                Substitute.For<Car>(),
            };
            _carService.GetCars().Returns(cars);

            // Act
            var result = await _controller.GetAll().ConfigureAwait(false);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var resultItem = result as OkObjectResult;
            resultItem.Should().NotBeNull();
            var resultItemValue = resultItem?.Value as IEnumerable<Car>;
            resultItemValue.Should().NotBeNull();
            resultItemValue.Should().HaveCount(3);
        }

        [TestMethod]
        public async Task Get_ShouldReturnBadRequest_WhenIdIsEmpty()
        {
            // Arrange
            var id = Guid.Empty;

            // Act
            var result = await _controller.Get(id);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public async Task Get_ShouldReturnNotFound_WhenCarDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            CarModel? car = null;
            _carService.GetCar(Arg.Any<Guid>()).Returns(car);

            // Act
            var result = await _controller.Get(id);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task Get_ShouldReturnCorrectCar_WhenCarExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var car = new CarModel
            {
                Id = id,
            };
            _carService.GetCar(Arg.Any<Guid>()).Returns(car);

            // Act
            var result = await _controller.Get(id);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var resultItem = result as OkObjectResult;
            resultItem.Should().NotBeNull();
            var resultItemValue = resultItem?.Value as Car;
            resultItemValue.Should().NotBeNull();
            resultItemValue?.Id.Should().Be(car.Id);
        }
    }
}
