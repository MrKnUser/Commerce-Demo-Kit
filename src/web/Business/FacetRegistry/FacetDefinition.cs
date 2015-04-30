using System.Collections.Generic;
using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using EPiServer.Framework.Localization;

namespace OxxCommerceStarterKit.Web.Business.FacetRegistry
{
    public abstract class FacetDefinition
    {
        private string _displayName = null;
        public string Name { get; set; }
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(_displayName))
                    return LocalizationService.Current.GetString("/facetregistry/" + Name.ToLowerInvariant(), Name);
                return _displayName;
            }

            set { _displayName = value; }
        }

        public string FieldName { get; set; }
        public string RenderType { get; set; }

        public abstract ITypeSearch<T> Filter<T>(ITypeSearch<T> query);
        public abstract ITypeSearch<T> Facet<T>(ITypeSearch<T> query);
        public abstract void PopulateFacet(Facet facet);
    }
}