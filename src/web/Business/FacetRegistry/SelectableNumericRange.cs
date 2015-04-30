using EPiServer.Find.Api.Facets;

namespace OxxCommerceStarterKit.Web.Business.FacetRegistry
{
    public class SelectableNumericRange : NumericRange, ISelectable
    {
        public bool Selected { get; set; }
    }
}