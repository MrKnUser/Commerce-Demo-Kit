using System.Collections.Generic;
using EPiServer.Find;
using EPiServer.Find.Api.Facets;

namespace OxxCommerceStarterKit.Web.Business.FacetRegistry
{
    public abstract class FacetDefinition
    {
        public string Name { get; set; }
        public string FieldName { get; set; }
        public string RenderType { get; set; }

        public abstract ITypeSearch<T> Filter<T>(ITypeSearch<T> query);
        public abstract ITypeSearch<T> Facet<T>(ITypeSearch<T> query);
        public abstract void PopulateFacet(Facet facet);
    }
}