using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Order;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Security;
using EPiServer.Web.Mvc;
using Mediachase.Commerce.Security;
using OxxCommerceStarterKit.Web.Models.Blocks;
using OxxCommerceStarterKit.Web.Services;

namespace OxxCommerceStarterKit.Web.Controllers
{
    [TemplateDescriptor(AvailableWithoutTag = true, Default = true, Inherited = false)]
    public class AlmostDiscountController : BlockController<AlmostDiscountBlock>
    {
        private readonly IPromotionEngine _promotionEngine;
        private readonly IOrderRepository _orderRepository;
        private const string Default = "Default";

        public AlmostDiscountController(IPromotionEngine promotionEngine,
            IOrderRepository orderRepository)
        {
            _promotionEngine = promotionEngine;
            _orderRepository = orderRepository;
        }

        public override ActionResult Index(AlmostDiscountBlock currentBlock)
        {
            return PartialView(CheckAlmostFulfilled());
        }

        private IEnumerable<RewardDescription> CheckAlmostFulfilled()
        {
            var cart = _orderRepository.Load<ICart>(PrincipalInfo.CurrentPrincipal.GetContactId(), Default)
                .FirstOrDefault();

            if (cart == null)
            {
                return new List<RewardDescription>();
            }

            return _promotionEngine.Run(cart, new PromotionEngineSettings

            {
                ApplyReward = false,
                RequestedStatuses = RequestFulfillmentStatus.PartiallyFulfilled
            });
        }
    }
}