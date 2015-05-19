using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Castle.Core.Logging;
using CommerceStarterKit.CatalogImporter;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using ILogger = EPiServer.Logging.ILogger;

namespace OxxCommerceStarterKit.Web.Api
{
    [Authorize(Roles = "WebAdmins")]
    public class ImportController : BaseApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var log = EPiServer.Logging.LogManager.GetLogger();
            var importService = ServiceLocator.Current.GetInstance<CommerceStarterKit.CatalogImporter.ImportService>();

            try
            {
                importService.ImportCatalogFromJsonFile("catalog.json");
            }
            catch (Exception exception)
            {
                log.Error("Failed to import catalog.", exception);
                throw;
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
