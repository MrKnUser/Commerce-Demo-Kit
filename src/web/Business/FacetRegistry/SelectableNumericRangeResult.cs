using EPiServer.Find.Api.Facets;

namespace OxxCommerceStarterKit.Web.Business.FacetRegistry
{
    public class SelectableNumericRangeResult : NumericRangeResult, ISelectable
    {
        public bool Selected { get; set; }
    }
}