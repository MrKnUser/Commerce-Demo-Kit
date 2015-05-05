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
using Lucene.Net.Index;
using Mediachase.Commerce.Catalog;
using OxxCommerceStarterKit.Web.Models.Catalog;
using OxxCommerceStarterKit.Web.Reviews;

namespace OxxCommerceStarterKit.Web.Api
{
    public class ReviewData
    {
        public int Rating { get; set; }
        public int ProductId { get; set; }
        public string Text { get; set; }
    }
        
    
    public class ReviewController : BaseApiController
    {
        private ReferenceConverter _referenceConverter;
        private IContentRepository _contentRepository;
        private ContentAssetHelper _contentAssetHelper;

        public ReviewController(ReferenceConverter referenceConverter, IContentRepository contentRepository, ContentAssetHelper contentAssetHelper)
        {
            _referenceConverter = referenceConverter;
            _contentRepository = contentRepository;
            _contentAssetHelper = contentAssetHelper;
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public object Get(int id)
        {
           
            ContentReference contentLink = _referenceConverter.GetContentLink(id, CatalogContentType.CatalogEntry, 0);
            var contentAssetFolder = _contentAssetHelper.GetAssetFolder(contentLink);
            if (contentAssetFolder == null)
                return null;
            List<Review> reviews = new List<Review>();
            if (Language == null)
            {
                reviews = _contentRepository.GetChildren<Review>(contentAssetFolder.ContentLink).ToList();
            }
            else
                reviews = _contentRepository.GetChildren<Review>(contentAssetFolder.ContentLink, new LanguageSelector(Language)).ToList();


            return reviews;
        }

        // POST api/<controller>
        public void Post(ReviewData reviewData)
        {
            string language = Language;

            ContentReference contentLink = _referenceConverter.GetContentLink(reviewData.ProductId, CatalogContentType.CatalogEntry, 0);
            WineSKUContent product = _contentRepository.Get<WineSKUContent>(contentLink, new CultureInfo(language));
            ContentAssetFolder assetFolder = _contentAssetHelper.GetOrCreateAssetFolder(product.ContentLink);
            Review newReview = _contentRepository.GetDefault<Review>(assetFolder.ContentLink);
            newReview.Rating = reviewData.Rating;
            newReview.Text = reviewData.Text;
            //TODO: Get currentuser, need to be logedin to post review
            newReview.UserDisplayName = "Sveinung";
            newReview.Name = newReview.UserDisplayName + "(" + DateTime.Now.ToShortDateString() + ")";
            _contentRepository.Save(newReview, SaveAction.Publish, AccessLevel.NoAccess);
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