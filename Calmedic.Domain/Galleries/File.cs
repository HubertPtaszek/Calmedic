namespace Calmedic.Domain
{
    public class File : AuditEntity
    {
        public File()
        {
        }

        public string Name { get; set; }
        public string ThumbName { get; set; }
        public string OriginalName { get; set; }
        public string ContentType { get; set; }
        public int? Duration { get; set; }
        public int ClinicId { get; set; }
        public virtual Clinic Clinic { get; set; }
    }
}