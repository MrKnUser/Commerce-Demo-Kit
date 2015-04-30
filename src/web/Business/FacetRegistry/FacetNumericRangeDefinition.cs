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
            RangeResult = new List<SelectableNumericRangeResult>();
            RenderType = this.GetType().Name;
        }

        public Type BackingType = typeof(double);
        public List<SelectableNumericRange> Range;
        public List<SelectableNumericRangeResult> RangeResult;

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
            RangeResult.Clear();

            NumericRangeFacet numericRangeFacet = facet as NumericRangeFacet;
            if(numericRangeFacet != null)
            {
                // Iterate the defintion, and see if any have been selected
                foreach (NumericRangeResult result in numericRangeFacet.Ranges)
                {
                    SelectableNumericRangeResult rangeResult = new SelectableNumericRangeResult()
                    {
                        Count = result.Count,
                        From = result.From,
                        To = result.To,
                        Total = result.Total,
                        TotalCount = result.TotalCount,
                        Min = result.Min,
                        Max = result.Max,
                        Mean = result.Mean
                    };

                    SelectableNumericRange selectedRange = Range.FirstOrDefault(r => r.Id == rangeResult.Id);
                    rangeResult.Selected = selectedRange != null;
                    RangeResult.Add(rangeResult);
                }
            }
        }
    }
}