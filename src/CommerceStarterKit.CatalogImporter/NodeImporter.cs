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
    public class NodeImporter : ImporterBase
    {
        public NodeImporter(IContentRepository contentRepository, ReferenceConverter referenceConverter, IContentTypeRepository typeRepository, ILogger logger)
            : base(contentRepository, referenceConverter, typeRepository, logger)
        {

        }

        public void Import(List<Node> nodes)
        {
            // TODO: Iterate over list trying to find parents as we go, the
            // first implementation requires the list to be ordered so we can
            // find the parent as we go.
            foreach (Node node in nodes)
            {
                _log.Debug("Starting importing node '{0}' ({1})", node.name, node.code);
                // Find it's parent, we need it in order to add it
                var parent = GetNodeContent(node.parent);
                if(parent == null)
                {
                    throw new Exception("Cannot find parent content with code: " + node.parent);
                }

                // See if it exist first
                var content = GetNodeContent(node.code);
                if(content != null)
                {
                    // Existing content
                    _log.Debug("Starting importing EXISTING node '{0}' ({1})", node.name, node.code);
                    content = (NodeContent)content.CreateWritableClone();
                    var publishAndClearAction = SaveAction.Publish.SetExtendedActionFlag(ExtendedSaveAction.ClearVersions);
                    Update(content, node, publishAndClearAction);
                }
                else
                {
                    // New content
                    _log.Debug("Starting importing NEW node '{0}' ({1})", node.name, node.code);

                    // Get type of node
                    var nodeType = GetContentType(node.contentType);


                    CreateNew(parent.ContentLink, node, nodeType);

                }
            }
        }

        private NodeContent GetNodeContent(string code)
        {
            var contentLink = _referenceConverter.GetContentLink(code, CatalogContentType.CatalogNode);
            if (ContentReference.IsNullOrEmpty(contentLink) == false)
            {
                var content = _contentRepository.Get<NodeContent>(contentLink);
                return content;
            }
            return null;
        }

        public ContentReference CreateNew(ContentReference parentNodeLink, Node node, ContentType nodeType)
        {
            //Create a new instance of CatalogContentTypeSample that will be a child to the specified parentNode.
            var nodeContent = _contentRepository.GetDefault<NodeContent>(parentNodeLink, nodeType.ID);

            // Set required properties
            nodeContent.Code = node.code;
            return Update(nodeContent, node, SaveAction.Publish);
        }


        public ContentReference Update(NodeContent nodeContent, Node node, SaveAction saveAction)
        {
            if (string.IsNullOrEmpty(node.urlSegment) == false)
            {
                nodeContent.RouteSegment = node.urlSegment;
            }
            else
            {
                nodeContent.RouteSegment = node.code.ToLowerInvariant();
            }

            //Set the Name
            nodeContent.Name = node.name;
            nodeContent.DisplayName = node.name;

            // Override with language
            var displayName = GetPropertyValue(node, RootCatalog.DefaultLanguage, "DisplayName");
            if (string.IsNullOrEmpty(displayName) == false)
            {
                nodeContent.DisplayName = displayName;
            }

            //Publish the new content and return its ContentReference.
            return _contentRepository.Save(nodeContent, saveAction, AccessLevel.NoAccess);

        }

        private string GetPropertyValue(Node node, string language, string propertyName)
        {
            var property = node.properties.FirstOrDefault(
                n =>
                    n.language.Equals(language, StringComparison.InvariantCultureIgnoreCase) &&
                    n.name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
            if (property != null && string.IsNullOrEmpty(property.value) == false)
            {
                return property.value;
            }

            return null;
        }
    }
}