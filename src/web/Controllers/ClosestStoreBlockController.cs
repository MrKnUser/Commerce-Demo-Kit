using System;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Web.Mvc;
using OxxCommerceStarterKit.Web.Models.Blocks;
using EPiServer.Framework.DataAnnotations;
using OxxCommerceStarterKit.Web.Models.ViewModels;
using OxxCommerceStarterKit.Web.Models.PageTypes;
using EPiServer.Find.Framework;
using EPiServer.Find;
using EPiServer.Find.Cms;
using OxxCommerceStarterKit.Web.Helpers;

namespace OxxCommerceStarterKit.Web.Controllers
{
    [TemplateDescriptor(AvailableWithoutTag = true, Default = true, Inherited = false)]
    public class ClosestStoreBlockController : BlockController<ClosestStoreBlock>
    {
        private IContentLoader contentLoader; 

        public ClosestStoreBlockController(IContentLoader loader)
        {
            contentLoader = loader;
        }
        public override ActionResult Index(ClosestStoreBlock currentBlock)
        {
            var model = new ClosestStoreViewModel();
            model.Heading = currentBlock.Heading;

            var results = SearchClient.Instance.Search<StoreLocationPage>()
                .OrderByDescending(x => x.Coordinates)
                .DistanceFrom(UserLocation)
                .Take(1)
                .FilterForVisitor()
                .StaticallyCacheFor(new TimeSpan(0, 1, 0))
                .GetContentResult();

            if (results.TotalMatching > 0)
            {
                StoreLocationPage location = results.FirstOrDefault();
                model.Link = location.ContentLink;
                model.Name = location.Name;
                model.Description = location.FullAddress;
            }

            return PartialView("ClosestStore", model);
        }

        protected GeoLocation UserLocation
        {
            get
            {
                var geoLocationResult = GeoPosition.GetUsersLocation();
                return new GeoLocation(geoLocationResult.Location.Latitude, geoLocationResult.Location.Longitude);
            }
        }
    }
}
