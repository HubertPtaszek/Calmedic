using Calmedic.Dictionaries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calmedic.Domain
{
    public class DisplaySequence : AuditEntity
    {
        public DisplaySequence()
        { 
        }

        public MediaType MediaType { get; set; }
        public int FileId { get; set; }
        public virtual File File { get; set; }
        public int? DisplayTime { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
        public int ClinicId { get; set; }
        public virtual Clinic Clinic { get; set; }
    }
}
