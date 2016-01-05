using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Framework.DataAnnotations;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using OxxCommerceStarterKit.Core.Objects;
using OxxCommerceStarterKit.Core.Services;
using OxxCommerceStarterKit.Web.Business.BulkOrdering.Interfaces;
using OxxCommerceStarterKit.Web.Models.PageTypes;
using OxxCommerceStarterKit.Web.Models.ViewModels;
using EPiServer.Core;
using OxxCommerceStarterKit.Web.Helpers;

namespace OxxCommerceStarterKit.Web.Controllers
{

    [TemplateDescriptor(Inherited = true)]
    public class BulkUploadController : PageController<BulkUploadModelPage>
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly IFileValidator _fileValidator;
        private readonly ICartService _cartService;
        private readonly UrlResolver _urlResolver;
        readonly IContentLoader _contentLoader;

        public BulkUploadController(IServiceLocator serviceLocator, IFileValidator fileValidator,
            ICartService cartService, UrlResolver urlResolver, IContentLoader contentLoader)
        {
            _serviceLocator = serviceLocator;
            _fileValidator = fileValidator;
            _cartService = cartService;
            _urlResolver = urlResolver;
            _contentLoader = contentLoader;
        }

        /// <summary>
        /// The main view for the cart.
        /// </summary>
        public ViewResult Index(BulkUploadModelPage currentPage)
        {
            BulkUploadViewModel model = new BulkUploadViewModel(currentPage);

            return View(model);
        }

        [HttpPost]
        public ViewResult UploadFile(BulkUploadModelPage currentPage, HttpPostedFileBase fileUpload)
        {
            BulkUploadViewModel model = new BulkUploadViewModel(currentPage);

            ValidateFile(currentPage, fileUpload);
            if (ModelState.IsValid)
            {
                ParseFile(currentPage, fileUpload, model);
            }

            return View("Index", model);
        }

        [HttpPost]
        public ActionResult MoveToCart(BulkUploadModelPage currentPage, BulkUploadViewModel model)
        {
            //Add items to the cart
            foreach (var bulkUploadFileItem in model.ParsedUploadItems)
            {
                if (bulkUploadFileItem.CouldLookupProduct)
                {
                    var addLineItem = new LineItem()
                    {
                        Quantity = bulkUploadFileItem.Quantity,
                        Code = bulkUploadFileItem.ProductCode
                    };
                    _cartService.AddToCart(addLineItem);
                }
            }

            HomePage startPage = _contentLoader.Get<HomePage>(ContentReference.StartPage);
            return Redirect(UrlHelpers.GetExternalUrl(startPage.Settings.CartPage));
        }

        private void ValidateFile(BulkUploadModelPage currentPage, HttpPostedFileBase file)
        {
            if (!_fileValidator.FileIsValid(file))
            {
                var errorMessage = currentPage.InvalidFile; // "File type is not valid. Allowed file types are csv, xls and xlsx";
                ModelState.AddModelError(errorMessage, errorMessage);
            }
        }

        private void ParseFile(BulkUploadModelPage currentPage, HttpPostedFileBase file, BulkUploadViewModel model)
        {
            try
            {
                var fileExtension = Path.GetExtension(Path.GetExtension(file.FileName));
                var parserKey = fileExtension + "-parser";
                var parser = _serviceLocator.GetInstance<IFileParser>(parserKey);

                if (parser != null)
                {
                    model.ParsedUploadItems = parser.ParseFile(file);

                    if (!string.IsNullOrEmpty(parser.ParseError))
                    {
                        ModelState.AddModelError(parser.ParseError, parser.ParseError);
                    }
                }
                else
                {
                    string errorMessage = currentPage.NoParserFound;
                    ModelState.AddModelError(errorMessage, errorMessage);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = currentPage.ParseFileError + ex.ToString();
                ModelState.AddModelError(errorMessage, errorMessage);
            }
        }
    }
}
