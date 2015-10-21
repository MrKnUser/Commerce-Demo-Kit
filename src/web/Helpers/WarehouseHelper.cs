using Mediachase.Commerce.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OxxCommerceStarterKit.Web.Helpers
{
    public class WarehouseHelper
    {
        /// <summary>
        /// Gets the warehouse by warehouse code.
        /// </summary>
        /// <param name="warehouseCode">The warehouse code.</param>
        /// <returns></returns>
        public static IWarehouse GetWarehouse(string warehouseCode)
        {
            if (string.IsNullOrEmpty(warehouseCode))
            {
                return null;
            }

            return EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<IWarehouseRepository>().Get(warehouseCode);
        }
    }
}
