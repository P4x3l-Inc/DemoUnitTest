namespace UnitTestDemo.Models
{
    public class Inspection
    {
        internal Inspection() { }

        public Inspection (Facility inspectionFacility)
        {
            InspectionFacility = inspectionFacility;
        }

        public Guid Id { get; set; }

        public Guid InspectedCarId { get; set; }

        public bool Approved { get; set; }

        public virtual Facility InspectionFacility { get; private set; }
    }
}
