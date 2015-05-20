using System;
using System.Collections.Generic;
using System.Linq;
using CommerceStarterKit.CatalogImporter.DTO;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Logging;
using Mediachase.Commerce.Catalog;

namespace CommerceStarterKit.CatalogImporter
{
    public class ImporterBase
    {
        protected readonly IContentRepository _contentRepository;
        protected readonly ReferenceConverter _referenceConverter;
        protected readonly IContentTypeRepository _typeRepository;
        protected readonly ILogger _log;

        public virtual ContentType DefaultContentType { get; set; }
        public virtual CatalogContent RootCatalog { get; set; }
        public virtual Defaults Defaults { get; set; }

        public ImporterBase(IContentRepository contentRepository, ReferenceConverter referenceConverter, IContentTypeRepository typeRepository, ILogger logger)
        {
            _contentRepository = contentRepository;
            _referenceConverter = referenceConverter;
            _typeRepository = typeRepository;
            _log = logger;
        }

        protected virtual ContentType GetContentType(string contentTypeName)
        {
            ContentType contentType = null;
            if (string.IsNullOrEmpty(contentTypeName) == false)
            {
                contentType = _typeRepository.Load(contentTypeName);
            }
            if (contentType == null)
            {
                contentType = DefaultContentType;
            }

            if (contentType == null)
            {
                throw new Exception("Cannot load content type " + contentTypeName);
            }
            return contentType;
        }

        protected NodeContent GetNodeContent(string code)
        {
            return GetContent<NodeContent>(code, CatalogContentType.CatalogNode);
        }

        protected EntryContentBase GetEntryContent(string code)
        {
            return GetContent<EntryContentBase>(code, CatalogContentType.CatalogEntry);
        }

        protected T GetContent<T>(string code, CatalogContentType contentType) where T : IContentData
        {
            var contentLink = _referenceConverter.GetContentLink(code, contentType);
            if (ContentReference.IsNullOrEmpty(contentLink) == false)
            {
                var content = _contentRepository.Get<T>(contentLink);
                return content;
            }
            return default(T);
        }

        protected string GetPropertyValue(List<Property> properties, string language, string propertyName)
        {
            if (properties == null)
                return null;

            if (propertyName == null) throw new ArgumentNullException("propertyName");

            Property property;
            if (string.IsNullOrEmpty(language) == false)
            {
                property = properties.FirstOrDefault(
                    n =>
                        (n.language == null || n.language.Equals(language, StringComparison.InvariantCultureIgnoreCase))
                        &&
                        n.name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                property = properties.FirstOrDefault(n => n.name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
            }

            if (property != null && string.IsNullOrEmpty(property.value) == false)
            {
                return property.value;
            }


            return null;
        }

    }
}