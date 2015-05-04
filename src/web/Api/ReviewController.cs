using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Catalog;
using OxxCommerceStarterKit.Web.Models.Catalog;
using OxxCommerceStarterKit.Web.Reviews;

namespace OxxCommerceStarterKit.Web.Api
{
    public class ReviewController : BaseApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post(int id)
        {
            string language = Language;

            ReferenceConverter _referenceConverter = ServiceLocator.Current.GetInstance<ReferenceConverter>();
            IContentRepository _contentRepo = ServiceLocator.Current.GetInstance<IContentRepository>();
            ContentAssetHelper _contentAssetHelper = ServiceLocator.Current.GetInstance<ContentAssetHelper>();

            ContentReference contentLink = _referenceConverter.GetContentLink(id, CatalogContentType.CatalogEntry, 0);
            WineSKUContent product = _contentRepo.Get<WineSKUContent>(contentLink, new CultureInfo(language));
            ContentAssetFolder assetFolder = _contentAssetHelper.GetOrCreateAssetFolder(product.ContentLink);
            Review newReview = _contentRepo.GetDefault<Review>(assetFolder.ContentLink);
            newReview.Rating = 5;
            newReview.Text = "This is such a nice product";
            newReview.UserDisplayName = "Sveinung";
            newReview.Name = newReview.UserDisplayName + "(" + DateTime.Now.ToShortDateString() + ")";
            _contentRepo.Save(newReview, SaveAction.Publish, AccessLevel.NoAccess);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}