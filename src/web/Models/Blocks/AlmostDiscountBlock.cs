using EPiServer.DataAnnotations;
using OxxCommerceStarterKit.Core.Attributes;

namespace OxxCommerceStarterKit.Web.Models.Blocks
{
    [ContentType(GUID = "9ab7384f-8d6b-49e6-b3f8-42c32b7e46d7",
        DisplayName = "Almost at Discount",
        Description = "Almost at Discount notifies shopper when they have almost fulfilled a discount",
        GroupName = WebGlobal.GroupNames.Marketing)]
    [SiteImageUrl(EditorThumbnail.Content)]
    public class AlmostDiscountBlock : SiteBlockData
    {
    }
}