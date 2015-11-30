using System;
using System.Linq;
using EPiServer.Find;
using EPiServer.PlugIn;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Inventory;
using EPiServer.Scheduler;
using OxxCommerceStarterKit.Web.Models.FindModels;
using EPiServer.Find.Framework;
using OxxCommerceStarterKit.Web.Models.PageTypes;

namespace OxxCommerceStarterKit.Web.Jobs
{
    [ScheduledPlugIn(Description = "Warehouse Indexer. This job will index and geo-code all warehouse locations within the system.", DisplayName = "Warehouse Indexing")]
    public class WarehouseIndexing : ScheduledJobBase
    {

        public override string Execute()
        {

            var warehouseRepository = ServiceLocator.Current.GetInstance<IWarehouseRepository>();
            var client = SearchClient.Instance;
            string resultText = string.Empty;

            var storeResults = client.Search<StoreLocationPage>()
                                             .Select(x => new
                                             {
                                                 x.ContentLink,
                                                 x.Coordinates,
                                                 x.WarehouseCode
                                             })
                                             .GetResult();

            if (storeResults.TotalMatching > 0)
            {
                //Project to lightweight WarehouseIndexItem type
                var warehouseIndexItemList = warehouseRepository.List();
                short counter = 0;

                foreach (var warehouse in warehouseIndexItemList)
                {
                    var item = new WarehouseIndexItem
                    {
                        ContactInformation = warehouse.ContactInformation,
                        ApplicationId = warehouse.ApplicationId,
                        Code = warehouse.Code,
                        FulfillmentPriorityOrder = warehouse.FulfillmentPriorityOrder,
                        IsActive = warehouse.IsActive,
                        IsDeliveryLocation = warehouse.IsDeliveryLocation,
                        IsFulfillmentCenter = warehouse.IsFulfillmentCenter,
                        IsPickupLocation = warehouse.IsPickupLocation,
                        IsPrimary = warehouse.IsPrimary,
                        Name = warehouse.Name,
                        SortOrder = warehouse.SortOrder,
                        WarehouseId = warehouse.WarehouseId,
                        //TimeToLive = new TimeSpan(0, 30, 0)
                    };

                    var findStore = storeResults.Where(x => x.WarehouseCode == warehouse.Code).FirstOrDefault();
                    if (findStore != null)
                    {
                        item.Location = findStore.Coordinates;
                        item.StorePage = findStore.ContentLink;
                    }

                    client.Index(item);
                    counter++;
                }


                resultText = string.Format("Indexed {0} warehouses", counter);
            }
            else
            {
                resultText = "Could not locate any warehouses";
            }

            return resultText;

        }
    }
}