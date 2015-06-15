/*
Commerce Starter Kit for EPiServer

All rights reserved. See LICENSE.txt in project root.

Copyright (C) 2013-2014 Oxx AS
Copyright (C) 2013-2014 BV Network AS

*/

using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using OxxCommerceStarterKit.Web.Models.CustomProperties;

namespace OxxCommerceStarterKit.Web.Models.PageTypes
{
    public abstract class SitePage : PageData
    {
        [Display(
            GroupName = WebGlobal.GroupNames.MetaData,
            Order = 100)]
        [CultureSpecific]
        public virtual string MetaTitle
        {
            get
            {
                var metaTitle = this.GetPropertyValue(p => p.MetaTitle);

                // Use explicitly set meta title, otherwise fall back to page name
                return !string.IsNullOrWhiteSpace(metaTitle)
                       ? metaTitle
                       : PageName;
            }
            set { this.SetPropertyValue(p => p.MetaTitle, value); }
        }

        [Display(
            GroupName = WebGlobal.GroupNames.MetaData,
            Order = 200)]
        [CultureSpecific]
        [BackingType(typeof(PropertyStringList))]
        public virtual string[] MetaKeywords { get; set; }

        [Display(
            GroupName = WebGlobal.GroupNames.MetaData,
            Order = 300)]
        [CultureSpecific]
        [UIHint(UIHint.LongString)]
        public virtual string MetaDescription { get; set; }

        [Display(
            GroupName = WebGlobal.GroupNames.MetaData,
            Order = 400)]
        [CultureSpecific]
        public virtual bool DisableIndexing { get; set; }

    }
}
