using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Mediachase.Commerce;
using OxxCommerceStarterKit.Core.Extensions;
using OxxCommerceStarterKit.Web.Models.Blocks.Contracts;
using OxxCommerceStarterKit.Web.Models.FindModels;
using OxxCommerceStarterKit.Web.Models.ViewModels;

namespace OxxCommerceStarterKit.Web.Models.Catalog
{
    [CatalogContentType(GUID = "252BFD8F-EF0C-49FB-86D8-31E2436D1461", MetaClassName = "DigitalCameraSKU",
       DisplayName = "Camera",
       Description = "Digital Camera",
       GroupName = "Camera"
       )]
    public class DigitalCameraSkuContent : VariationContent, IFacetBrand, IIndexableContent, IProductListViewModelInitializer, IResourceable
    {

        // Multi lang
        [Display(Name = "Description", Order = 10)]
        [CultureSpecific]
        [Searchable]
        public virtual XhtmlString Info_Description { get; set; }

        // Multi lang
        [Display(Name = "Overview", Order = 20)]
        [CultureSpecific]
        [Searchable]
        public virtual XhtmlString Overview { get; set; }

        [Display(Name = "Resolution",
           Description = "Number in megapixels",
           Order = 30)]
        public virtual decimal Resolution { get; set; }

        [Display(Name = "Lens Mount",
           Description = "",
           Order = 40)]
        public virtual string LensMount { get; set; }


        [Display(Name = "Camera Format",
           Description = "Full-frame, 1.3 crop etc.",
           Order = 50)]
        public virtual string CameraFormat { get; set; }

        [Display(Name = "File Format",
           Description = "Information like image formats, video formats etc.",
           Order = 60)]
        public virtual string FileFormat { get; set; }

        [Display(Name = "Connectivity",
           Description = "Wifi, bluetooth ect.",
           Order = 70)]
        public virtual string Connectivity { get; set; }

        [Display(Name = "Memory Card Type",
           Description = "",
           Order = 70)]
        public virtual string MemoryCardType { get; set; }

        [Display(Name = "Battery",
           Description = "",
           Order = 80)]
        public virtual string Battery { get; set; }

        //[Display(Name = "Weight",
        //  Description = "",
        //  Order = 90)]
        //public virtual string Weight { get; set; }







        [Display(Name = "Show in lists",
         Description = "Default is true, set to false to hide product from lists. The product can still be linked to and found through search.",
         Order = 100,
         GroupName = SystemTabNames.PageHeader)]
        [DefaultValue(true)]
        public virtual bool ShowInList { get; set; }

        // Same for all languages
        [Display(Name = "Facet Brand",
            Order = 18)]
        public virtual string Facet_Brand { get; set; }

        public FindModels.FindProduct GetFindProduct(Mediachase.Commerce.IMarket market)
        {
            var language = (Language == null ? string.Empty : Language.Name);
            var findProduct = new DigitalCameraFindProduct(this, language);

            findProduct.Description = Info_Description;
            findProduct.Overview = Overview;
            findProduct.ShowInList = ShowInList;
            EPiServer.Commerce.SpecializedProperties.Price defaultPrice = this.GetDefaultPrice();
            findProduct.DefaultPrice = this.GetDisplayPrice(market);
            findProduct.DefaultPriceAmount = this.GetDefaultPriceAmount(market);
            findProduct.DiscountedPrice = this.GetDiscountDisplayPrice(defaultPrice, market);
            findProduct.CustomerClubPrice = this.GetCustomerClubDisplayPrice(market);

            return findProduct;
        }

        public bool ShouldIndex()
        {
            return !(StopPublish != null && StopPublish < DateTime.Now);
        }

        public ProductListViewModel Populate(Mediachase.Commerce.IMarket getCurrentMarket)
        {
            UrlResolver urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();

            ProductListViewModel productListViewModel = new ProductListViewModel
            {
                Code = this.Code,
                ContentLink = this.ContentLink,
                DisplayName = this.DisplayName,
                Description = Info_Description,
                ProductUrl = urlResolver.GetUrl(ContentLink),
                ImageUrl = this.GetDefaultImage(),
                PriceString = this.GetDisplayPrice(getCurrentMarket),
                BrandName = Facet_Brand,
                //Country = Country,
                ContentType = this.GetType().Name
            };
            ICurrentMarket currentMarket = ServiceLocator.Current.GetInstance<ICurrentMarket>();
            productListViewModel.PriceAmount = this.GetDefaultPriceAmount(currentMarket.GetCurrentMarket());
            return productListViewModel;
        }

        public virtual string ContentAssetIdInternal { get; set; }
        public Guid ContentAssetsID
        {
            get
            {
                Guid assetId;
                if (Guid.TryParse(ContentAssetIdInternal, out assetId))
                    return assetId;
                return Guid.Empty;
            }
            set
            {
                ContentAssetIdInternal = value.ToString();
                this.ThrowIfReadOnly();
                IsModified = true;
            }
        }
    }
}