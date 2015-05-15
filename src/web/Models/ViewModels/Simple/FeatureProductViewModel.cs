using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Mediachase.Commerce.Catalog;
using OxxCommerceStarterKit.Core.Extensions;
using OxxCommerceStarterKit.Web.Models.Blocks;

namespace OxxCommerceStarterKit.Web.Models.ViewModels.Simple
{
    public class FeatureProductViewModel
    {
        readonly FeatureProductBlock _featureProductBlock;
        private string ImageUrl { get; set; }
        private string ProductUrl { get; set; }
        private XhtmlString FeatureText { get; set; }

        public FeatureProductViewModel(FeatureProductBlock featureProductBlock)
        {
            //var referenceConverter = ServiceLocator.Current.GetInstance<ReferenceConverter>();
            _featureProductBlock = featureProductBlock;
            var _contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            UrlResolver urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
            EntryContentBase product = _contentRepository.Get<EntryContentBase>(_featureProductBlock.MenuFeatureLink);

            ImageUrl = GetImageUrl(product, urlResolver);
            FeatureText = _featureProductBlock.MenuFeatureText;
            ProductUrl = urlResolver.GetUrl(product.ContentLink);
        }

        private string GetImageUrl(EntryContentBase product, UrlResolver urlResolver)
        {

            var mediaReferences = product.AssetImageUrls();

            if (mediaReferences != null && mediaReferences.Any())
            {
                var media = mediaReferences.FirstOrDefault();
                var mediaUrl = urlResolver.GetUrl(media);
                return mediaUrl + "?preset=imagenarrow";
            }
            return string.Empty;

        }
    }
}