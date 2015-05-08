using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using Mediachase.Commerce.Catalog;
using OxxCommerceStarterKit.Web.Models.Catalog;
using OxxCommerceStarterKit.Web.Reviews;

namespace OxxCommerceStarterKit.Web.Api
{
    public class ReviewData
    {
        public int Rating { get; set; }
        public int ContentId { get; set; }
        public string Heading { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
        public string UserDisplayName { get; set; }
        public DateTime ReviewDate { get; set; }
    }

    public class ReviewResult
    {
        public List<ReviewData> Reviews { get; set; }
        public int TotalNumberOfReviews { get; set; }
        public double AverageReview { get; set; }
    }


 
    public class ReviewController : BaseApiController
    {
        private readonly ReferenceConverter _referenceConverter;
        private readonly IContentRepository _contentRepository;
        private readonly ContentAssetHelper _contentAssetHelper;

        public ReviewController(ReferenceConverter referenceConverter, IContentRepository contentRepository, ContentAssetHelper contentAssetHelper)
        {
            _referenceConverter = referenceConverter;
            _contentRepository = contentRepository;
            _contentAssetHelper = contentAssetHelper;
        }

        //TODO: This should take in id, and type. Then it can be used on both commerce products and pagedata

        public object Get(int id)
        {

            ReviewResult reviewResult = new ReviewResult();
            reviewResult.AverageReview = 0;
            reviewResult.TotalNumberOfReviews = 0;
            reviewResult.Reviews = new List<ReviewData>();
            ContentReference contentLink = _referenceConverter.GetContentLink(id, CatalogContentType.CatalogEntry, 0);
            var contentAssetFolder = _contentAssetHelper.GetAssetFolder(contentLink);
            if (contentAssetFolder == null)
                return reviewResult;
            List<Review> reviews = new List<Review>();
            if (Language == null)
            {
                reviews = _contentRepository.GetChildren<Review>(contentAssetFolder.ContentLink).ToList();
            }
            else
                reviews = _contentRepository.GetChildren<Review>(contentAssetFolder.ContentLink, new LanguageSelector(Language)).ToList();

         
            if (reviews.Any())
            {
                reviewResult.Reviews = reviews.Select(x => new ReviewData
                {
                    ContentId = x.ContentId,
                    Rating = x.Rating,
                    Heading = x.Heading,
                    Text = x.Text,
                    UserName = x.UserDisplayName,
                    ReviewDate = x.ReviewDate

                }).ToList();
                reviewResult.TotalNumberOfReviews = reviewResult.Reviews.Count;
                reviewResult.AverageReview = reviewResult.Reviews.Average(x => x.Rating);
            }

            return reviewResult;
        }

        // POST api/<controller>
        public ReviewData Post(ReviewData reviewData)
        {
            string language = Language;

            ContentReference contentLink = _referenceConverter.GetContentLink(reviewData.ContentId, CatalogContentType.CatalogEntry,0);
            EntryContentBase product = _contentRepository.Get<EntryContentBase>(contentLink, new CultureInfo(language));
            ContentAssetFolder assetFolder = _contentAssetHelper.GetOrCreateAssetFolder(product.ContentLink);
            Review newReview = _contentRepository.GetDefault<Review>(assetFolder.ContentLink);
            newReview.Rating = reviewData.Rating;
            newReview.Heading = reviewData.Heading;
            newReview.Text = reviewData.Text;
            //TODO: Get currentuser, need to be logedin to post review
            newReview.UserDisplayName = "Sveinung";
            newReview.Name = newReview.UserDisplayName + "(" + DateTime.Now.ToShortDateString() + ")";
            newReview.ReviewDate = DateTime.Now;
            ContentReference cf = _contentRepository.Save(newReview, SaveAction.Publish, AccessLevel.NoAccess);
            Review postedReview = _contentRepository.Get<Review>(cf, new CultureInfo(language));
            ReviewData review = new ReviewData
            {
                ContentId = postedReview.ContentId,
                Rating = postedReview.Rating,
                Heading = postedReview.Heading,
                Text = postedReview.Text,
                UserName = postedReview.UserDisplayName,
                ReviewDate = postedReview.ReviewDate

            };
            return review;

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