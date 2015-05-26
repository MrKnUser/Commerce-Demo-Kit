/*
Commerce Starter Kit for EPiServer

All rights reserved. See LICENSE.txt in project root.

Copyright (C) 2013-2014 Oxx AS
Copyright (C) 2013-2014 BV Network AS

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Mediachase.Commerce;
using OxxCommerceStarterKit.Core.Extensions;
using OxxCommerceStarterKit.Core.Models;
using OxxCommerceStarterKit.Web.Models.Catalog;
using OxxCommerceStarterKit.Web.Models.Catalog.Base;

namespace OxxCommerceStarterKit.Web.Models.ViewModels
{
    public class ProductListViewModel
    {
        private UrlResolver _urlResolver;

        public ProductListViewModel()
        {
            _urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
        }

        public ProductListViewModel(VariationContent content, Mediachase.Commerce.IMarket currentMarket) : this()
        {
            

            Code = content.Code;
            ContentLink = content.ContentLink;
            DisplayName = content.DisplayName;
            ProductUrl = _urlResolver.GetUrl(ContentLink);
            ImageUrl = content.GetDefaultImage();
            PriceString = content.GetDisplayPrice(currentMarket);
            PriceAmount = content.GetDefaultPriceAmount(currentMarket);
            ContentType = content.GetType().Name;
            IsVariation = true;
            var discountPriceAmount =  content.GetDiscountPrice();
            DiscountPriceAmount = GetPriceWithCheck(discountPriceAmount);
            DiscountPriceString = GetDisplayPriceWithCheck(discountPriceAmount);

        }

        public ProductListViewModel(ProductBase content, IMarket currentMarket) : this()
        {
            Code = content.Code;
            ContentLink = content.ContentLink;
            DisplayName = content.DisplayName;
            ProductUrl = _urlResolver.GetUrl(ContentLink);
            ImageUrl = content.GetDefaultImage();
            ContentType = content.GetType().Name;
            IsVariation = false;
        }

        private double GetPriceWithCheck(PriceAndMarket price)
        {
            return price != null ? (double)price.UnitPrice.Amount : 0;
        }

        private string GetDisplayPriceWithCheck(PriceAndMarket price)
        {
            return price != null ? price.UnitPrice.ToString() : string.Empty;
        }


        public string Code { get; set; }
        public string DisplayName { get; set; }
        public string NewItemText { get; set; }
        public XhtmlString Description { get; set; }
        public ContentReference ContentLink { get; set; }
        public string PriceString { get; set; }
        public double PriceAmount { get; set; }
        public string DiscountPriceString { get; set; }
        public double DiscountPriceAmount { get; set; }
        public string BrandName { get; set; }
        public Dictionary<string, ContentReference> Images { get; set; }
        public Dictionary<string, string> Variants { get; set; }
        public string Country { get; set; }
        public string ProductUrl { get; set; }
        public string ImageUrl { get; set; }
        public string ContentType { get; set; }
        public double AverageRating { get; set; }
        public List<string> AllImageUrls { get; set; }
        public XhtmlString Overview { get; set; }
        public bool IsVariation { get; set; }
    }
}
