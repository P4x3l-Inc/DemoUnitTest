using UnitTestDemo.Models;

namespace UnitTestDemo.Repositories
{
    public interface IInspectionsRepository
    {
        Task<IEnumerable<Inspection>> GetInspections(Guid? id);

        Task<Inspection> GetLatestInspection(Guid? id);
    }
}