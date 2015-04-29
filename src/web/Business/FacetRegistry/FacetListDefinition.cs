using System.Collections.Generic;
using System.Linq;
using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using OxxCommerceStarterKit.Web.Api;

namespace OxxCommerceStarterKit.Web.Business.FacetRegistry
{
    public class FacetStringListDefinition : FacetDefinition
    {
        public FacetStringListDefinition()
        {
            TermsList = new List<MultiSelectTermCount>();
        }

        public List<MultiSelectTermCount> TermsList { get; set; }

        public override ITypeSearch<T> Filter<T>(ITypeSearch<T> query)
        {
            var selectedFacetValues = TermsList.Where(x => x.Selected.Equals(true)).Select(x => x.Term).ToList();
            return query.AddStringListFilter(selectedFacetValues, FieldName);
        }

        public override ITypeSearch<T> Facet<T>(ITypeSearch<T> query)
        {
            return query.TermsFacetFor(FieldName, 50);
        }

        public override void PopulateFacet(Facet facet)
        {
            TermsFacet termsFacet = facet as TermsFacet;
            if (termsFacet != null)
            {
                foreach (TermCount termCount in termsFacet.Terms)
                {
                    TermsList.Add(new MultiSelectTermCount()
                    {
                        Count = termCount.Count,
                        Term = termCount.Term,
                        Selected = false
                    });
                }
            }
        }
    }

    public class MultiSelectTermCount : TermCount
    {
        public bool Selected { get; set; }
    }
}