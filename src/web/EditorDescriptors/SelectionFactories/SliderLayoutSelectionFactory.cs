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
    public class SliderLayoutSelectionFactory : GenericSelectionFactory
    {
        public override IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return GetSelectionFromDictionary(
                new Dictionary<string, string>()
                {
                    {"Boxed", "boxed"}, // The default layout of slider, size of the slider does not exceed the specified width and height in slider options.
                    {"Full Width", "fullwidth"}, // Forces the slider to adapt width to the browser width.
                    {"Full screen", "fullscreen"}, // Forces the slider to adapt width and height to the browser window dimension.
                    {"Fill Width", "fillwidth"}, // Enables the slider to adapt width to its parent element.
                    {"Auto fill", "autofill"}, // Enables the slider to adapt width and height to its parent element.
                    {"Partial View", "partialview"} //  It's like the boxed layout but nearby slides are always visible.
                });
        }
    }
}
