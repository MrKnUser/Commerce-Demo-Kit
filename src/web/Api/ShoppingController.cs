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
using System.Web.Http;
using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Api.Querying.Filters;
using EPiServer.Find.Framework;
using EPiServer.Find.Framework.Statistics;
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
            public string SearchTerm { get; set; }
            public List<FacetValues> Facets { get; set; }
            public string SelectedFacetName { get; set; }

        }

        [HttpPost]
        public JObject GetProducts2(ProductSearchData productSearchData)
        {
            // If we do not get any facets as part of query, we use the default facets from the registry
            var facetRegistry = ServiceLocator.Current.GetInstance<FacetRegistry>();
            if (productSearchData.ProductData.Facets == null || productSearchData.ProductData.Facets.Any() == false)
            {
                productSearchData.ProductData.Facets = new List<FacetValues>();
                foreach (FacetDefinition definition in facetRegistry.FacetDefinitions)
                {
                    var valuesForFacet = new FacetValues()
                    {
                        Definition = definition
                    };
                    productSearchData.ProductData.Facets.Add(valuesForFacet);
                }
            }

            // The language is part of the route
            string language = Language;

            // search term, used if part of freetext search
            var searchTerm = productSearchData.ProductData.SearchTerm;
            SearchResults<FindProduct> productsSearchResult = GetProductsForSearchAndFilters(productSearchData, searchTerm, language);

            string selectedFacetName = productSearchData.ProductData.SelectedFacetName ?? string.Empty;
            var productFacetsResult = GetFacetResult(productSearchData, language, productSearchData.ProductData.Facets, searchTerm);

            // parse facet results
            IEnumerable<TermCount> productCategoryFacetsResult = productFacetsResult.TermsFacetFor(x => x.ParentCategoryName).Terms;
            List<FacetViewModel> allProductCategoryFacets = CreateCategoryFacetViewModels(productCategoryFacetsResult,
                productSearchData.ProductData.SelectedProductCategories.Select(x => x.ToString()).ToList());

            // If we're doing a filtering on a specific facet, we handle that one
            // in a special way, in order to show the count for all terms for the facet
            SearchResults<FindProduct> productSelectedFacetsResult = null;
            if (string.IsNullOrEmpty(selectedFacetName) == false)
            {
                List<FacetValues> selectedFacet = new List<FacetValues>();
                if (productSearchData.ProductData.Facets != null)
                {
                    FacetValues selectedValues = productSearchData.ProductData.Facets.Find(f => f.Definition.Name.Equals(selectedFacetName));
                    if (selectedValues != null)
                        selectedFacet.Add(selectedValues);
                }


                // Get facet values and counts for the selected facet
                productSelectedFacetsResult = GetFacetResult(
                    productSearchData,
                    language,
                    selectedFacet,
                    searchTerm, applyFacetFilters: false);
            }


            // var productMainCategoryFacetResult = productFacetsResult.TermsFacetFor(x => x.MainCategoryName).Terms;
            //var allMainCategoryFacets = CreateFacetViewModels(productMainCategoryFacetResult,
            //    productSearchData.ProductData.SelectedMainCategoryFacets);

            //Facets - To get all, color, size and fit facets, based on selected product categories
            //var facetsQuery = SearchClient.Instance.Search<FindProduct>(GetLanguage(language));

            //// search text in common fields
            //facetsQuery = ApplyTermFilter(productSearchData, facetsQuery);

            //// common filters (language, show in list)
            //facetsQuery = ApplyCommonFilters(productSearchData, facetsQuery, language);

            //facetsQuery = facetsQuery
            //    .Filter(x => GetCategoryFilter(productSearchData.ProductData.SelectedProductCategories))
            //    //.Filter(x => GetMainCategoryFilter(productSearchData.ProductData.SelectedMainCategoryFacets))
            //    .TermsFacetFor(x => x.Color, r => r.Size = 50)
            //    .TermsFacetFor(x => x.Fit, r => r.Size = 50)
            //    .TermsFacetFor(x => x.SizesList, r => r.Size = 200)
            //    .TermsFacetFor(x => x.GrapeMixList, r => r.Size = 50);

            //facetsQuery = facetsQuery
            //    .TermsFacetFor("Region", 50)
            //    .TermsFacetFor("Country", 50);

            // execute search
            //var facetsResult = facetsQuery.
            //    Take(0)
            //    .StaticallyCacheFor(TimeSpan.FromMinutes(1))
            //    .GetResult();

            // results
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


            // Get all facet values based on facet registry

            var facetsAndValues = GetFacetsAndValues(productSearchData, facetRegistry, productFacetsResult, productSelectedFacetsResult);

            var result = new
            {
                products = productsSearchResult.ToList(),
                productCategoryFacets = allProductCategoryFacets,
                facets = facetsAndValues,
                totalResult = productsSearchResult.TotalMatching
            };

            var serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Auto;

            return JObject.FromObject(result, serializer);

        }

        private List<FacetValues> GetFacetsAndValues(ProductSearchData productSearchData, FacetRegistry facetRegistry,
            SearchResults<FindProduct> productFacetsResult, SearchResults<FindProduct> productSelectedFacetsResult)
        {
            List<FacetValues> facetsAndValues = new List<FacetValues>();
            foreach (FacetDefinition definition in facetRegistry.FacetDefinitions)
            {
                Facet facet = productFacetsResult.Facets.FirstOrDefault(f => f.Name.Equals(definition.FieldName));

                if (productSelectedFacetsResult != null)
                {
                    Facet selectedfacet =
                        productSelectedFacetsResult.Facets.FirstOrDefault(f => f.Name.Equals(definition.FieldName));
                    if (selectedfacet != null)
                    {
                        facet = selectedfacet;
                    }
                }

                if (facet != null)
                {
                    var valuesForFacet = new FacetValues()
                    {
                        Definition = definition
                    };

                    // The definition must also keep track
                    // of what facets are selected 
                    valuesForFacet.Definition.PopulateFacet(facet);

                    // TODO: Remove when we have a generic way to track selected facets
                    TermsFacet termsFacet = facet as TermsFacet;
                    if (termsFacet != null)
                    {

                        foreach (TermCount termCount in termsFacet.Terms)
                        {
                            valuesForFacet.Values.Add(new FacetValue()
                            {
                                Count = termCount.Count,
                                Name = termCount.Term,
                                Selected =
                                    GetIsSelected(definition.FieldName, termCount.Term,
                                        productSearchData.ProductData.Facets)
                            });
                        }
                    }
                    else
                    {
                        // Use the incoming facet to mark selected values
                        FacetValues incomingFacet = GetFacetFromList(productSearchData.ProductData.Facets, definition.Name);
                        // Todo - this won't work, just a workaround as we flush out the design
                        if (incomingFacet != null)
                        {
                            valuesForFacet.Values = incomingFacet.Values;
                        }

                    }


                    facetsAndValues.Add(valuesForFacet);
                }
            }
            return facetsAndValues;
        }


        [HttpPost]
        public object GetProducts(ProductSearchData productSearchData)
        {
            //try
            //{

            // If we do not get any facets as part of query, we use the default facets from the registry
            var facetRegistry = ServiceLocator.Current.GetInstance<FacetRegistry>();
            if (productSearchData.ProductData.Facets == null || productSearchData.ProductData.Facets.Any() == false)
            {
                productSearchData.ProductData.Facets = new List<FacetValues>();
                foreach (FacetDefinition definition in facetRegistry.FacetDefinitions)
                {
                    var valuesForFacet = new FacetValues()
                    {
                        Definition = definition
                    };
                    productSearchData.ProductData.Facets.Add(valuesForFacet);
                }
            }

            // The language is part of the route
            string language = Language;

            // search term, used if part of freetext search
            var searchTerm = productSearchData.ProductData.SearchTerm;
            SearchResults<FindProduct> productsSearchResult = GetProductsForSearchAndFilters(productSearchData, searchTerm, language);

            string selectedFacetName = productSearchData.ProductData.SelectedFacetName ?? string.Empty;
            var productFacetsResult = GetFacetResult(productSearchData, language, productSearchData.ProductData.Facets, searchTerm);

            // parse facet results
            IEnumerable<TermCount> productCategoryFacetsResult = productFacetsResult.TermsFacetFor(x => x.ParentCategoryName).Terms;
            List<FacetViewModel> allProductCategoryFacets = CreateCategoryFacetViewModels(productCategoryFacetsResult,
                productSearchData.ProductData.SelectedProductCategories.Select(x => x.ToString()).ToList());

            // If we're doing a filtering on a specific facet, we handle that one
            // in a special way, in order to show the count for all terms for the facet
            SearchResults<FindProduct> productSelectedFacetsResult = null;
            if (string.IsNullOrEmpty(selectedFacetName) == false)
            {
                List<FacetValues> selectedFacet = new List<FacetValues>();
                if (productSearchData.ProductData.Facets != null)
                {
                    FacetValues selectedValues = productSearchData.ProductData.Facets.Find(f => f.Definition.Name.Equals(selectedFacetName));
                    if (selectedValues != null)
                        selectedFacet.Add(selectedValues);
                }


                // Get facet values and counts for the selected facet
                productSelectedFacetsResult = GetFacetResult(
                    productSearchData,
                    language,
                    selectedFacet,
                    searchTerm, applyFacetFilters: false);
            }


            // var productMainCategoryFacetResult = productFacetsResult.TermsFacetFor(x => x.MainCategoryName).Terms;
            //var allMainCategoryFacets = CreateFacetViewModels(productMainCategoryFacetResult,
            //    productSearchData.ProductData.SelectedMainCategoryFacets);

            //Facets - To get all, color, size and fit facets, based on selected product categories
            //var facetsQuery = SearchClient.Instance.Search<FindProduct>(GetLanguage(language));

            //// search text in common fields
            //facetsQuery = ApplyTermFilter(productSearchData, facetsQuery);

            //// common filters (language, show in list)
            //facetsQuery = ApplyCommonFilters(productSearchData, facetsQuery, language);

            //facetsQuery = facetsQuery
            //    .Filter(x => GetCategoryFilter(productSearchData.ProductData.SelectedProductCategories))
            //    //.Filter(x => GetMainCategoryFilter(productSearchData.ProductData.SelectedMainCategoryFacets))
            //    .TermsFacetFor(x => x.Color, r => r.Size = 50)
            //    .TermsFacetFor(x => x.Fit, r => r.Size = 50)
            //    .TermsFacetFor(x => x.SizesList, r => r.Size = 200)
            //    .TermsFacetFor(x => x.GrapeMixList, r => r.Size = 50);

            //facetsQuery = facetsQuery
            //    .TermsFacetFor("Region", 50)
            //    .TermsFacetFor("Country", 50);

            // execute search
            //var facetsResult = facetsQuery.
            //    Take(0)
            //    .StaticallyCacheFor(TimeSpan.FromMinutes(1))
            //    .GetResult();

            // results
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


            // Get all facet values based on facet registry
            var facetsAndValues = GetFacetsAndValues(productSearchData, facetRegistry, productFacetsResult, productSelectedFacetsResult);
            //List<FacetValues> facetsAndValues = new List<FacetValues>();
            //foreach (FacetDefinition definition in facetRegistry.FacetDefinitions)
            //{
            //    Facet facet = productFacetsResult.Facets.FirstOrDefault(f => f.Name.Equals(definition.FieldName));

            //    if (productSelectedFacetsResult != null)
            //    {
            //        Facet selectedfacet = productSelectedFacetsResult.Facets.FirstOrDefault(f => f.Name.Equals(definition.FieldName));
            //        if (selectedfacet != null)
            //        {
            //            facet = selectedfacet;
            //        }
            //    }

            //    if (facet != null)
            //    {
            //        var valuesForFacet = new FacetValues()
            //        {
            //            Definition = definition
            //        };

            //        TermsFacet termsFacet = facet as TermsFacet;
            //        if (termsFacet != null)
            //        {

            //            foreach (TermCount termCount in termsFacet.Terms)
            //            {
            //                valuesForFacet.Values.Add(new FacetValue()
            //                {
            //                    Count = termCount.Count,
            //                    Name = termCount.Term,
            //                    Selected = GetIsSelected(definition.FieldName, termCount.Term, productSearchData.ProductData.Facets)
            //                });
            //            }
            //        }

            //        facetsAndValues.Add(valuesForFacet);
            //    }
            //}

            var result = new
            {
                products = productsSearchResult.ToList(),
                productCategoryFacets = allProductCategoryFacets,
                facets = facetsAndValues,
                totalResult = productsSearchResult.TotalMatching
            };

            var serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Auto;

            return JObject.FromObject(result, serializer);

            //}
            //catch (OperationCanceledException operationCanceledException)
            //{
            //    _log.Error("Failed to perform search", operationCanceledException);
            //    return null;
            //}
            //catch (ServiceException serviceException)
            //{
            //    _log.Error("Failed to perform search", serviceException);
            //    return null;
            //}
        }

        protected SearchResults<FindProduct> GetProductsForSearchAndFilters(ProductSearchData productSearchData, string searchTerm, string language)
        {
            var productsQuery = SearchClient.Instance.Search<FindProduct>(GetFindLanguage(language));

            productsQuery = ApplyTermFilter(productsQuery, searchTerm, trackSearchTerm: true);

            // common filters
            productsQuery = ApplyCommonFilters(productsQuery, language);

            // selected categories
            if (productSearchData.ProductData.SelectedProductCategories != null &&
                productSearchData.ProductData.SelectedProductCategories.Any())
            {
                productsQuery =
                    productsQuery.Filter(x => GetCategoryFilter(productSearchData.ProductData.SelectedProductCategories));
            }

            // Add filters from the passed in fasets
            foreach (FacetValues fv in productSearchData.ProductData.Facets)
            {
                // Get all facet values for a facet definition which has been selected
                var selectedFacetValues = fv.Values.Where(x => x.Selected.Equals(true)).Select(x => x.Name).ToList();
                if (selectedFacetValues.Any())
                {
                    // and then add the values as a filter
                    // productsQuery = fv.Definition.Filter(productsQuery);
                    productsQuery = productsQuery.AddStringListFilter(selectedFacetValues, fv.Definition.FieldName);
                }
            }

            // execute product search
            productsQuery = productsQuery
                .Skip((productSearchData.Page - 1) * productSearchData.PageSize)
                .Take(productSearchData.PageSize)
                .StaticallyCacheFor(TimeSpan.FromMinutes(1));
            var productsSearchResult = productsQuery.GetResult();
            return productsSearchResult;
        }

        private SearchResults<FindProduct> GetFacetResult(ProductSearchData productSearchData, string language, List<FacetValues> facets, string searchTerm, bool applyFacetFilters = true)
        {
            var productFacetQuery = SearchClient.Instance.Search<FindProduct>(GetFindLanguage(language));

            // search term
            productFacetQuery = ApplyTermFilter(productFacetQuery, searchTerm);

            // common filters
            productFacetQuery = ApplyCommonFilters(productFacetQuery, language);


            // categories
            //if (productSearchData.ProductData.SelectedMainCategoryFacets != null &&
            //    productSearchData.ProductData.SelectedMainCategoryFacets.Any())
            //{
            //    productFacetQuery =
            //        productFacetQuery.Filter(
            //            x => GetMainCategoryFilter(productSearchData.ProductData.SelectedMainCategoryFacets));
            //}


            productFacetQuery = productFacetQuery
                .TermsFacetFor(x => x.ParentCategoryName)
                .TermsFacetFor(x => x.MainCategoryName);

            if (facets != null && facets.Any())
            {
                foreach (FacetValues fv in facets)
                {
                    // productFacetQuery = productFacetQuery.TermsFacetFor(fv.Definition.FieldName, 50);
                    productFacetQuery = fv.Definition.Facet(productFacetQuery);
                }

                if (applyFacetFilters)
                {
                    foreach (FacetValues fv in facets)
                    {
                        var selectedFacets = fv.Values.Where(x => x.Selected.Equals(true)).Select(x => x.Name).ToList();
                        if (selectedFacets.Any())
                        {
                            productFacetQuery = productFacetQuery.AddStringListFilter(selectedFacets, fv.Definition.FieldName);
                        }
                    }

                }
            }

            var productFacetsResult = productFacetQuery
                .Take(0)
                .GetResult();
            return productFacetsResult;
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

        private FacetValues GetFacetFromList(List<FacetValues> facets, string facetName)
        {
            if (facets == null)
                return null;
            foreach (var facetValue in facets)
            {
                if (facetValue.Definition.Name.Equals(facetName))
                {
                    return facetValue;
                }
            }
            return null;
        }


        private static ITypeSearch<FindProduct> ApplyCommonFilters(ITypeSearch<FindProduct> query, string language)
        {
            return query.Filter(x => x.Language.Match(language))
                .Filter(x => x.ShowInList.Match(true));
        }

        /// <summary>
        /// Adds freetext search filter to known fields
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="trackSearchTerm">if set to <c>true</c> [track search term].</param>
        /// <returns></returns>
        private ITypeSearch<FindProduct> ApplyTermFilter(ITypeSearch<FindProduct> query, string searchTerm, bool trackSearchTerm = false)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.For(searchTerm)
                    .InFields(x => x.Name,
                        x => x.MainCategoryName,
                        x => string.Join(",", x.Color),
                        x => x.DisplayName,
                        x => x.Fit,
                        x => x.Description.ToString(),
                        x => string.Join(",", x.ParentCategoryName))
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
            List<FacetViewModel> list = new List<FacetViewModel>();

            foreach (var term in facetResult)
            {
                var id = term.Term;
                var name = term.Term;

                if (term.Term.Contains("|"))
                {
                    string[] idAndName = term.Term.Split('|');
                    id = idAndName[0];
                    name = idAndName[1];
                }

                var model = new FacetViewModel
                {
                    Count = term.Count,
                    Id = id,
                    Name = name,
                    Selected = selectedFacets != null && selectedFacets.Contains(id)
                };
                list.Add(model);
            }

            return list;
        }


        private FilterBuilder<FindProduct> GetCategoryFilter(List<int> categories)
        {
            var filter = SearchClient.Instance.BuildFilter<FindProduct>();
            return categories.Aggregate(filter, (current, category) => current.Or(x => x.ParentCategoryId.Match(category)));
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
        private FilterBuilder<FindProduct> GetGrapeFilter(List<string> grapeList)
        {
            var filter = SearchClient.Instance.BuildFilter<FindProduct>();
            return grapeList.Aggregate(filter, (current, grape) => current.Or(x => x.GrapeMixList.Match(grape)));
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
        public static Expression<Func<T, object>> GetTermFacetForResult<T>(string fieldName)
        {
            ParameterExpression paramX = Expression.Parameter(typeof(T), "x");
            MemberExpression property = Expression.Property(paramX, fieldName);
            Expression conversion = Expression.Convert(property, typeof(object));
            Expression<Func<T, object>> expr = Expression.Lambda<Func<T, object>>(conversion, paramX);

            return expr;
        }

        public static ITypeSearch<TSource> NumericRangeFacetFor<TSource>(this ITypeSearch<TSource> search, string name, List<NumericRange> range, Type backingType)
        {
            return search.RangeFacetFor(GetTermFacetForResult<TSource>(name),
                NumericRangfeFacetRequestAction(search.Client, name, range, backingType));

        }

        public static ITypeSearch<TSource> NumericRangeFacetFor<TSource>(this ITypeSearch<TSource> search, string name, List<NumericRange> range)
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

        private static Action<NumericRangeFacetRequest> NumericRangfeFacetRequestAction(IClient searchClient, string fieldName, List<NumericRange> range, Type type)
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
                x.AllTerms = true;
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
                return query.Filter(GetOrFilterForStringList<T>(stringFieldValues, SearchClient.Instance, fieldName));
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

    }
}
