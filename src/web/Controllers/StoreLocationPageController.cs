using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using OxxCommerceStarterKit.Web.Models.PageTypes;
using OxxCommerceStarterKit.Web.Helpers;

namespace OxxCommerceStarterKit.Web.Controllers
{
    public class StoreLocationPageController : PageControllerBase<StoreLocationPage>
    {
        public ActionResult Index(StoreLocationPage currentPage)
        {
            
            var model = CreatePageViewModel(currentPage);
            
            return View(model);
            
        }
    }
}