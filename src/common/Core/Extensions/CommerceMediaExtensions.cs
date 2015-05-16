/*
Commerce Starter Kit for EPiServer

All rights reserved. See LICENSE.txt in project root.

Copyright (C) 2013-2014 Oxx AS
Copyright (C) 2013-2014 BV Network AS

*/

using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.SpecializedProperties;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;

namespace OxxCommerceStarterKit.Core.Extensions
{
    public static class CommerceMediaExtensions
    {
        //public static Guid AssetId(this CommerceMedia media)
        //{
        //    return new Guid(media.AssetKey);
        //}

        public static ContentReference AssetContentLink(this CommerceMedia media, IPermanentLinkMapper permanentLinkMapper)
        {
            return media.AssetLink;
            // return PermanentLinkUtility.FindContentReference(media.AssetLink, permanentLinkMapper);
        }

        public static CommerceMedia GetCommerceMedia(this EntryContentBase entry)
        {
            return GetCommerceMedia(entry, 0);
        }

        public static CommerceMedia GetCommerceMedia(this EntryContentBase entry, int index)
        {
            if (entry.CommerceMediaCollection.Any())
            {
                return entry.CommerceMediaCollection.OrderBy(m => m.SortOrder).Skip(index).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public static List<ContentReference> AssetImageUrls(this EntryContentBase entry)
        {
            var output = new List<ContentReference>();
            if (entry != null)
            {
                var permanentLinkMapper = ServiceLocator.Current.GetInstance<IPermanentLinkMapper>();

                if (entry.CommerceMediaCollection != null)
                {
                    foreach (var commerceMedia in entry.CommerceMediaCollection)
                    {
                        if (commerceMedia.GroupName == null || (commerceMedia.GroupName != null && commerceMedia.GroupName.ToLower() != "swatch"))
                        {
                            var contentLink = commerceMedia.AssetContentLink(permanentLinkMapper);
                            output.Add(contentLink);
                        }
                    }

                }
            }
            return output;
        }

        public static string AssetSwatchUrl(this EntryContentBase entry)
        {

            if (entry != null)
            {
                var permanentLinkMapper = ServiceLocator.Current.GetInstance<IPermanentLinkMapper>();
                var urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();

                if (entry.CommerceMediaCollection != null)
                {
                    foreach (var commerceMedia in entry.CommerceMediaCollection)
                    {
                        if (commerceMedia.GroupName != null && commerceMedia.GroupName.ToLower() == "swatch")
                        {
                            var contentLink = commerceMedia.AssetContentLink(permanentLinkMapper);
                            return urlResolver.GetUrl(contentLink);
                        }
                    }

                    // Use first
                    string defaultImage = entry.GetDefaultImage("swatch");
                    return defaultImage;
                }
            }
            return null;
        }
    }
}
