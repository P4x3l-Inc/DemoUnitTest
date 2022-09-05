namespace UnitTestDemo.Models
{
    public class Facility
    {
        internal Facility() { }

        public Facility(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }

        public string? City { get; private set; }

        public string? Name { get; private set; }

        public virtual IEnumerable<InspectionPersonal>? Personal { get; private set; }
    }
}
