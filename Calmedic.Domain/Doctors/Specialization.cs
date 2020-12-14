namespace Calmedic.Domain
{
    public class Specialization : AuditEntity
    {
        public Specialization()
        {
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}