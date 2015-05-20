/*
Commerce Starter Kit for EPiServer

All rights reserved. See LICENSE.txt in project root.

Copyright (C) 2013-2014 Oxx AS
Copyright (C) 2013-2014 BV Network AS

*/

using System;
using System.Collections.Generic;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;

namespace OxxCommerceStarterKit.Web.Models.ViewModels
{
    public class ProductViewModel<TProductContent> : IProductViewModel<TProductContent>
        where TProductContent : ProductContent        
    {
        public ProductViewModel(TProductContent currentContent)
            : base()
        {
            CatalogContent = currentContent;
        }

        public TProductContent CatalogContent { get; set; }
        public Lazy<IEnumerable<NodeContent>> ChildCategories { get; set; }
        public LazyProductViewModelCollection Products { get; set; }
        public LazyProductViewModelCollection StyleProducts { get; set; }
        public IEnumerable<IProductViewModel<ProductContent>> AllProductsSameStyle { get; set; }
        public IEnumerable<IVariationViewModel<VariationContent>> AllVariationSameStyle { get; set; }
        public IEnumerable<IProductViewModel<ProductContent>> RelatedProducts { get; set; }
        public ContentArea RelatedProductsContentArea { get; set; }
        public LazyVariationViewModelCollection Variants { get; set; }
        public EntryContentBase ContentWithAssets { get; set; }
    }
}
