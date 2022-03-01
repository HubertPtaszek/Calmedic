namespace Calmedic.Application
{
    public class GalleyItemVM
    {
        public GalleyItemVM()
        { }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ThumbName { get; set; }
        public string OriginalName { get; set; }
        public string ContentType { get; set; }
        public int ClinicId { get; set; }
    }
}