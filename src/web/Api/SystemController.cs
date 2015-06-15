using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Castle.Core.Logging;
using CommerceStarterKit.CatalogImporter;
using EPiServer;
using EPiServer.Core;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using ILogger = EPiServer.Logging.ILogger;

namespace OxxCommerceStarterKit.Web.Api
{
    [System.Web.Http.Authorize(Roles = "WebAdmins")]
    public class ImportController : BaseApiController
    {
        [System.Web.Http.HttpGet]
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

    [System.Web.Http.Authorize(Roles = "WebAdmins")]
    public class ContentController : BaseApiController
    {
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            var log = EPiServer.Logging.LogManager.GetLogger();
            var contentRepo = ServiceLocator.Current.GetInstance<IContentRepository>();
            var reference = new ContentReference(id);
            var content = contentRepo.Get<IContent>(reference);
            if(content == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            contentRepo.Delete(reference, true);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }


}


