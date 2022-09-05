using UnitTestDemo.Models;

namespace UnitTestDemo.Repositories
{
    public interface ICarRepository
    {
        Task<Car> CreateCar(Car car);

        Task<Car> GetCar(Guid? id);

        Task<IEnumerable<Car>> GetCars();
    }
}
