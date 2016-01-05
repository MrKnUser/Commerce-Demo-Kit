using System.Collections.Generic;
using OxxCommerceStarterKit.Web.Models.ViewModels;

namespace OxxCommerceStarterKit.Web.Business.BulkOrdering.Interfaces
{
    public interface IProductLookup
    {
        void LookupProducts(IList<BulkUploadFileItem> products);
    }
}