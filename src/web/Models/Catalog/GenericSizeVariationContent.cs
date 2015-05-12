using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.DataAnnotations;

namespace OxxCommerceStarterKit.Web.Models.Catalog
{
      [CatalogContentType(DisplayName = "Generic size variation",
        GUID = "6C00EADF-9246-42FF-8833-CB5FEA79B1C7",
        MetaClassName = "GenericSizeVariationContent")]
    public class GenericSizeVariationContent : VariationContent
    {
          [Display(Name = "Size")]
          public virtual string Size { get; set; }
          
          [Display(Name = "Color")]
          [CultureSpecific]
          public virtual string Color { get; set; }
    }
}