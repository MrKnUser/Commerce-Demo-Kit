using OxxCommerceStarterKit.Web.Models.Blocks;

namespace OxxCommerceStarterKit.Web.Models.PageTypes
{
    public interface IHasFeatureProduct
    {
        FeatureProductBlock FeatureProduct { get; set; }
    }
}