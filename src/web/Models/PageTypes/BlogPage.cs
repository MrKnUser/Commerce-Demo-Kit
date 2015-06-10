/*
Commerce Starter Kit for EPiServer

All rights reserved. See LICENSE.txt in project root.

Copyright (C) 2013-2014 Oxx AS
Copyright (C) 2013-2014 BV Network AS

*/

using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using OxxCommerceStarterKit.Core.Attributes;

namespace OxxCommerceStarterKit.Web.Models.PageTypes
{
	[ContentType(GUID = "41dfefca-e8c0-4ed4-b251-dfcdac0389c4",
	 DisplayName = "Blog",
	 Description = "A dynamic blog template",
     GroupName = WebGlobal.GroupNames.Default,
	 Order = 100)]
    [SiteImageUrl(thumbnail: EditorThumbnail.Content)]
    public class BlogPage : ArticlePage
	{
		[Display(
			Name = "Sub page title",
			GroupName = SystemTabNames.Content,
			Order = 30)]
		[CultureSpecific]
		public virtual string SubPageTitle { get; set; }
	}
}
