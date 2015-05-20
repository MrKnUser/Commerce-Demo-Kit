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
    public interface ICatalogViewModel<out T> : IBaseCatalogViewModel<T>
        where T : CatalogContentBase
    {
        Lazy<IEnumerable<NodeContent>> ChildCategories { get; set; }
        LazyProductViewModelCollection Products { get; set; }
        LazyProductViewModelCollection StyleProducts { get; set; }
        IEnumerable<IProductViewModel<ProductContent>> AllProductsSameStyle { get; set; }
        IEnumerable<IVariationViewModel<VariationContent>> AllVariationSameStyle { get; set; }
        IEnumerable<IProductViewModel<ProductContent>> RelatedProducts { get; set; }
        ContentArea RelatedProductsContentArea { get; set; }
        LazyVariationViewModelCollection Variants { get; set; }
        EntryContentBase ContentWithAssets { get; set; }
    }
}
