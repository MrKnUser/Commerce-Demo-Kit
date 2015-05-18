using System.Collections.Generic;
using System.Linq;
using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using OxxCommerceStarterKit.Web.Api;
using OxxCommerceStarterKit.Web.Extensions;

namespace OxxCommerceStarterKit.Web.Business.FacetRegistry
{
    public class FacetStringDefinition : FacetDefinition
    {
        public FacetStringDefinition()
        {
            TermsList = new List<MultiSelectTermCount>();
            SelectedTerms = new List<string>();
            RenderType = this.GetType().Name;
        }

        public List<MultiSelectTermCount> TermsList { get; set; }
        public List<string> SelectedTerms { get; set; }

        public override ITypeSearch<T> Filter<T>(ITypeSearch<T> query)
        {
            if (SelectedTerms.Any())
            {
                return query.AddStringFilter(SelectedTerms, FieldName);
            }
            return query;
        }

        public override ITypeSearch<T> Facet<T>(ITypeSearch<T> query)
        {
            return query.TermsFacetFor(FieldName, 50);
        }

        public override void PopulateFacet(Facet facet)
        {
            TermsList.Clear();

            TermsFacet termsFacet = facet as TermsFacet;
            if (termsFacet != null)
            {
                foreach (TermCount termCount in termsFacet.Terms)
                {
                    TermsList.Add(new MultiSelectTermCount()
                    {
                        Count = termCount.Count,
                        Term = termCount.Term,
                        Selected = SelectedTerms.Contains(termCount.Term)
                    });
                }

            }
        }
    }
}