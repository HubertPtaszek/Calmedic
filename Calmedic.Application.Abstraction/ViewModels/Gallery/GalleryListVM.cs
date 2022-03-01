using System.Collections.Generic;

namespace Calmedic.Application
{
    public class GalleryListVM
    {
        public GalleryListVM()
        {
        }

        public int ClinicId { get; set; }
        public List<GalleyItemVM> Files { get; set; }
    }
}