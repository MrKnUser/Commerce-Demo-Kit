using EPiServer;
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

        public ImporterBase(IContentRepository contentRepository, ReferenceConverter referenceConverter, IContentTypeRepository typeRepository, ILogger logger)
        {
            _contentRepository = contentRepository;
            _referenceConverter = referenceConverter;
            _typeRepository = typeRepository;
            _log = logger;
        }

    }
}