using UnitTestDemo.Models;
using UnitTestDemo.Repositories;

namespace UnitTestDemo.Services
{
    public class CarService : ICarService
    {
        public ExampleService exampleService;
        private readonly ICarRepository _carRepository;
        private readonly IInspectionsRepository _inspectionsRepository;

        public CarService(ICarRepository carRepository, IInspectionsRepository inspectionsRepository)
        {
            _carRepository = carRepository;
            _inspectionsRepository = inspectionsRepository;
            exampleService = new ExampleService();
        }

        public async Task<Car> CreateCar(Car car)
        {
            car.Id = Guid.NewGuid();

            await _carRepository.CreateCar(car).ConfigureAwait(false);

            return car;
        }

        public async Task<CarModel?> GetCar(Guid? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException();
            }

            var car = await _carRepository.GetCar(id).ConfigureAwait(false);

            if (car == null)
            {
                return null;  
            }

            var inspections = await _inspectionsRepository.GetInspections(id).ConfigureAwait(false);

            return new CarModel
            {
                Id = car.Id,
                Brand = car.Brand,
                LicenseNumber = car.LicenseNumber,
                Model = car.Model,
                Owner = car.Owner,
                Inspections = inspections
            };
        }

        public async Task<IEnumerable<Car>> GetCars()
        {
            var cars = await _carRepository.GetCars().ConfigureAwait(false);

            return cars;
        }

        public async Task<Inspection> GetLatestInspection(Guid id)
        {
            var inspection = await _inspectionsRepository.GetLatestInspection(id).ConfigureAwait(false);

            return inspection;
        }

        #region 

        // avoid static, cannot be mocked
        public (int, bool) Calculator(int num1, int numn2)
        {
            var result = num1 + numn2;

            var somethingIrrelevant = SuperAdvancedCalculationWithDatabaseRequests();

            return (result, somethingIrrelevant);
        }

        private static bool SuperAdvancedCalculationWithDatabaseRequests()
        { 
            // Call database
            return true;
        }

        // Avoid new
        public Person GetPerson()
        {
            // Cannot mock
            var person = new Person().GetPersonFromDb();

            person.FirstName = "Peter";
            person.LastName = "Parker";

            return person;
        }

        #endregion

        public string GetName()
        {
            return exampleService.GetName();
        }
    }
}
