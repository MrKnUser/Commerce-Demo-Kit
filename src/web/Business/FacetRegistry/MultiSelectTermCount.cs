using EPiServer.Find.Api.Facets;

namespace OxxCommerceStarterKit.Web.Business.FacetRegistry
{
    public class MultiSelectTermCount : TermCount, ISelectable
    {
        public bool Selected { get; set; }
    }
}