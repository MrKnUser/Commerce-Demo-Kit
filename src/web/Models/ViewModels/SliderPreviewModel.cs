/*
Commerce Starter Kit for EPiServer

All rights reserved. See LICENSE.txt in project root.

Copyright (C) 2013-2014 Oxx AS
Copyright (C) 2013-2014 BV Network AS

*/

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using OxxCommerceStarterKit.Web.Models.PageTypes;

namespace OxxCommerceStarterKit.Web.Models.ViewModels
{
    public class SliderPreviewModel : PageViewModel<SitePage>
    {
        public SliderPreviewModel(SitePage currentPage, IContent previewContent)
            : base(currentPage)
        {
            PreviewContent = previewContent;
        }

        public IContent PreviewContent { get; set; }
        public ContentArea SliderContent { get; set; }

    }
}
