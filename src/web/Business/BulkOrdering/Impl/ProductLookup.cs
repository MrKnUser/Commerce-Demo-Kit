using System.Collections.Generic;
using Mediachase.Commerce.Catalog;
using OxxCommerceStarterKit.Web.Business.BulkOrdering.Interfaces;
using OxxCommerceStarterKit.Web.Models.ViewModels;

namespace OxxCommerceStarterKit.Web.Business.BulkOrdering.Impl
{
    public class ProductLookup : IProductLookup
    {
        public void LookupProducts(IList<BulkUploadFileItem> products)
        {
            foreach (var product in products)
            {
                try
                {
                    var entry = CatalogContext.Current.GetCatalogEntry(product.ProductCode);
                    if (entry == null) // Potential to add other checks here (inventory, status etc)
                    {
                        product.CouldLookupProduct = false;
                    }
                    else
                    {
                        product.Description = entry.Name;
                    }
                }
                catch
                {
                    product.CouldLookupProduct = false;
                }
            }
        }
    }
}