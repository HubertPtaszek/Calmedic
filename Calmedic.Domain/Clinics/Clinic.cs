using System;

namespace Calmedic.Domain
{
    public class Clinic : AuditEntity
    {
        public Clinic()
        { }

        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan OpenFrom { get; set; }
        public TimeSpan OpenTo { get; set; }
    }
}
