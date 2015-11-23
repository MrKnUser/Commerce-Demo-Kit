using EPiServer.Core;

namespace OxxCommerceStarterKit.Web.Models.ViewModels
{
    public class ClosestStoreViewModel
    {
        public string Heading { get; set; }

        public string Description { get; set; }

        public string Coordinates { get; set; }

        public ContentReference Link { get; set; }

        public string Name { get; set; }
    }
}
