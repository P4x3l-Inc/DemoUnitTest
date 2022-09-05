﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using UnitTestDemo.Controllers;
using UnitTestDemo.Models;
using UnitTestDemo.Services;

namespace UnitTestDemo.NUnit.Controllers
{
    [TestFixture]
    public class CarControllerTests
    {
        private static ICarService _carService;
        private static CarController _controller;

        [SetUp]
        public void Setup()
        {
            _carService = Substitute.For<ICarService>();
            _controller = new CarController(_carService);
        }

        [Test]
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

        [Test]
        public async Task Get_ShouldReturnBadRequest_WhenIdIsEmpty()
        {
            // Arrange
            var id = Guid.Empty;

            // Act
            var result = await _controller.Get(id);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
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

        [Test]
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

        [TestCase(0, true)]
        [TestCase(1, false)]
        [TestCase(2, true)]
        public void IsEven_ShouldCheckIfNumberIsEven(int number, bool expectedResult)
        {
            // Act
            var result = _controller.IsEven(number);

            // Assert
            result.Should().Be(expectedResult);
        }
    }
}
