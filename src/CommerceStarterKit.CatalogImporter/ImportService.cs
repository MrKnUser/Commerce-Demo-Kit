using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CommerceStarterKit.CatalogImporter.DTO;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.DataAbstraction;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Catalog.Objects;
using Newtonsoft.Json;

namespace CommerceStarterKit.CatalogImporter
{
    [ServiceConfiguration]
    public class ImportService
    {
        private readonly IContentRepository _contentRepository;
        private readonly ReferenceConverter _referenceConverter;
        private readonly IContentTypeRepository _typeRepository;
        private readonly ILogger _log;

        public ImportService(IContentRepository contentRepository, ReferenceConverter referenceConverter, IContentTypeRepository typeRepository, ILogger logger)
        {
            _contentRepository = contentRepository;
            _referenceConverter = referenceConverter;
            _typeRepository = typeRepository;
            _log = logger;
        }

        public void ImportCatalogFromJsonFile(string appDataRelativePath)
        {
            _log.Debug("Starting import");
            string path = HttpContext.Current.Server.MapPath("~/App_Data/" + appDataRelativePath);
            CatalogRoot root = ReadDataFromFile(path);

            // Verify content
            var catalogContent = GetCatalogFromName(root.catalog);
            if (catalogContent == null) 
                throw new ArgumentNullException("Cannot find catalog with name: " + root.catalog);

            _log.Debug("Importing to catalog {0}", catalogContent.Name);

            // Import Nodes
            ImportNodes(root, catalogContent);

            // Import Entries
            ImportEntries(root, catalogContent);
        }

        protected void ImportNodes(CatalogRoot root, CatalogContent catalogContent)
        {
            NodeImporter nodeImporter = ServiceLocator.Current.GetInstance<NodeImporter>();

            var defaultContentType = _typeRepository.Load(root.defaults.defaultNodeType);
            nodeImporter.DefaultContentType = defaultContentType;
            _log.Debug("Default content type: {0}", defaultContentType != null ? defaultContentType.FullName : "None");

            nodeImporter.RootCatalog = catalogContent;
            nodeImporter.Defaults = root.defaults;

            nodeImporter.Import(root.nodes);
        }    
        
        protected void ImportEntries(CatalogRoot root, CatalogContent catalogContent)
        {
            EntryImporter entryImporter = ServiceLocator.Current.GetInstance<EntryImporter>();

            var defaultContentType = _typeRepository.Load(root.defaults.defaultEntryType);
            entryImporter.DefaultContentType = defaultContentType;
            _log.Debug("Default content type: {0}", defaultContentType != null ? defaultContentType.FullName : "None");

            entryImporter.RootCatalog = catalogContent;
            entryImporter.Defaults = root.defaults;

            entryImporter.Import(root.entries);
        }

        private CatalogContent GetCatalogFromName(string catalog)
        {
            var catalogs = _contentRepository.GetChildren<CatalogContent>(_referenceConverter.GetRootLink());
            return catalogs.FirstOrDefault(c => c.Name.Equals(catalog, StringComparison.InvariantCultureIgnoreCase));
        }

        private CatalogRoot ReadDataFromFile(string path)
        {
            if (File.Exists(path))
            {
                var catalog = JsonConvert.DeserializeObject<CatalogRoot>(File.ReadAllText(path));
                return catalog;
            }
            else
            {
                throw new FileNotFoundException("Cannot load catalog data from " + path);
            }

        }
    }
}
