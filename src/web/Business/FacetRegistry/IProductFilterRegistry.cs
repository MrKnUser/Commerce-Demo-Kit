using System;
using System.Collections.Generic;

namespace OxxCommerceStarterKit.Web.Business.FacetRegistry
{
    public interface IProductFilterRegistry
    {
        List<FilterAttribute> GetFilters(Type filterAttributeType = null, Type indexedProductType = null);
    }
}