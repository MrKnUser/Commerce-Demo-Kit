using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using OxxCommerceStarterKit.Core.Attributes;

namespace OxxCommerceStarterKit.Web.Models.Blocks
{
    [ContentType(DisplayName = "Closest Store Block", GUID = "6c862133-0016-43c2-8f04-142ec43e1723", Description = "Displays the closest geographic store to the customer", 
        GroupName = WebGlobal.GroupNames.Location
        )]
    [SiteImageUrl(thumbnail: EditorThumbnail.Content)]
    public class ClosestStoreBlock : SiteBlockData
    {
        [CultureSpecific]
        [Required(AllowEmptyStrings = false)]
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual string Heading { get; set; }
    }
}