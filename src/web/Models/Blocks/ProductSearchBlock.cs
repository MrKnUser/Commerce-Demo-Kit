/*
Commerce Starter Kit for EPiServer

All rights reserved. See LICENSE.txt in project root.

Copyright (C) 2013-2014 Oxx AS
Copyright (C) 2013-2014 BV Network AS

*/


using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Find;
using EPiServer.ServiceLocation;
using OxxCommerceStarterKit.Core.Attributes;
using OxxCommerceStarterKit.Core.Extensions;
using OxxCommerceStarterKit.Web.Extensions;
using OxxCommerceStarterKit.Web.Models.Blocks.Base;
using OxxCommerceStarterKit.Web.Models.Blocks.ProductFilters;
using OxxCommerceStarterKit.Web.Models.FindModels;

namespace OxxCommerceStarterKit.Web.Models.Blocks
{
    [ContentType(
        DisplayName = "Configurable Product List",
        GUID = "8BD1CF05-4980-4BA2-9304-C0EAF946DAD5",
        Description = "Configurable search block for all products, allows generic filtering.",
        GroupName = "Commerce")]
    [SiteImageUrl(thumbnail: EditorThumbnail.Commerce)]
    public class ProductSearchBlock : FindBaseBlockType
    {

        [Display(Name = "Categories",
            Description = "Root categories to get products from, includes sub categories",
            GroupName = SystemTabNames.Content, Order = 5)]
        [AllowedTypes(typeof(NodeContent))]
        public virtual ContentArea Nodes { get; set; }

        [Display(Name = "Filters",
                Description = "Filters to apply to the search result",
                Order = 10)]
        [AllowedTypes(typeof(FilterBaseBlock))]
        public virtual ContentArea Filters { get; set; }

        [Display(Name = "Priority Products",
                Description = "Products to put first in the list.",
                Order = 20)]
        [AllowedTypes(typeof(EntryContentBase))]
        public virtual ContentArea PriorityProducts { get; set; }

        /// <summary>
        /// Applies the filters selected
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        protected override ITypeSearch<FindProduct> ApplyFilters(ITypeSearch<FindProduct> query)
        {
            // Only get products from the selected nodes
            if(Nodes != null)
            {
                IEnumerable<int> nodeIds = Nodes.FilteredItems.Select(x => x.ContentLink.ID);
                query = query.AddFilterForIntList(nodeIds, "ParentCategoryId");
            }

            // Allow all filters to add their Find filters to the query
            if (Filters != null)
            {
                foreach (var item in Filters.FilteredItems)
                {
                    FilterBaseBlock filter = item.GetContent() as FilterBaseBlock;
                    if (filter != null)
                    {
                        query = filter.ApplyFilter(query);
                    }
                }
            }
            
            return query;
        }
    }

 
}
