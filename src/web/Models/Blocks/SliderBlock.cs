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
using EPiServer.SpecializedProperties;
using OxxCommerceStarterKit.Core.Attributes;
using OxxCommerceStarterKit.Web.Business.Rendering;
using OxxCommerceStarterKit.Web.EditorDescriptors.SelectionFactories;

namespace OxxCommerceStarterKit.Web.Models.Blocks
{
	[ContentType(GUID = "c1a6d014-fa3f-4786-b35d-fa78940a1fdd",
        DisplayName = "Slider",
        Description = "Show multiple content items in a configurable slider",
        GroupName = "Multimedia"
        )]
    [SiteImageUrl(thumbnail: EditorThumbnail.Multimedia)]
    public class SliderBlock : SiteBlockData, IDefaultDisplayOption
	{

	    [Display(
	        GroupName = SystemTabNames.Content,
	        Order = 20,
	        Name = "Slider Content")]
	    [CultureSpecific(false)]
	    public virtual ContentArea SliderContent { get; set; }


	    [Display(
	        GroupName = SystemTabNames.Content,
	        Order = 10,
	        Name = "Slider Height",
            Description = "Default height is 500 pixels")]
	    [CultureSpecific(false)]
	    public virtual  int Height { get; set; }


		[ScaffoldColumn(false)]
	    public string Tag {
	        get { return string.Empty; }
	    }

	    [Display(
	        GroupName = SystemTabNames.Content,
	        Order = 8,
	        Name = "Layout")]
	    [CultureSpecific(false)]
        [SelectOne(SelectionFactoryType = typeof(SliderLayoutSelectionFactory))]
        [UIHint("FloatingEditor")]
	    public virtual string Layout { get; set; }

	    public override void SetDefaultValues(ContentType contentType)
	    {
	        base.SetDefaultValues(contentType);
	        Height = 500;
	        Layout = "fullwidth";
	    }
	}
}
