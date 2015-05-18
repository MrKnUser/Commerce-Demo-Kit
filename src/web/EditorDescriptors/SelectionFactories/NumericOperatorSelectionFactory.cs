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
    public class NumericOperatorSelectionFactory : GenericSelectionFactory
    {
        public class OperatorNames
        {
            public const string Equal = "Equal";
            public const string GreaterThan = "GreaterThan";
            public const string LessThan = "LessThan";

        }

        public override IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return GetSelectionFromDictionary(
                new Dictionary<string, string>()
                {
                    {"Equals", OperatorNames.Equal}, 
                    {"Greater Than", OperatorNames.GreaterThan}, 
                    {"Less Than", OperatorNames.LessThan}
                });
        }
    }
}
