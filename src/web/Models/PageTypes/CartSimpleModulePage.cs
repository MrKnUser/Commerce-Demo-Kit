/*
Commerce Starter Kit for EPiServer

All rights reserved. See LICENSE.txt in project root.

Copyright (C) 2013-2014 Oxx AS
Copyright (C) 2013-2014 BV Network AS

*/

using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using OxxCommerceStarterKit.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace OxxCommerceStarterKit.Web.Models.PageTypes
{
    [ContentType(GUID = "BAE8C7EE-AA11-4884-8ECC-325AB02B9E8E",
        DisplayName = "Cart Page",
        GroupName = WebGlobal.GroupNames.Commerce,
        Order = 100,
		AvailableInEditMode = false,
		Description = "")]
    [SiteImageUrl(EditorThumbnail.Commerce)]
    public class CartSimpleModulePage : CommerceSampleModulePage
    {
        [Searchable(false)]
        [Display(
            Name = "MarketingArea",
            Description = "MarketingArea",
            GroupName = WebGlobal.GroupNames.Marketing,
            Order = 1)]
       public virtual ContentArea MarketingArea { get; set; }
    }
}
