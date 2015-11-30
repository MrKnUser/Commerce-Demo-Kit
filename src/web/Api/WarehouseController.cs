using System;
using System.Collections.Generic;
using EPiServer.Find;
using EPiServer.Find.Framework;
using OxxCommerceStarterKit.Web.Models.FindModels;

namespace OxxCommerceStarterKit.Web.Api
{
    public class WarehouseController : BaseApiController
    {
        //// GET api/warehouse/52/0.25/10
        //public IEnumerable<WarehouseIndexItem> GetByProduct(double lat, double lon, string productCode, int distance = 10)
        //{
        //    var client = SearchClient.Instance;

        //    //Create co-ordinates container
        //    var selectedLocation = new GeoLocation(lat, lon);

        //    //Get warehouses in desired distance, filter by relevant warehouse codes.
        //    var result = client.Search<WarehouseIndexItem>().Take(30)
        //        .Filter(x =>
        //                x.Location.WithinDistanceFrom(
        //                    selectedLocation,
        //                    distance.Kilometers()))
        //        .Filter(x => x.Code.In(
        //            client.Search<WarehouseInventoryIndexItem>()
        //                .Filter(w =>
        //                        w.CatalogEntryCode.MatchCaseInsensitive(productCode))
        //                .Select(w => w.WarehouseCode)
        //                .GetResult()
        //                         ))
        //        .OrderBy(x => x.Location).DistanceFrom(selectedLocation)
        //        .StaticallyCacheFor(TimeSpan.FromMinutes(1))

        //        .GetResult();

        //    return result;
        //}


        // GET api/warehouse/52/0.25/10
        public IEnumerable<WarehouseIndexItem> Get(double lat, double lon, int distance = 10)
        {
            var client = SearchClient.Instance;

            //Get warehouses in desired distance, filter by relevant warehouse codes.
            var selectedLocation = new GeoLocation(lat, lon);

            var result = client.Search<WarehouseIndexItem>().Take(30)

                .Filter(x =>
                        x.Location.WithinDistanceFrom(
                            selectedLocation,
                            distance.Kilometers()))

                .OrderBy(x => x.Location).DistanceFrom(selectedLocation)

                .StaticallyCacheFor(TimeSpan.FromMinutes(1))

                .GetResult();

            return result;
        }

    }

}