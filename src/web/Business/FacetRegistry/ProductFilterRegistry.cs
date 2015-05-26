using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EPiServer.ServiceLocation;
using OxxCommerceStarterKit.Web.Models.FindModels;

namespace OxxCommerceStarterKit.Web.Business.FacetRegistry
{
    [ServiceConfiguration(typeof(IProductFilterRegistry))]
    public class ProductFilterRegistry : IProductFilterRegistry
    {
        public List<FilterAttribute> GetFilters(Type filterAttributeType = null, Type indexedProductType = null)
        {
            List<FilterAttribute> filterAttributes = new List<FilterAttribute>();

            if (indexedProductType == null)
            {
                indexedProductType = typeof(FindProduct);
            }

            if(filterAttributeType == null)
            {
                filterAttributeType = typeof (FilterAttribute);
            }


            IEnumerable<PropertyInfo> properties = indexedProductType.GetProperties()
                             .Where(prop => prop.IsDefined(filterAttributeType, false));

            foreach (PropertyInfo property in properties)
            {
                var attribute = (FilterAttribute)property.GetCustomAttributes(filterAttributeType, false).FirstOrDefault();
                if (attribute != null)
                {
                    attribute.PropertyName = property.Name;
                    filterAttributes.Add(attribute);
                }
            }

            // Sort it
            filterAttributes.Sort((firstPair, nextPair) => String.Compare(
                firstPair.DisplayName, 
                nextPair.DisplayName, 
                StringComparison.Ordinal));

            return filterAttributes;
        }
    }
}