using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Find;
using EPiServer.Find.Api.Querying.Filters;
using OxxCommerceStarterKit.Core.Attributes;
using OxxCommerceStarterKit.Web.Extensions;
using OxxCommerceStarterKit.Web.Models.FindModels;

namespace OxxCommerceStarterKit.Web.Models.Blocks.ProductFilters
{
    [ContentType(DisplayName = "Exists Filter",
        GUID = "E93C9A50-4B62-4116-8E56-1DF84AB93EF7", 
        Description = "Filter product that has a value for the given field",
        GroupName = WebGlobal.GroupNames.Commerce)]
    [SiteImageUrl(thumbnail: EditorThumbnail.Commerce)]
    public class ExistsFilterBlock : FilterBaseBlock
    {

        [CultureSpecific]
        [Display(
            Name = "Field Name",
            Description = "Name of field in index",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual String FieldName { get; set; }

        public override ITypeSearch<FindProduct> ApplyFilter(ITypeSearch<FindProduct> query)
        {
            if(string.IsNullOrEmpty(FieldName) == false)
            {
                string fullFieldName = query.Client.GetFullFieldName(FieldName);
                query = query.Filter(new ExistsFilter(fullFieldName));
            }
            return query;
        }
    }
}