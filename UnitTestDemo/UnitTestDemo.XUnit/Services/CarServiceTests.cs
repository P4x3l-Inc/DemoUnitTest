using FluentAssertions;
using Moq;
using NSubstitute;
using UnitTestDemo.Models;
using UnitTestDemo.Repositories;
using UnitTestDemo.Services;

namespace UnitTestDemo.XUnit.Services
{
    public class CarServiceTests
    {
        private ICarRepository _carRepositorySub;
        private IInspectionsRepository _inspectionRepositorySub;
        private CarService _serviceSub;

        private Mock<ICarRepository> _carRepositoryMoq;
        private Mock<IInspectionsRepository> _inspectionRepositoryMoq;
        private CarService _serviceMoq;

        public CarServiceTests()
        {
            _carRepositorySub = Substitute.For<ICarRepository>();
            _inspectionRepositorySub = Substitute.For<IInspectionsRepository>();
            _serviceSub = new CarService(_carRepositorySub, _inspectionRepositorySub);

            _carRepositoryMoq = new Mock<ICarRepository>();
            _inspectionRepositoryMoq = new Mock<IInspectionsRepository>();
            _serviceMoq = new CarService(_carRepositoryMoq.Object, _inspectionRepositoryMoq.Object);
        }

        #region Mock & Substitute
        [Fact]
        public async Task GetCar_ShouldReturnNull_WhenCarNotFound_Moq()
        {
            // Arrange
            var id = Guid.NewGuid();
            Car? car = null;
            _carRepositoryMoq.Setup(x => x.GetCar(It.IsAny<Guid?>())).ReturnsAsync(car);

            // Act
            await _serviceMoq.GetCar(id).ConfigureAwait(false);

            // Assert
            _carRepositoryMoq.Verify(x => x.GetCar(It.Is<Guid?>(y => y == id)), Times.Once);
            _inspectionRepositoryMoq.Verify(x => x.GetInspections(It.IsAny<Guid?>()), Times.Never);
        }


        [Fact]
        public async Task GetCar_ShouldReturnNull_WhenCarNotFound_Substitute()
        {
            // Arrange
            var id = Guid.NewGuid();
            Car? car = null;
            _carRepositorySub.GetCar(Arg.Any<Guid?>()).Returns(car);

            // Act
            await _serviceSub.GetCar(id).ConfigureAwait(false);

            // Assert
            await _carRepositorySub.Received(1).GetCar(Arg.Any<Guid?>());
            await _inspectionRepositorySub.DidNotReceive().GetInspections(Arg.Any<Guid?>());
        }

        [Fact]
        public async Task GetCar_ShouldReturnCar_WhenCarExists_Moq()
        {
            // Arrange
            var id = Guid.NewGuid();

            var car = new Car
            {
                Id = id,
            };
            _carRepositoryMoq.Setup(x => x.GetCar(It.IsAny<Guid?>())).ReturnsAsync(car);

            var inspections = new List<Inspection>
            {
                new Inspection
                {
                    Id = Guid.NewGuid(),
                    InspectedCarId = id,
                    Approved = false,
                }
            };
            _inspectionRepositoryMoq.Setup(x => x.GetInspections(It.IsAny<Guid?>())).ReturnsAsync(inspections);

            // Act
            await _serviceMoq.GetCar(id).ConfigureAwait(false);

            // Assert
            _carRepositoryMoq.Verify(x => x.GetCar(It.Is<Guid?>(y => y == id)), Times.Once);
            _inspectionRepositoryMoq.Verify(x => x.GetInspections(It.Is<Guid?>(y => y == id)), Times.Once);
        }

        [Fact]
        public async Task GetCar_ShouldReturnCar_WhenCarExists_Substitute()
        {
            // Arrange
            var id = Guid.NewGuid();

            var car = new Car
            {
                Id = id,
            };
            _carRepositorySub.GetCar(Arg.Any<Guid?>()).Returns(car);

            var inspections = new List<Inspection>
            {
                new Inspection
                {
                    Id = Guid.NewGuid(),
                    InspectedCarId = id,
                    Approved = false,
                }
            };
            _inspectionRepositorySub.GetInspections(Arg.Any<Guid?>()).Returns(inspections);

            // Act
            await _serviceSub.GetCar(id).ConfigureAwait(false);

            // Assert
            await _carRepositorySub.Received(1).GetCar(Arg.Any<Guid?>());
            await _inspectionRepositorySub.Received(1).GetInspections(Arg.Any<Guid?>());
        }
        #endregion

        #region private set
        /// <summary>
        /// Requires (internal?) empty constructor for mocks
        /// visiblity for internals in csproj for Mock namespace (DynamicProxyGenAssembly2) and project under test
        /// To mock class attribute it has to be virtual
        /// Nested mocks does not work for NSubstiute, Moq is required
        /// </summary>
        [Fact]
        public async Task GetLatestInspection_ShouldGetInspectinFacility()
        {
            // Arrange
            var id = Guid.NewGuid();
            var firstName = "Test";
            var lastName = "Testsson";
            var person = new Mock<InspectionPersonal>();
            person.Setup(x => x.FirstName).Returns(firstName);
            person.Setup(x => x.LastName).Returns(lastName);

            var personal = new List<InspectionPersonal>
            {
                person.Object,
            };

            var inspectionFacility = new Mock<Facility>();
            inspectionFacility.Setup(x => x.Personal).Returns(personal);
            var inspection = new Mock<Inspection>();
            inspection.Setup(x => x.InspectionFacility).Returns(inspectionFacility.Object);

            _inspectionRepositoryMoq.Setup(x => x.GetLatestInspection(It.IsAny<Guid>())).ReturnsAsync(inspection.Object);

            /*
            var inspections = new List<Inspection>
            {
                new Inspection
                {
                    InspectionFacility = new List<Facility>
                    {
                        new Facility
                        {
                            Personal = new List<InspectionPersonal>
                            {
                                new InspectionPersonal
                                {
                                    FirstName = firstName,
                                    LastName = lastName,
                                },
                            }
                        }
                    }
                }
            };
            */

            // Act
            var result = await _serviceMoq.GetLatestInspection(id).ConfigureAwait(false);

            // Assert
            var resultPersonal = result?.InspectionFacility?.Personal?.FirstOrDefault();
            resultPersonal.Should().NotBeNull();
            resultPersonal?.FirstName.Should().Be(firstName);
            resultPersonal?.LastName.Should().Be(lastName);
        }

        #endregion

        #region Not using DI through constructor
        [Fact]
        public void GetName_ShouldGetName()
        {
            // Arrange
            var name = "Not Test Testsson";

            var exampleService = new Mock<ExampleService>();
            exampleService.Setup(x => x.GetName()).Returns(name);

            _serviceMoq.exampleService = exampleService.Object;

            // Act
            var result = _serviceMoq.GetName();

            // Assert
            result.Should().Be(name);
        }
        #endregion
    }
}
