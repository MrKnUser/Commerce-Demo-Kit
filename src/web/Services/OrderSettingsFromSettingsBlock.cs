using EPiServer.ServiceLocation;
using OxxCommerceStarterKit.Core.Services;
using OxxCommerceStarterKit.Web.Business;
using OxxCommerceStarterKit.Web.Models.PageTypes;

namespace OxxCommerceStarterKit.Web.Services
{
    [ServiceConfiguration(typeof(IOrderSettings))]
    public class OrderSettingsFromSettingsBlock : IOrderSettings
    {
        private readonly ISiteSettingsProvider _settings;

        public OrderSettingsFromSettingsBlock(ISiteSettingsProvider settings)
        {
            _settings = settings;
        }

        public bool ReleaseShipmentAutomatically
        {
            get
            {
                var settings = _settings.GetSettings();
                if(settings != null)
                {
                    return settings.ReleaseShipmentAutomatically;
                }

                return false;
            }
        }
    }
}