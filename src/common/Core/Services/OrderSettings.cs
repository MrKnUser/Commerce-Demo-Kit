using OxxCommerceStarterKit.Core.Customers;

namespace OxxCommerceStarterKit.Core.Services
{
    public class OrderSettings : IOrderSettings
    {
        public OrderSettings()
        {
        }

        public bool ReleaseShipmentAutomatically
        {
            get { return false; }
        }
    }
}