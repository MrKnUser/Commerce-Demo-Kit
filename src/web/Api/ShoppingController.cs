/*
Commerce Starter Kit for EPiServer

All rights reserved. See LICENSE.txt in project root.

Copyright (C) 2013-2014 Oxx AS
Copyright (C) 2013-2014 BV Network AS

*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Web.Http;
using System.Web.UI.WebControls;
using EPiServer.Enterprise.Transfer;
using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Api.Querying.Filters;
using EPiServer.Find.Framework;
using EPiServer.Find.Framework.Statistics;
using EPiServer.Find.Helpers;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OxxCommerceStarterKit.Web.Business.FacetRegistry;
using OxxCommerceStarterKit.Web.Models.FindModels;

namespace OxxCommerceStarterKit.Web.Api
{
    public class ShoppingController : BaseApiController
    {
        private ILogger _log = LogManager.GetLogger();
        public class FacetViewModel
        {
            public string Name { get; set; }
            public int Count { get; set; }
            public bool Selected { get; set; }
            public string Size { get; set; }
            public string Id { get; set; }
        }

        public class SizeFacetViewModel
        {
            public string SizeUnit { get; set; }
            public bool SomeIsSelected { get; set; }
            public List<FacetViewModel> SizeFacets { get; set; }
        }


        public class ProductSearchData
        {

            [DefaultValue(1)]
            public int Page { get; set; }
            [DefaultValue(10)]
            public int PageSize { get; set; }
            public ProductData ProductData { get; set; }

        }
        public class ProductData
        {
            public List<int> SelectedProductCategories { get; set; }
            public List<string> SelectedColorFacets { get; set; }
            public List<string> SelectedSizeFacets { get; set; }
            public List<string> SelectedFitsFacets { get; set; }
            public List<string> SelectedMainCategoryFacets { get; set; }
            public List<string> SelectedRegionFacets { get; set; }
            public List<string> SelectedGrapeFacets { get; set; }
            public List<string> SelectedCountryFacets { get; set; }
            public string SearchTerm { get; set; }
            public List<FacetValues> Facets { get; set; }

        }

        [HttpPost]
        public object GetProducts(ProductSearchData productSearchData)
        {
            try
            {

                string language = Language;
                //Starting the find query
                var query = SearchClient.Instance.Search<FindProduct>(GetLanguage(language));

                // search term
                query = ApplyTermFilter(productSearchData, query, true);

                // common filters
                query = ApplyCommonFilters(productSearchData, query, language);


                // selected categories
                if (productSearchData.ProductData.SelectedProductCategories != null &&
                    productSearchData.ProductData.SelectedProductCategories.Any())
                {
                    query = query.Filter(x => GetCategoryFilter(productSearchData.ProductData.SelectedProductCategories));
                }
                if (productSearchData.ProductData.Facets != null && productSearchData.ProductData.Facets.Any())
                {
                    foreach (FacetValues fv in productSearchData.ProductData.Facets)
                    {
                        var selectedFacets = fv.Values.Where(x => x.Selected.Equals(true)).Select(x => x.Name).ToList();
                        if (selectedFacets.Any())
                            AddStringListFilter(selectedFacets, ref query, fv.Definition.FieldName);
                    }
                }

                // execute search
                query = query.Skip((productSearchData.Page - 1) * productSearchData.PageSize)
                    .Take(productSearchData.PageSize);
                var searchResult = query.StaticallyCacheFor(TimeSpan.FromMinutes(1)).GetResult();
                //Done with search query

                //Facets for product cagetories, get all, only filtered on search term and main category(menn/dame) if selected
                var productFacetQuery = SearchClient.Instance.Search<FindProduct>(GetLanguage(language));

                // search term
                productFacetQuery = ApplyTermFilter(productSearchData, productFacetQuery);

                // common filters
                productFacetQuery = ApplyCommonFilters(productSearchData, productFacetQuery, language);


                // categories
                if (productSearchData.ProductData.SelectedMainCategoryFacets != null &&
                    productSearchData.ProductData.SelectedMainCategoryFacets.Any())
                {
                    productFacetQuery =
                        productFacetQuery.Filter(
                            x => GetMainCategoryFilter(productSearchData.ProductData.SelectedMainCategoryFacets));
                }

                // execute 
                var productFacetsResult = productFacetQuery
                    .TermsFacetFor(x => x.ParentCategoryName)
                    .TermsFacetFor(x => x.MainCategoryName)
                    .Take(0)
                    .GetResult();

                // results
                var productCategoryFacetsResult = productFacetsResult.TermsFacetFor(x => x.ParentCategoryName).Terms;
                var productMainCategoryFacetResult = productFacetsResult.TermsFacetFor(x => x.MainCategoryName).Terms;
                var allProductCategoryFacets = CreateCategoryFacetViewModels(productCategoryFacetsResult,
                    productSearchData.ProductData.SelectedProductCategories.Select(x => x.ToString()).ToList());
                var allMainCategoryFacets = CreateFacetViewModels(productMainCategoryFacetResult,
                    productSearchData.ProductData.SelectedMainCategoryFacets);

                //Facets - To get all, color, size and fit facets, based on selected product categories
                var facetsQuery = SearchClient.Instance.Search<FindProduct>(GetLanguage(language));

                // search text in common fields
                facetsQuery = ApplyTermFilter(productSearchData, facetsQuery);

                // common filters (language, show in list)
                facetsQuery = ApplyCommonFilters(productSearchData, facetsQuery, language);

                facetsQuery = facetsQuery
                    .Filter(x => GetCategoryFilter(productSearchData.ProductData.SelectedProductCategories))
                    //.Filter(x => GetMainCategoryFilter(productSearchData.ProductData.SelectedMainCategoryFacets))
                    .TermsFacetFor(x => x.Color, r => r.Size = 50)
                    .TermsFacetFor(x => x.Fit, r => r.Size = 50)
                    .TermsFacetFor(x => x.SizesList, r => r.Size = 200)
                    .TermsFacetFor(x => x.GrapeMixList, r => r.Size = 50);

                // TODO: Add these from facet registry
                facetsQuery = facetsQuery
                    .TermsFacetFor("Region", 50)
                    .TermsFacetFor("Country", 50);

                // execute search
                var facetsResult = facetsQuery.
                    Take(0)
                    .StaticallyCacheFor(TimeSpan.FromMinutes(1))
                    .GetResult();

                // results
                // TODO: This list of facets should be replaced by facet registry as below
                //var productColorFacetsResult = facetsResult.TermsFacetFor(x => x.Color).Terms;
                //var productFitFacetsResult = facetsResult.TermsFacetFor(x => x.Fit).Terms;
                //var productsizesResult = facetsResult.TermsFacetFor(x => x.SizesList).Terms;

                //var productRegionResult = facetsResult.TermsFacetFor(GetTermFacetForResult("Region")).Terms;
                ////var productRegionResult = facetsResult.TermsFacetFor(x => x.Region).Terms;
                //var productGrapeResult = facetsResult.TermsFacetFor(x => x.GrapeMixList).Terms;
                //var productCountryResult = facetsResult.TermsFacetFor(x => x.Country).Terms;
                //var allColorFacets = CreateFacetViewModels(productColorFacetsResult,
                //    productSearchData.ProductData.SelectedColorFacets);
                //var allFitFacets = CreateFacetViewModels(productFitFacetsResult,
                //    productSearchData.ProductData.SelectedFitsFacets);
                //var allRegionFacets = CreateFacetViewModels(productRegionResult,
                //    productSearchData.ProductData.SelectedRegionFacets);
                //var allGrapeFacets = CreateFacetViewModels(productGrapeResult,
                //    productSearchData.ProductData.SelectedGrapeFacets);
                //var allcountryFacets = CreateFacetViewModels(productCountryResult,
                //    productSearchData.ProductData.SelectedCountryFacets);
                ////Testing different size type facets
                //var allDifferentSizeFacets = GetAllDifferentSizeFacets(productsizesResult,
                //    productSearchData.ProductData.SelectedSizeFacets);

                var totalMatching = searchResult.TotalMatching;

                // Get all facet values based on facet registry
                var facetRegistry = ServiceLocator.Current.GetInstance<FacetRegistry>();
                List<FacetValues> facetValues = new List<FacetValues>();
                foreach (FacetDefinition definition in facetRegistry.FacetDefinitions)
                {
                    var facet = facetsResult.Facets.FirstOrDefault(f => f.Name.Equals(definition.FieldName));
                    if (facet != null)
                    {
                        Test t = new Test();
                        t.FieldName = definition.FieldName;
                        t.Name = definition.Name;
                        var valuesForFacet = new FacetValues()
                        {
                            Definition = definition
                        };

                        TermsFacet termsFacet = facet as TermsFacet;
                        if (termsFacet != null)
                        {

                            foreach (TermCount termCount in termsFacet.Terms)
                            {
                                valuesForFacet.Values.Add(new FacetValue()
                                {
                                    Count = termCount.Count,
                                    Name = termCount.Term,
                                    // TODO Determine "Selected" based on input to
                                    // this method (currently in productSearchData.ProductData)
                                    Selected = GetIsSelected(definition.FieldName, termCount.Term, productSearchData.ProductData.Facets)
                                });
                            }
                        }

                        facetValues.Add(valuesForFacet);
                    }
                }

                var result = new
                {
                    products = searchResult.ToList(),
                    productCategoryFacets = allProductCategoryFacets,
                    //productColorFacets = allColorFacets,
                    //allSizeFacetLists = allDifferentSizeFacets,
                    //productFitFacets = allFitFacets,
                    //productRegionFacets = allRegionFacets,
                    //productGrapeFacets = allGrapeFacets,
                    //productCountryFacets = allcountryFacets,
                    mainCategoryFacets = allMainCategoryFacets,
                    facets = facetValues,
                    totalResult = totalMatching
                };

                var serializer = new JsonSerializer();
                serializer.TypeNameHandling = TypeNameHandling.Auto;
            
                return JObject.FromObject(result, serializer);

                return JsonConvert.SerializeObject(result, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }
            catch (OperationCanceledException operationCanceledException)
            {
                _log.Error("Failed to perform search", operationCanceledException);
                return null;
            }
            catch (ServiceException serviceException)
            {
                _log.Error("Failed to perform search", serviceException);
                return null;
            }
        }

        private bool GetIsSelected(string fieldName, string facet, List<FacetValues> facets)
        {
            if (facets == null)
                return false;
            foreach (var facetValue in facets)
            {
                if (facetValue.Definition.FieldName.Equals(fieldName))
                {
                    foreach (var value in facetValue.Values)
                    {
                        if (value.Name.Equals(facet))
                            return value.Selected;
                    }
                }
            }
            return false;
        }

        protected void AddStringListFilter(List<string> stringFieldValues, ref ITypeSearch<FindProduct> query, string fieldName)
        {
            if (stringFieldValues != null && stringFieldValues.Any())
            {
                // First attempt at moving away from expressions, making this
                // implementation extensible and pluggable
                query = query.Filter(GetOrFilterForStringList(stringFieldValues, SearchClient.Instance, fieldName));
            }
        }

        private static ITypeSearch<FindProduct> ApplyCommonFilters(ProductSearchData productSearchData, ITypeSearch<FindProduct> query, string language)
        {
            return query.Filter(x => x.Language.Match(language))
                .Filter(x => x.ShowInList.Match(true));
        }

        private static ITypeSearch<FindProduct> ApplyTermFilter(ProductSearchData productSearchData, ITypeSearch<FindProduct> query, bool trackSearchTerm = false)
        {
            if (!string.IsNullOrEmpty(productSearchData.ProductData.SearchTerm))
            {
                query = query.For(productSearchData.ProductData.SearchTerm)
                    .InFields(x => x.Name, x => x.MainCategoryName, x => string.Join(",", x.Color),
                        x => x.DisplayName, x => x.Fit, x => x.Description.ToString(), x => string.Join(",", x.ParentCategoryName))
                        .InAllField();

                if (trackSearchTerm)
                {
                    query = query.Track();
                }
            }
            return query;
        }

        private List<SizeFacetViewModel> GetAllDifferentSizeFacets(IEnumerable<TermCount> facetResult, List<string> selectedSizes)
        {
            List<SizeFacetViewModel> allDifferentSizeFacets = new List<SizeFacetViewModel>();

            foreach (var sizeFacet in facetResult.OrderBy(x => SortSizes(x.Term)))
            {
                string term = sizeFacet.Term.ToLower();
                string[] typeAndSize = term.Split('/');
                string sizeType = typeAndSize[0];
                string size = typeAndSize[1];
                if (typeAndSize.Length == 3)
                {
                    sizeType = typeAndSize[1];
                    size = typeAndSize[2];
                }

                bool newSizeTypeList = true;
                SizeFacetViewModel sizeList = allDifferentSizeFacets.FirstOrDefault(x => x.SizeUnit == sizeType);
                if (sizeList == null)
                {
                    sizeList = new SizeFacetViewModel();
                    sizeList.SizeUnit = sizeType;
                    sizeList.SizeFacets = new List<FacetViewModel>();
                }
                else
                {
                    newSizeTypeList = false;
                }
                // check for duplicate entry and join them
                var facetViewModel = sizeList.SizeFacets.FirstOrDefault(x => x.Name == term);
                if (facetViewModel == null)
                {
                    facetViewModel = new FacetViewModel();
                    facetViewModel.Name = term;
                    facetViewModel.Size = size;
                    facetViewModel.Count = sizeFacet.Count;
                    sizeList.SizeFacets.Add(facetViewModel);
                }
                else
                {
                    facetViewModel.Count += sizeFacet.Count;
                }
                facetViewModel.Selected = selectedSizes != null && selectedSizes.Contains(term);

                if (sizeList.SizeFacets.Any(x => x.Selected.Equals(true)))
                {
                    sizeList.SomeIsSelected = true;
                }
                if (newSizeTypeList)
                {
                    allDifferentSizeFacets.Add(sizeList);
                }
            }
            return allDifferentSizeFacets;
        }

        /// <summary>
        /// Add a number in front of letter sizes to specify ordering
        /// </summary>
        /// <param name="term">Examples: unisex/xxl , 85-105, 85, inch/82-82, cm ny/20</param>
        /// <returns></returns>
        public static int SortSizes(string term)
        {
            if (term == null)
            {
                return 10000000;
            }
            term = term.ToLower();
            string sizeType = "", sizeUnit = "", size = "";
            var terms = term.Split('/');
            if (terms.Length == 2)
            {
                sizeUnit = terms[0];
                size = terms[1];
            }
            else if (terms.Length == 3)
            {
                sizeType = terms[0];
                sizeUnit = terms[1];
                size = terms[2];
            }
            else
            {
                return 10000000;
            }

            if (sizeType.StartsWith("m") &&
                sizeUnit == "standard") // herre jakke
            {
                // group sizes by ranges
                int sizeNumber = ParseInt(size, 1000000);
                if (sizeNumber == 1000000)
                {
                    return sizeNumber;
                }
                if (sizeNumber >= 46 && sizeNumber <= 64)
                {
                    return sizeNumber + 1000;
                }
                else if (sizeNumber >= 23 && sizeNumber <= 31)
                {
                    return sizeNumber + 10000;
                }
                else if (sizeNumber >= 146 && sizeNumber <= 160)
                {
                    return sizeNumber + 100000;
                }
                return sizeNumber + 1000000;
            }

            if (sizeUnit == "unisex")
            {
                var sort = LetterSizeSortIndex.blokk;
                Enum.TryParse<LetterSizeSortIndex>(size, true, out sort);
                return ((int)sort);
            }


            // check for number
            if (size.Contains("-"))
            {
                return GetSortScoreForTwoSizes(size);
            }
            else
            {
                return ParseInt(size, 100000);
            }
        }
        public static int ParseInt(string input, int defaultValue = 0)
        {
            int val = defaultValue;
            if (Int32.TryParse(input, out val))
            {
                return val;
            }
            else
            {
                return defaultValue;
            }
        }
        private static int GetSortScoreForTwoSizes(string term)
        {
            string sizePart = "";
            int sizeNumber = 0;
            sizePart = term.Split('-')[0];
            sizeNumber = ParseInt(sizePart, 100000) * 100;
            sizePart = term.Split('-')[1];
            sizeNumber += ParseInt(sizePart);
            return sizeNumber;
        }
        private enum LetterSizeSortIndex
        {
            xxxs = 10,
            xxs = 11,
            xs = 12,
            s = 13,
            m = 14,
            l = 15,
            xl = 16,
            xxl = 17,
            xxxl = 18,
            xxxxl = 19,
            xxxxxl = 20,
            xxxxxxl = 21,
            blokk = 50
        }

        private List<FacetViewModel> CreateFacetViewModels(IEnumerable<TermCount> facetResult, List<string> selectedFacets)
        {
            return facetResult.Select(
                        term =>
                            new FacetViewModel
                            {
                                Count = term.Count,
                                Name = term.Term,
                                Selected = selectedFacets != null && selectedFacets.Contains(term.Term)
                            }).ToList();
        }

        private List<FacetViewModel> CreateCategoryFacetViewModels(IEnumerable<TermCount> facetResult, List<string> selectedFacets)
        {
            return facetResult.Select(
                        term =>
                        {
                            string id = term.Term.Contains("|") ? term.Term.Split('|')[0] : term.Term;
                            return new FacetViewModel
                                {
                                    Count = term.Count,
                                    Name = term.Term.Contains("|") ? term.Term.Split('|')[1] : term.Term,
                                    Id = id,
                                    Selected = selectedFacets != null && selectedFacets.Contains(id)
                                };
                        }).ToList();
        }


        private FilterBuilder<FindProduct> GetCategoryFilter(List<int> categories)
        {
            var filter = SearchClient.Instance.BuildFilter<FindProduct>();
            return categories.Aggregate(filter, (current, category) => current.Or(x => x.ParentCategoryId.Match(category)));
        }
        private FilterBuilder<FindProduct> GetMainCategoryFilter(List<string> mainCategories)
        {
            var filter = SearchClient.Instance.BuildFilter<FindProduct>();
            return mainCategories.Aggregate(filter, (current, mainCategory) => current.Or(x => x.MainCategoryName.Match(mainCategory)));
        }

        private FilterBuilder<FindProduct> GetColorFilter(List<string> colorsList)
        {
            var filter = SearchClient.Instance.BuildFilter<FindProduct>();
            return colorsList.Aggregate(filter, (current, color) => current.Or(x => x.Color.Match(color)));
        }
        private FilterBuilder<FindProduct> GetSizeFilter(List<string> sizeList)
        {
            var filter = SearchClient.Instance.BuildFilter<FindProduct>();
            return sizeList.Aggregate(filter, (current, size) => current.Or(x => x.SizesList.MatchCaseInsensitive(size)));
        }
        private FilterBuilder<FindProduct> GetFitFilter(List<string> fitList)
        {
            var filter = SearchClient.Instance.BuildFilter<FindProduct>();
            return fitList.Aggregate(filter, (current, fit) => current.Or(x => x.Fit.Match(fit)));
        }
        //private FilterBuilder<FindProduct> GetRegionFilter(List<string> regionList)
        //{
        //    var filter = SearchClient.Instance.BuildFilter<FindProduct>();
        //    return regionList.Aggregate(filter, (current, region) => current.Or(x => x.Region.Match(region)));
        //}
        private FilterBuilder<FindProduct> GetGrapeFilter(List<string> grapeList)
        {
            var filter = SearchClient.Instance.BuildFilter<FindProduct>();
            return grapeList.Aggregate(filter, (current, grape) => current.Or(x => x.GrapeMixList.Match(grape)));
        }
        private FilterBuilder<FindProduct> GetOrFilterForStringList(List<string> fieldValues, IClient client, string fieldName)
        {
            // Appends type convention to field name (like "$$string")
            string fullFieldName = client.GetFullFieldName(fieldName);

            List<Filter> filters = new List<Filter>();
            foreach (string s in fieldValues)
            {
                filters.Add(new TermFilter(fullFieldName, s));
            }

            OrFilter orFilter = new OrFilter(filters);
            FilterBuilder<FindProduct> filterBuilder = new FilterBuilder<FindProduct>(client, orFilter);
            return filterBuilder;
        }

        private Expression<Func<FindProduct, object>> GetTermFacetForResult(string fieldName)
        {
            string fullFieldName = SearchClient.Instance.GetFullFieldName(fieldName);

            var paramX = Expression.Parameter(typeof(FindProduct), "x");
            var property = Expression.Property(paramX, fieldName);
            var expr = Expression.Lambda<Func<FindProduct, object>>(property, paramX);

            return expr;
        }
    }

    public class FacetValues
    {
        public FacetValues()
        {
            Values = new List<FacetValue>();
        }

        public FacetDefinition Definition { get; set; }
        public List<FacetValue> Values { get; set; }
    }


    public class FacetValue
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public bool Selected { get; set; }
    }


    public static class SearchExtensions
    {
        public static ITypeSearch<TSource> TermsFacetFor<TSource>(this ITypeSearch<TSource> search, string name, int size)
        {
            return search.TermsFacetFor(name, FacetRequestAction(search.Client, name, size));

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
            return fieldName + searchClient.Conventions.FieldNameConvention.GetFieldName(Expression.Variable(typeof(string), fieldName));
        }

    }
}
