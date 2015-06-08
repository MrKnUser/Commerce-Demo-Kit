/*
Commerce Starter Kit for EPiServer

All rights reserved. See LICENSE.txt in project root.

Copyright (C) 2013-2014 Oxx AS
Copyright (C) 2013-2014 BV Network AS

*/

using System.Collections.Generic;
using EPiServer.Shell.ObjectEditing;

namespace OxxCommerceStarterKit.Web.EditorDescriptors.SelectionFactories
{
    public class ColorSchemeSelectionFactory : GenericSelectionFactory
    {
        public override IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return GetSelectionFromDictionary(
                new Dictionary<string, string>()
                {
                    {"Default", ""}, 
                    {"Purple Red", "color-scheme2"}, 
                    {"Cyan Brown", "color-scheme3"}, 
                    {"Green Brown", "color-scheme4"}
                });
        }
    }
}
