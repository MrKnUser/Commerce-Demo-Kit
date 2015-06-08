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
    public class HeadingElementSelectionFactory : GenericSelectionFactory
    {
        public override IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return GetSelectionFromDictionary(
                new Dictionary<string, string>()
                {
                    {"Heading 1", "h1"}, 
                    {"Heading 2", "h2"}, 
                    {"Heading 3", "h3"}, 
                    {"Heading 4", "h4"}, 
                    {"Heading 5", "h5"}
                });
        }
    }
}
