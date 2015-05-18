using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Find;
using EPiServer.Find.Framework;
using OxxCommerceStarterKit.Web.Models.FindModels;

namespace OxxCommerceStarterKit.Web.Business.Search
{
    public static class FilterUtil
    {
        public static FilterBuilder<FindProduct> GetProductCategoryFilter(this IClient client, List<int> categories)
        {
            var filter = client.BuildFilter<FindProduct>();
            return categories.Aggregate(filter, (current, category) => current.Or(x => x.ParentCategoryId.Match(category)));
        }

    }
}