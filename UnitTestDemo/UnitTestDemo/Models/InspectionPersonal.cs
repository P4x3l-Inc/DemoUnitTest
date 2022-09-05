namespace UnitTestDemo.Models
{
    public class InspectionPersonal
    {
        internal InspectionPersonal() { }

        public InspectionPersonal(Guid id)
        {
            Id = id;
        }

        public virtual Guid Id { get; private set; }

        public virtual string? FirstName { get; private set; }

        public virtual string? LastName { get; private set; }
    }
}
