using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using OxxCommerceStarterKit.Web.Api;

namespace OxxCommerceStarterKit.Web.Business.FacetRegistry
{
    public class FacetNumericRangeDefinition : FacetDefinition
    {
        public FacetNumericRangeDefinition()
        {
            Range = new List<SelectableNumericRange>();
            RangeResult = new List<NumericRangeResult>();
        }

        public Type BackingType = typeof(double);
        public List<SelectableNumericRange> Range;
        public List<NumericRangeResult> RangeResult;

        public override ITypeSearch<T> Filter<T>(ITypeSearch<T> query)
        {
            return query;
        }

        public override ITypeSearch<T> Facet<T>(ITypeSearch<T> query)
        {
            // Json deserializer adds empty ranges
            var range = Range.Where(x => x != null).ToList();
            if(range.Any())
            {
                return query.NumericRangeFacetFor(FieldName, range, BackingType);
            }
            return query;
        }

        public override void PopulateFacet(Facet facet)
        {
            NumericRangeFacet numericRangeFacet = facet as NumericRangeFacet;
            if(numericRangeFacet != null)
            {
                RangeResult = numericRangeFacet.Ranges.ToList();
            }
        }
    }

    public class SelectableNumericRange : NumericRange, ISelectable
    {
        public bool Selected { get; set; }
    }
}