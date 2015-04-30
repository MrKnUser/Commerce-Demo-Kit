using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Api.Querying.Filters;
using EPiServer.Find.Framework;
using OxxCommerceStarterKit.Web.Business.FacetRegistry;

namespace OxxCommerceStarterKit.Web.Api
{
    public static class SearchExtensions
    {
        public static Expression<Func<T, object>> GetTermFacetForResult<T>(string fieldName)
        {
            ParameterExpression paramX = Expression.Parameter(typeof(T), "x");
            MemberExpression property = Expression.Property(paramX, fieldName);
            Expression conversion = Expression.Convert(property, typeof(object));
            Expression<Func<T, object>> expr = Expression.Lambda<Func<T, object>>(conversion, paramX);

            return expr;
        }

        public static ITypeSearch<TSource> NumericRangeFacetFor<TSource>(this ITypeSearch<TSource> search, string name, IEnumerable<NumericRange> range, Type backingType)
        {
            return search.RangeFacetFor(GetTermFacetForResult<TSource>(name),
                NumericRangfeFacetRequestAction(search.Client, name, range, backingType));

        }

        public static ITypeSearch<TSource> NumericRangeFacetFor<TSource>(this ITypeSearch<TSource> search, string name, IEnumerable<NumericRange> range)
        {
            return search.RangeFacetFor(GetTermFacetForResult<TSource>(name),
                NumericRangfeFacetRequestAction(search.Client, name, range, typeof(double)));

        }


        public static ITypeSearch<TSource> NumericRangeFacetFor<TSource>(this ITypeSearch<TSource> search, string name, double from, double to)
        {
            return search.RangeFacetFor(GetTermFacetForResult<TSource>(name),
                NumericRangfeFacetRequestAction(search.Client, name, from, to, typeof(double)));
        }


        public static ITypeSearch<TSource> TermsFacetFor<TSource>(this ITypeSearch<TSource> search, string name, int size)
        {
            return search.TermsFacetFor(name, FacetRequestAction(search.Client, name, size));

        }

        private static Action<NumericRangeFacetRequest> NumericRangfeFacetRequestAction(IClient searchClient, string fieldName, IEnumerable<NumericRange> range, Type type)
        {
            string fullFieldName = GetFullFieldName(searchClient, fieldName, type);

            return (x =>
            {
                x.Field = fullFieldName;
                x.Ranges.AddRange(range);
            });
        }

        private static Action<NumericRangeFacetRequest> NumericRangfeFacetRequestAction(IClient searchClient, string fieldName, double from, double to, Type type)
        {
            List<NumericRange> range = new List<NumericRange>();
            range.Add(new NumericRange(from, to));
            return NumericRangfeFacetRequestAction(searchClient, fieldName, range, type);
        }

        private static Action<TermsFacetRequest> FacetRequestAction(IClient searchClient, string fieldName, int size)
        {

            string fullFieldName = GetFullFieldName(searchClient, fieldName);

            return (x =>
            {
                x.Field = fullFieldName;
                x.Size = size;
            });
        }

        public static string GetFullFieldName(this IClient searchClient, string fieldName)
        {
            return GetFullFieldName(searchClient, fieldName, typeof(string));
        }

        public static string GetFullFieldName(this IClient searchClient, string fieldName, Type type)
        {
            return fieldName + searchClient.Conventions.FieldNameConvention.GetFieldName(Expression.Variable(type, fieldName));
        }

        public static ITypeSearch<T> AddStringListFilter<T>(this ITypeSearch<T> query, List<string> stringFieldValues, string fieldName)
        {
            if (stringFieldValues != null && stringFieldValues.Any())
            {
                return query.Filter(GetOrFilterForStringList<T>(stringFieldValues, query.Client, fieldName));
            }
            return query;
        }

        private static FilterBuilder<T> GetOrFilterForStringList<T>(List<string> fieldValues, IClient client, string fieldName)
        {
            // Appends type convention to field name (like "$$string")
            string fullFieldName = client.GetFullFieldName(fieldName);

            List<Filter> filters = new List<Filter>();
            foreach (string s in fieldValues)
            {
                filters.Add(new TermFilter(fullFieldName, s));
            }

            OrFilter orFilter = new OrFilter(filters);
            FilterBuilder<T> filterBuilder = new FilterBuilder<T>(client, orFilter);
            return filterBuilder;
        }

        public static ITypeSearch<T> AddFilterForNumericRange<T>(this ITypeSearch<T> query, IEnumerable<SelectableNumericRange> range, string fieldName)
        {
            return AddFilterForNumericRange(query, range, fieldName, typeof(double));
        }

        public static ITypeSearch<T> AddFilterForNumericRange<T>(this ITypeSearch<T> query, IEnumerable<SelectableNumericRange> range, string fieldName, Type type)
        {
            return query.Filter(GetOrFilterForNumericRange<T>(query, range, fieldName, type));
        }

        private static FilterBuilder<T> GetOrFilterForNumericRange<T>(ITypeSearch<T> query, IEnumerable<SelectableNumericRange> range, string fieldName, Type type)
        {
            // Appends type convention to field name (like "$$string")
            IClient client = query.Client;
            string fullFieldName = client.GetFullFieldName(fieldName, type);

            List<Filter> filters = new List<Filter>();
            foreach (SelectableNumericRange rangeItem in range)
            {
                filters.Add(RangeFilter.Create(fullFieldName, rangeItem.From ?? 0, rangeItem.To ?? double.MaxValue));
            }
            

            OrFilter orFilter = new OrFilter(filters);
            FilterBuilder<T> filterBuilder = new FilterBuilder<T>(client, orFilter);
            return filterBuilder;
        }

    }
}