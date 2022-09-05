namespace UnitTestDemo.Models
{
    public class Car
    {
        public Guid Id { get; set; }

        public string LicenseNumber { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public Person Owner { get; set; }
    }
}
