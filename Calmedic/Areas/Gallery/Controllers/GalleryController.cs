using Calmedic.Application;
using Calmedic.Dictionaries;
using Calmedic.Utils;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Calmedic.Areas.Gallery.Controllers
{
    [Area(AreaNames.Gallery_Area)]
    public class GalleryController : AppController
    {
        #region Dependencies


        #endregion Dependencies

        public GalleryController()
        {
        }

        [AppRoleAuthorization(AppRoleType.Clinic)]
        public ActionResult Index()
        {
            return View(new GalleryListVM());
        }
    }
}