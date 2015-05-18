using EPiServer.Shell;
using OxxCommerceStarterKit.Web.Models.Blocks.ProductFilters;

namespace OxxCommerceStarterKit.Web.Business.UIDescriptor
{
    [UIDescriptorRegistration]
    public class FilterBlockUIDescriptor : UIDescriptor<FilterBaseBlock>
    {
        public FilterBlockUIDescriptor()
            : base()
        {
            DefaultView = CmsViewNames.AllPropertiesView;
        }
    }
}