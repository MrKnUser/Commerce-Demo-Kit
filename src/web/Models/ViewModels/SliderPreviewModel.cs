/*
Commerce Starter Kit for EPiServer

All rights reserved. See LICENSE.txt in project root.

Copyright (C) 2013-2014 Oxx AS
Copyright (C) 2013-2014 BV Network AS

*/

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using OxxCommerceStarterKit.Web.Models.Blocks;
using OxxCommerceStarterKit.Web.Models.PageTypes;

namespace OxxCommerceStarterKit.Web.Models.ViewModels
{
    public class SliderPreviewModel : PageViewModel<SitePage>
    {
        public SliderPreviewModel(SitePage currentPage, IContent previewContent)
            : base(currentPage)
        {
            PreviewContent = previewContent;
            var typedContent = ((SliderBlock)previewContent);
            if (typedContent != null)
            {
                Height = typedContent.Height;
                Layout = typedContent.Layout;
            }
        }

        public IContent PreviewContent { get; set; }
        public ContentArea SliderContent { get; set; }
        public int Height { get; set; }
        public string Layout { get; set; }

    }
}
