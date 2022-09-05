using UnitTestDemo.Models;

namespace UnitTestDemo.Services
{
    public interface ICarService
    {
        Task<Car> CreateCar(Car car);

        Task<CarModel?> GetCar(Guid? id);

        Task<IEnumerable<Car>> GetCars();

        Task<Inspection> GetLatestInspection(Guid id);
    }
}
