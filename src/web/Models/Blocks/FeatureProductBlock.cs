using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Commerce;

namespace OxxCommerceStarterKit.Web.Models.Blocks
{
    [ContentType(DisplayName = "Feature product",
        GUID = "BA2F1BC6-6078-4FDC-AC88-046D156E2E06",
        AvailableInEditMode = false)]
    public class FeatureProductBlock : SiteBlockData
    {
        [Display(
            Name ="Menu feature link (catalog product)",
            GroupName = WebGlobal.GroupNames.MenuFeature,
            Order = 10)]
        [CultureSpecific]
        [UIHint(UIHint.CatalogEntry)]
        public virtual ContentReference MenuFeatureLink { get; set; }

        [Display(
            Name ="Menu feature text",
            GroupName = WebGlobal.GroupNames.MenuFeature,
            Order = 30)]
        [CultureSpecific]
        public virtual XhtmlString MenuFeatureText { get; set; }

        [Display(
            Name ="Menu feature button text",
            GroupName = WebGlobal.GroupNames.MenuFeature,
            Order = 40)]
        [CultureSpecific]
        public virtual string MenuFeatureActionText { get; set; }
    }
}