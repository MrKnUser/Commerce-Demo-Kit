using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using OxxCommerceStarterKit.Web.Models.Blocks;
using OxxCommerceStarterKit.Web.Models.PageTypes;
using OxxCommerceStarterKit.Web.Models.ViewModels;

namespace OxxCommerceStarterKit.Web.Controllers
{
    [TemplateDescriptor(
        Inherited = true,
        TemplateTypeCategory = TemplateTypeCategories.MvcController,
        //Required as controllers for blocks are registered as MvcPartialController by default
        Tags = new[] {RenderingTags.Preview, RenderingTags.Edit},
        AvailableWithoutTag = false)]
    [VisitorGroupImpersonation]
    public class SliderPreviewController : ActionControllerBase, IRenderTemplate<SliderBlock> // Note use of specific IRendetTemplate to target correct block type
    {
        private readonly IContentLoader _contentLoader;
        private readonly TemplateResolver _templateResolver;
        private readonly DisplayOptions _displayOptions;

        public SliderPreviewController(IContentLoader contentLoader, TemplateResolver templateResolver, DisplayOptions displayOptions)
        {
            _contentLoader = contentLoader;
            _templateResolver = templateResolver;
            _displayOptions = displayOptions;
        }

        public ActionResult Index(IContent currentContent)
        {
            //As the layout requires a page for title etc we "borrow" the start page
            var startPage = _contentLoader.Get<HomePage>(ContentReference.StartPage);

            var model = new SliderPreviewModel(startPage, currentContent);
            //var contentArea = new ContentArea();
            //contentArea.Items.Add(new ContentAreaItem
            //{
            //    ContentLink = currentContent.ContentLink
            //});
            var contentArea = ((SliderBlock) currentContent).SliderContent;
            model.SliderContent = contentArea;

            return View(model);
        }

    }
}