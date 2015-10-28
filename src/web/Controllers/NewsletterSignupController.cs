using System.Web.Mvc;
using EPiServer.Web.Mvc;
using OxxCommerceStarterKit.Web.Models.Blocks;

namespace OxxCommerceStarterKit.Web.Controllers
{
    public class NewsletterSignupBlockController : BlockController<NewsletterSignupBlock>
    {
        public override ActionResult Index(NewsletterSignupBlock currentBlock)
        {
            return PartialView(currentBlock);
        }
    }
}
