/*
Commerce Starter Kit for EPiServer

All rights reserved. See LICENSE.txt in project root.

Copyright (C) 2013-2014 Oxx AS
Copyright (C) 2013-2014 BV Network AS

*/

using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using OxxCommerceStarterKit.Web.Extensions;
using OxxCommerceStarterKit.Web.Models.PageTypes;
using OxxCommerceStarterKit.Web.Models.ViewModels.Contracts;

namespace OxxCommerceStarterKit.Web.Models.ViewModels
{
	public class PageListBlockItemViewModel
	{
		private string _imageUrl;
		public string ImageUrl
		{
			get
			{
				if (!string.IsNullOrEmpty(_imageUrl) && !string.IsNullOrEmpty(ImageExtraUrlParameters))
				{
					if (_imageUrl.Contains("?"))
					{
                        return _imageUrl + "&" + ImageExtraUrlParameters;
					}
					else
					{
						return _imageUrl + "?" + ImageExtraUrlParameters;
					}
				}

				return _imageUrl;
			}
		}
		public string Title { get; set; }
		public string Text { get; set; }
		public ContentReference ContentLink { get; set; }

		public string ImageExtraUrlParameters { get; set; }

		public PageListBlockItemViewModel(PageData page)
		{
		    IHasListViewContentItem hasListViewContent = page as IHasListViewContentItem;
            if(hasListViewContent != null)
            {
                var urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();

                var item = hasListViewContent.GetListViewContentItem();
                Title = item.Title;
                Text = item.Intro;
                ContentLink = item.ContentLink;
                if(ContentReference.IsNullOrEmpty(item.ContentLink) == false)
                {
                    _imageUrl = urlResolver.GetUrl(item.ImageUrl);
                }
            }
            else
            {
                // Fallback for other types
                Title = page.Name;
                ContentLink = page.ContentLink;
            }

		}
	}
}
