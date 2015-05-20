using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace OxxCommerceStarterKit.Web.Models.Blocks
{
    [ContentType(DisplayName = "Fature product",
        GUID = "BA2F1BC6-6078-4FDC-AC88-046D156E2E06",
        AvailableInEditMode = false)]
    public class FeatureProductBlock : SiteBlockData
    {
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 10)]
        [CultureSpecific]
        public virtual ContentReference MenuFeatureLink { get; set; }

        [Display(
          GroupName = SystemTabNames.Content,
          Order = 30)]
        [CultureSpecific]
        public virtual XhtmlString MenuFeatureText { get; set; }

        [Display(
         GroupName = SystemTabNames.Content,
         Order = 40)]
        [CultureSpecific]
        public virtual string MenuFeatureActionText { get; set; }
    }
}