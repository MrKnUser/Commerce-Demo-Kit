using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Mediachase.Commerce;
using OxxCommerceStarterKit.Core.Extensions;
using OxxCommerceStarterKit.Web.Models.Blocks.Contracts;
using OxxCommerceStarterKit.Web.Models.Catalog.Base;
using OxxCommerceStarterKit.Web.Models.FindModels;
using OxxCommerceStarterKit.Web.Models.ViewModels;

namespace OxxCommerceStarterKit.Web.Models.Catalog
{
     [CatalogContentType(GUID = "1F13773D-14B0-4F32-9E04-20BA9B2B29F1", MetaClassName = "GenericProductContent",
       DisplayName = "Generic Product",
       Description = "A generic product with generic fields",
       GroupName = WebGlobal.GroupNames.Commerce
       )]
    public class GenericProductContent : ProductBase, IIndexableContent, IProductListViewModelInitializer, IResourceable
    {

         [Display(Name = "Show in product list",
           Order = 10,
           GroupName = SystemTabNames.PageHeader)]
         [DefaultValue(true)]
         public virtual bool ShowInList { get; set; }

         // Same for all languages
         [Display(Name = "Facet Brand",
             Order = 12)]
         public virtual string Facet_Brand { get; set; }

         // Multi lang
         [Display(Name = "Color", Order = 15)]
         [CultureSpecific]
         public virtual string Color { get; set; }

        // Multi lang
        [Display(Name = "Overview", Order = 20)]
        [CultureSpecific]
        public virtual XhtmlString Overview { get; set; }

        // Multi lang
        [Display(Name = "Details", Order = 30)]
        [CultureSpecific]
        public virtual XhtmlString Details { get; set; }

        [Display(
         GroupName = SystemTabNames.Content,
         Order = 300,
         Name = "Average Rating")]
        [Editable(false)]
        public virtual double AverageRating { get; set; }


        public FindProduct GetFindProduct(IMarket market)
        {
            List<VariationContent> productVariants = GetVariants(this);
            var variations = GetGenericVariants(productVariants, market);

            var language = (Language == null ? string.Empty : Language.Name);
         
            var findProduct = new FindProduct(this, language);

            findProduct.Description = Description;
            findProduct.Color = Color != null ? new List<string>() {Color} : new List<string>();
            findProduct.Sizes =
                variations.Select(x => x.Size ?? string.Empty).Distinct().ToList();
            findProduct.Brand = this.Facet_Brand;

            findProduct.ShowInList = ShowInList && variations.Any(x => x.Stock > 0);
            EPiServer.Commerce.SpecializedProperties.Price defaultPrice = productVariants.GetDefaultPrice(market);

            findProduct.DefaultPrice = productVariants.GetDisplayPrice(market);
            findProduct.DefaultPriceAmount = productVariants.GetDefaultPriceAmount(market);
            findProduct.DiscountedPrice = productVariants.GetDiscountDisplayPrice(defaultPrice, market);

            // TODO: Set if not the same as default price
            findProduct.DiscountedPriceAmount = 0;

            findProduct.CustomerClubPrice = productVariants.GetCustomerClubDisplayPrice(market);
            findProduct.GenericVariants = variations;

            return findProduct;
        }

        public List<VariationContent> GetVariants(ProductContent product)
        {
            var linksRepository = ServiceLocator.Current.GetInstance<ILinksRepository>();
            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            CultureInfo cultureInfo = product.Language;

            IEnumerable<Relation> relationsBySource = linksRepository.GetRelationsBySource(product.ContentLink).OfType<ProductVariation>();
            List<VariationContent> productVariants = relationsBySource.Select(x => contentLoader.Get<VariationContent>(x.Target, new LanguageSelector(cultureInfo.Name))).ToList();
            return productVariants;
        }

        private List<GenericFindVariant> GetGenericVariants(List<VariationContent> productVariants, IMarket market)
        {
            List<GenericFindVariant> variations = new List<GenericFindVariant>();
            foreach (var variation in productVariants)
            {
                if (variation is GenericSizeVariationContent)
                {

                    GenericSizeVariationContent genericSizeVariationContent = variation as GenericSizeVariationContent;
                    var genericVariation = new GenericFindVariant()
                    {
                        Id = genericSizeVariationContent.ContentLink.ID,
                        Color = new List<string>{genericSizeVariationContent.Color},
                        Size = genericSizeVariationContent.Size ?? string.Empty,
                        Prices = genericSizeVariationContent.GetPricesWithMarket(market),
                        Code = genericSizeVariationContent.Code,
                        Stock = genericSizeVariationContent.GetStock()
                    };
                    variations.Add(genericVariation);
                }

            }
            return variations;
        }

         public bool ShouldIndex()
         {
             return !(StopPublish != null && StopPublish < DateTime.Now);
         }

         public ProductListViewModel Populate(IMarket market)
         {
             UrlResolver urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();

             var variation = this.GetFirstVariation();

             ProductListViewModel productListViewModel = new ProductListViewModel
             {
                 Code = this.Code,
                 ContentLink = this.ContentLink,
                 DisplayName = this.DisplayName,
                 Description = Description,
                 ProductUrl = urlResolver.GetUrl(ContentLink),
                 ImageUrl = this.GetDefaultImage(),
                 PriceString = variation.GetDisplayPrice(market),
                 ContentType = this.GetType().Name
             };

             productListViewModel.PriceAmount = variation.GetDefaultPriceAmount(market);
             return productListViewModel;
         }

         [ScaffoldColumn(false)]
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