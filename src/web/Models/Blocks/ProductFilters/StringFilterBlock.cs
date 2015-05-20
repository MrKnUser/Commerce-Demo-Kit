using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Find;
using OxxCommerceStarterKit.Core.Attributes;
using OxxCommerceStarterKit.Web.Extensions;
using OxxCommerceStarterKit.Web.Models.FindModels;

namespace OxxCommerceStarterKit.Web.Models.Blocks.ProductFilters
{
    [ContentType(DisplayName = "String Filter",
        GUID = "efcb0aef-5427-49bb-ab1b-2b429a2f2cc3", 
        Description = "Filter product search blocks by field values",
        GroupName = WebGlobal.GroupNames.Commerce)]
    [SiteImageUrl(thumbnail: EditorThumbnail.Commerce)]
    public class StringFilterBlock : FilterBaseBlock
    {

        [CultureSpecific]
        [Display(
            Name = "Field Name",
            Description = "Name of field in index",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual String FieldName { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 5,
            Name = "Filter Value",
            Description = "The value to filter search results on")]
        [CultureSpecific(true)]
        public virtual string FieldValue { get; set; }


        public override ITypeSearch<FindProduct> ApplyFilter(ITypeSearch<FindProduct> query)
        {
            if(string.IsNullOrEmpty(FieldName) == false && string.IsNullOrEmpty(FieldValue) == false)
            {
                query = query.AddStringFilter(FieldValue, FieldName);
            }
            return query;
        }
    }
}