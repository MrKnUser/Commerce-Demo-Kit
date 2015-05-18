using EPiServer.Core;
using EPiServer.Find;
using OxxCommerceStarterKit.Web.Models.FindModels;

namespace OxxCommerceStarterKit.Web.Models.Blocks.ProductFilters
{
    public abstract class FilterBaseBlock : BlockData
    {
        public abstract ITypeSearch<FindProduct> ApplyFilter(ITypeSearch<FindProduct> query);
    }
}