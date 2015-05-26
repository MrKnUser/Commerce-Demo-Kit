/*
Commerce Starter Kit for EPiServer

All rights reserved. See LICENSE.txt in project root.

Copyright (C) 2013-2014 Oxx AS
Copyright (C) 2013-2014 BV Network AS

*/

using System.Collections.Generic;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using OxxCommerceStarterKit.Web.Business.FacetRegistry;

namespace OxxCommerceStarterKit.Web.EditorDescriptors.SelectionFactories
{
    public class FindProductFilterStringFieldSelectionFactory : GenericSelectionFactory
    {
        public override IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            IProductFilterRegistry registry = ServiceLocator.Current.GetInstance<IProductFilterRegistry>();
            var filterAttributes = registry.GetFilters(typeof(StringFilterAttribute));
            var selectionDictionary = new Dictionary<string, string>();

            foreach (FilterAttribute attribute in filterAttributes)
            {
                selectionDictionary.Add(attribute.DisplayName, attribute.PropertyName);
            }

            return GetSelectionFromDictionary(selectionDictionary);
        }
    }
}
