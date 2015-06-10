using EPiServer.Core;
using OxxCommerceStarterKit.Web.Models.ViewModels.Contracts;

namespace OxxCommerceStarterKit.Web.Models.ViewModels
{
    public class ListViewContentItem : IListViewContentItem
    {
        public string Title { get; set; }

        public string Intro { get; set; }

        public string ImageUrl { get; set; }

        public ContentReference ContentLink { get; set; }
    }
}