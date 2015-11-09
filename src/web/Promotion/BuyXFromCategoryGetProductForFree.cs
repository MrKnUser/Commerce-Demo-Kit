using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Marketing;
using EPiServer.Core;
using EPiServer.DataAnnotations;

namespace OxxCommerceStarterKit.Web.Promotion
{
    [ContentType(DisplayName = "Buy a number of products from a category and get a product for free",
        GUID="FACC7148-9AE2-48A3-A86A-3E9E1C1505C5")]
    public class BuyXFromCategoryGetProductForFree : OrderPromotion
    {
        [UIHint("allcontent"), AllowedTypes(new Type[] { typeof(NodeContent) })]
        public virtual ContentReference Category { get; set; }

        [UIHint("allcontent"), AllowedTypes(new Type[] { typeof(VariationContent) })]
        public virtual ContentReference FreeProduct { get; set; }

        public virtual int Threshold { get; set; }

       
    }    
}
