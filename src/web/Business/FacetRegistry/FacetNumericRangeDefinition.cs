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
            List<SelectableNumericRange> numericRanges = Range.Where(r => r.Selected == true).ToList();
            if (numericRanges.Any())
            {
                query = query.AddFilterForNumericRange(numericRanges, FieldName, BackingType);
            }
            
            return query;
        }

        public override ITypeSearch<T> Facet<T>(ITypeSearch<T> query)
        {
            // Json deserializer adds empty ranges
            var range = Range.Where(x => x != null).ToList();
            if(range.Any())
            {
                // Convert to a range that Find can understand
                List<NumericRange> convertedRangeList = new List<NumericRange>();
                foreach (SelectableNumericRange selectableNumericRange in range)
                {
                    convertedRangeList.Add(selectableNumericRange.ToNumericRange());
                }
                return query.NumericRangeFacetFor(FieldName, convertedRangeList, BackingType);
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

                    SelectableNumericRange selectedRange = Range.FirstOrDefault(r => r.Id == rangeResult.Id && r.Selected == true);
                    rangeResult.Selected = selectedRange != null;
                    RangeResult.Add(rangeResult);
                }
            }
        }
    }
}