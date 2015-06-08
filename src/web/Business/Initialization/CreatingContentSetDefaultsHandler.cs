/*
Commerce Starter Kit for EPiServer

All rights reserved. See LICENSE.txt in project root.

Copyright (C) 2013-2014 Oxx AS
Copyright (C) 2013-2014 BV Network AS

*/

using EPiServer;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using InitializationModule = EPiServer.Web.InitializationModule;

namespace OxxCommerceStarterKit.Web.Business.Initialization
{
    [InitializableModule]
    [ModuleDependency(typeof(InitializationModule))]
    public class CreatingContentSetDefaultsHandler : IInitializableModule
    {
        protected static ILogger _log = LogManager.GetLogger();

        /// <summary>
        /// Set this flag to temporary disable indexing while running long jobs. It is enabled by default.
        /// </summary>
        public static bool IndexingEnabled = true;

        public void Initialize(InitializationEngine context)
        {
			// hook up events for indexing
			IContentEvents events = ServiceLocator.Current.GetInstance<IContentEvents>();
            events.CreatingContent += EventsOnCreatingContent;
        }

        private void EventsOnCreatingContent(object sender, ContentEventArgs contentEventArgs)
        {
            if (IndexingEnabled == false)
                return;

            var content = contentEventArgs.Content;
            if (content != null)
            {
                SetPropertyValue(content, "Heading", content.Name);
                SetPropertyValue(content, "Title", content.Name);
                SetPropertyValue(content, "Header", content.Name);
                SetPropertyValue(content, "PageName", content.Name);
            }
            //if (contentEventArgs.Content is ISetDefaults)
            //{

            //}
        }

        private void SetPropertyValue(IContent content, string key, string value)
        {
            if (content.Property.Contains(key))
            {
                // Strip away anything before ":"
                int colonPos = value.IndexOf(":");
                if(colonPos > 0)
                {
                    value = value.Substring(colonPos + 1).Trim();
                }

                content.Property[key].Value = value;
            }
        }

        public void Preload(string[] parameters) { }

        public void Uninitialize(InitializationEngine context)
        {
            //Add uninitialization logic
        }
    }
}
