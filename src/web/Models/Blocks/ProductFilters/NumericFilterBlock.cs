using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Find;
using EPiServer.Shell.ObjectEditing;
using OxxCommerceStarterKit.Core.Attributes;
using OxxCommerceStarterKit.Web.EditorDescriptors.SelectionFactories;
using OxxCommerceStarterKit.Web.Extensions;
using OxxCommerceStarterKit.Web.Models.FindModels;

namespace OxxCommerceStarterKit.Web.Models.Blocks.ProductFilters
{


    [ContentType(DisplayName = "Numeric Filter",
        GUID = "7747D13C-D029-4CB5-B020-549676123AC4", 
        Description = "Filter product search blocks by field values",
        GroupName = WebGlobal.GroupNames.Commerce)]
    [SiteImageUrl(thumbnail: EditorThumbnail.Commerce)]
    public class NumericFilterBlock : FilterBaseBlock
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
        public virtual double FieldValue { get; set; }


        [Display(
            GroupName = SystemTabNames.Content,
            Order = 20,
            Name = "Operator")]
        [CultureSpecific(true)]
        [SelectOne(SelectionFactoryType = typeof(NumericOperatorSelectionFactory))]
        public virtual string FieldOperator { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            FieldOperator = NumericOperatorSelectionFactory.OperatorNames.Equal;
        }

        public override ITypeSearch<FindProduct> ApplyFilter(ITypeSearch<FindProduct> query)
        {
            if(string.IsNullOrEmpty(FieldName) == false)
            {
                switch (FieldOperator)
                {
                    case NumericOperatorSelectionFactory.OperatorNames.GreaterThan:
                        query = query.Filter(x => x.DiscountedPriceAmount.GreaterThan(FieldValue));
                        break;
                    case NumericOperatorSelectionFactory.OperatorNames.LessThan:
                        query = query.Filter(x => x.DiscountedPriceAmount.LessThan(FieldValue));
                        break;
                    default:
                    case NumericOperatorSelectionFactory.OperatorNames.Equal:
                        query = query.Filter(x => x.DiscountedPriceAmount.Match(FieldValue));
                        break;
                        
                }
            }
            return query;
        }
    }
}