using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BVNetwork.NotFound.Core;
using EPiServer;
using EPiServer.Find;
using EPiServer.Find.Api;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;
using OxxCommerceStarterKit.Web.Models.FindModels;

namespace OxxCommerceStarterKit.Web.Business.Seo
{
    public class CustomProductRedirectHandler : INotFoundHandler
    {
        public string RewriteUrl(string url)
        {
            if(url.Contains("productid"))
            {
                // Give it a thorough look - see if we can redirect it
                Url uri = new Url(url);
                string[] productIds = uri.QueryCollection.GetValues("productid");
                if(productIds != null && productIds.Any())
                {
                    string productId = productIds.FirstOrDefault();

                    if (productId != null && string.IsNullOrEmpty(productId) == false)
                    {
                        SearchResults<FindProduct> results = SearchClient.Instance.Search<FindProduct>()
                            .Filter(p => p.Code.MatchCaseInsensitive(productId))
                            .GetResult();
                        if (results.Hits.Any())
                        {
                            // Pick the first one
                            SearchHit<FindProduct> product = results.Hits.FirstOrDefault();
                            return product.Document.ProductUrl;
                        }
                    }
                    
                }
            }
            return null;
        }
    }
}