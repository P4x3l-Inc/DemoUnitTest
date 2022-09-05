namespace UnitTestDemo.Models
{
    public class CarModel : Car
    {
        public IEnumerable<Inspection> Inspections { get; set; }
    }
}
