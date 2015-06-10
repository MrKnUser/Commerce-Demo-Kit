using EPiServer.Core;

namespace OxxCommerceStarterKit.Web.Models.ViewModels.Contracts
{
    public interface IListViewContentItem
    {
        string Title { get; set; }
        string Intro { get; set; }
        string ImageUrl { get; set; }
        ContentReference ContentLink { get; set; }
    }
}