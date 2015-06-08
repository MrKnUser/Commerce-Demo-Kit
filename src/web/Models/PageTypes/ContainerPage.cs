using EPiServer;
using EPiServer.DataAnnotations;
using OxxCommerceStarterKit.Core.Attributes;
using OxxCommerceStarterKit.Web.Business.Rendering;

namespace OxxCommerceStarterKit.Web.Models.PageTypes
{
    /// <summary>
    /// Used to logically group pages in the page tree
    /// </summary>
    [ContentType(
        GUID = "1426A16F-EDE4-45D2-90B3-47608B11EA18",
        GroupName = WebGlobal.GroupNames.Specialized,
        DisplayName = "Container Page",
        Description = "A page used to hold other pages. The Container page cannot be shown directly")]
    [SiteImageUrl]
    public class ContainerPage : SitePage, IContainerPage
    {
        
    }
}
