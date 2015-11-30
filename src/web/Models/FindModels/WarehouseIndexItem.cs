using System;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Api;
using Mediachase.Commerce.Inventory;
using Newtonsoft.Json;

namespace OxxCommerceStarterKit.Web.Models.FindModels
{
    public class WarehouseIndexItem 
    {
        [Id]
        public int? WarehouseId { get; set; }
        public Guid ApplicationId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsPrimary { get; set; }
        public int SortOrder { get; set; }
        public string Code { get; set; }
        public IWarehouseContactInformation ContactInformation { get; set; }
        public GeoLocation Location { get; set; }
        public int FulfillmentPriorityOrder { get; set; }
        public bool IsFulfillmentCenter { get; set; }
        public bool IsPickupLocation { get; set; }
        public bool IsDeliveryLocation { get; set; }
        public ContentReference StorePage { get; internal set; }

        [TimeToLive]
        public TimeToLive TimeToLive { get; set; }

        [JsonIgnore]
        public string PrintAddress
        {
            get
            {
                return string.Format("{0}, {1}{2}, {3}{4} {5}",
                                                                    ContactInformation.Line1,
                                                                    string.IsNullOrWhiteSpace(ContactInformation.Line2) ? "" : ContactInformation.Line2 + ", ",
                                                                    ContactInformation.City,
                                                                    string.IsNullOrWhiteSpace(ContactInformation.State) ? "" : ContactInformation.State + ", ",
                                                                    ContactInformation.PostalCode,
                                                                    ContactInformation.CountryName);
            }
        }

    }
}