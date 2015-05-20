using System;
using System.Collections.Generic;
using System.Linq;
using CommerceStarterKit.CatalogImporter.DTO;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Provider;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAccess;
using EPiServer.Logging;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Catalog;

namespace CommerceStarterKit.CatalogImporter
{
    [ServiceConfiguration]
    public class EntryImporter : ImporterBase
    {
        public EntryImporter(IContentRepository contentRepository, ReferenceConverter referenceConverter, IContentTypeRepository typeRepository, ILogger logger)
            : base(contentRepository, referenceConverter, typeRepository, logger)
        {

        }

        public void Import(List<Entry> entries)
        {
            // TODO: Iterate over list trying to find parents as we go, the
            // first implementation requires the list to be ordered so we can
            // find the parent as we go.
            foreach (Entry entry in entries)
            {
                _log.Debug("Starting importing entry '{0}' ({1})", entry.name, entry.code);
                
                // Find it's parent, we need it in order to add it
                var parent = GetNodeContent(entry.parent);

                if(parent == null)
                {
                    throw new Exception("Cannot find parent content with code: " + entry.parent);
                }

                // See if it exist first
                var content = GetEntryContent(entry.code);
                if(content != null)
                {
                    // Existing content
                    _log.Debug("Starting importing EXISTING entry '{0}' ({1})", entry.name, entry.code);
                    content = (EntryContentBase)content.CreateWritableClone();
                    var publishAndClearAction = SaveAction.Publish.SetExtendedActionFlag(ExtendedSaveAction.ClearVersions);
                    Update(content, entry, publishAndClearAction);
                }
                else
                {
                    var parentNode = GetNodeContent(entry.parent);

                    // Get type of node
                    var entryType = GetContentType(entry.contentType);

                    // New content
                    _log.Debug("Starting importing NEW entry '{0}' ({1}) of type {2}", entry.name, entry.code, entryType.Name);

                    CreateNew(parentNode.ContentLink, entry, entryType);
                }
            }
        }


        public ContentReference CreateNew(ContentReference parentNodeLink, Entry entry, ContentType nodeType)
        {
            //Create a new instance of CatalogContentTypeSample that will be a child to the specified parentNode.
            var content = _contentRepository.GetDefault<EntryContentBase>(parentNodeLink, nodeType.ID);

            // Set required properties
            content.Code = entry.code;
            return Update(content, entry, SaveAction.Publish);
        }


        public ContentReference Update(EntryContentBase content, Entry entry, SaveAction saveAction)
        {
            if (string.IsNullOrEmpty(entry.urlSegment) == false)
            {
                content.RouteSegment = entry.urlSegment;
            }
            else
            {
                content.RouteSegment = entry.code.ToLowerInvariant();
            }

            //Set the Name
            content.Name = entry.name;
            content.DisplayName = entry.name;

            // Override with language
            var displayName = GetPropertyValue(entry.properties, RootCatalog.DefaultLanguage, "DisplayName");
            if (string.IsNullOrEmpty(displayName) == false)
            {
                content.DisplayName = displayName;
            }

            //Publish the new content and return its ContentReference.
            return _contentRepository.Save(content, saveAction, AccessLevel.NoAccess);

        }

    }
}