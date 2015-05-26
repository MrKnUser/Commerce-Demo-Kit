/*
Commerce Starter Kit for EPiServer

All rights reserved. See LICENSE.txt in project root.

Copyright (C) 2013-2014 Oxx AS
Copyright (C) 2013-2014 BV Network AS

*/

using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using OxxCommerceStarterKit.Core.Attributes;
using OxxCommerceStarterKit.Web.Business.Rendering;

namespace OxxCommerceStarterKit.Web.Models.Blocks
{
	[ContentType(
        DisplayName = "Title",
        Description = "Title with styling options",
        GUID = "d133d8c6-d176-4fa6-8afd-5f37285caaa4",
        GroupName="Content")]
	[SiteImageUrl(thumbnail: EditorThumbnail.Content)]
	public class TitleBlock : SiteBlockData, IDefaultDisplayOption
	{
		[Display(
			GroupName = SystemTabNames.Content,
			Order = 10)]
		[CultureSpecific]
		public virtual string Title{ get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Name = "Text Color",
            Order = 50)]
        [ClientEditor(ClientEditingClass = "dijit/ColorPalette")]
        public virtual string TextColor { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Name = "Background Color",
            Order = 60)]
        [ClientEditor(ClientEditingClass = "dijit/ColorPalette" /*, EditorConfiguration = "{\"palette\": \"3x4\"}"*/ )]
        public virtual string TextBackgroundColor { get; set; }

        [Display(
            GroupName = SystemTabNames.Settings,
            Name = "Padding",
            Description = "Padding settings as CSS (top right bottom left). Example: '1em 0 10px 0'",
            Order = 70)]
        public virtual string Padding { get; set; }

        [Display(
            GroupName = SystemTabNames.Settings,
            Name = "Margin",
            Description = "Margin settings as CSS (top right bottom left). Example: '1em 0 10px 0'",
            Order = 80)]
        public virtual string Margin { get; set; }

        public string Tag
	    {
	        get { return WebGlobal.ContentAreaTags.FullWidth; }
	    }

	}
}
