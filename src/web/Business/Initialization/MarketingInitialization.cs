using EPiServer;
using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Marketing.Promotions;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Security;
using EPiServer.ServiceLocation;

namespace OxxCommerceStarterKit.Web.Business.Initialization
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Commerce.Initialization.InitializationModule))]
    public class MarketingInitialization : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            UpdateThresholds();
            var events = ServiceLocator.Current.GetInstance<IContentEvents>();
            events.SavingContent += Events_SavingContent;
        }

        private void Events_SavingContent(object sender, ContentEventArgs e)
        {
            if (e.Content is SpendAmountGetOrderDiscount)
            {
                ((SpendAmountGetOrderDiscount) e.Content).Condition.PartiallyFulfilledThreshold = 0.75m;
                return;
            }

            if (e.Content is SpendAmountGetGiftItems)
            {
                ((SpendAmountGetGiftItems) e.Content).Condition.PartiallyFulfilledThreshold = 0.75m;
                return;
            }

            if (e.Content is SpendAmountGetItemDiscount)
            {
                ((SpendAmountGetItemDiscount) e.Content).Condition.PartiallyFulfilledThreshold = 0.75m;
                return;
            }

            if (e.Content is SpendAmountGetShippingDiscount)
            {
                ((SpendAmountGetShippingDiscount) e.Content).Condition.PartiallyFulfilledThreshold = 0.75m;
                return;
            }

            if (e.Content is SpendAmountGetFreeShipping)
            {
                ((SpendAmountGetFreeShipping) e.Content).Condition.PartiallyFulfilledThreshold = 0.75m;
            }
        }

        private static void UpdateThresholds()
        {
            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var campaigns = contentLoader.GetChildren<SalesCampaign>(SalesCampaignFolder.CampaignRoot);
            foreach (var salesCampaign in campaigns)
            {
                var orderPromotions = contentLoader.GetChildren<OrderPromotion>(salesCampaign.ContentLink);
                foreach (var orderPromotion in orderPromotions)
                {
                    if (orderPromotion is SpendAmountGetOrderDiscount)
                    {
                        var clone = orderPromotion.CreateWritableClone() as SpendAmountGetOrderDiscount;
                        clone.Condition.PartiallyFulfilledThreshold = 0.75m;
                        contentRepository.Save(clone, SaveAction.Publish, AccessLevel.NoAccess);
                    }

                }

                var entryPromotions = contentLoader.GetChildren<EntryPromotion>(salesCampaign.ContentLink);
                foreach (var entryPromotion in entryPromotions)
                {
                    if (entryPromotion is SpendAmountGetGiftItems)
                    {
                        var clone = entryPromotion.CreateWritableClone() as SpendAmountGetGiftItems;
                        clone.Condition.PartiallyFulfilledThreshold = 0.75m;
                        contentRepository.Save(clone, SaveAction.Publish, AccessLevel.NoAccess);
                    }

                    if (entryPromotion is SpendAmountGetItemDiscount)
                    {
                        var clone = entryPromotion.CreateWritableClone() as SpendAmountGetItemDiscount;
                        clone.Condition.PartiallyFulfilledThreshold = 0.75m;
                        contentRepository.Save(clone, SaveAction.Publish, AccessLevel.NoAccess);
                    }
                }

                var shippingPromotions = contentLoader.GetChildren<ShippingPromotion>(salesCampaign.ContentLink);
                foreach (var shippingPromotion in shippingPromotions)
                {
                    if (shippingPromotion is SpendAmountGetShippingDiscount)
                    {
                        var clone = shippingPromotion.CreateWritableClone() as SpendAmountGetShippingDiscount;
                        clone.Condition.PartiallyFulfilledThreshold = 0.75m;
                        contentRepository.Save(clone, SaveAction.Publish, AccessLevel.NoAccess);
                    }
                
                    if (shippingPromotion is SpendAmountGetFreeShipping)
                    {
                        var clone = shippingPromotion.CreateWritableClone() as SpendAmountGetFreeShipping;
                        clone.Condition.PartiallyFulfilledThreshold = 0.75m;
                        contentRepository.Save(clone, SaveAction.Publish, AccessLevel.NoAccess);
                    }
                }
            }
        }

        public void Uninitialize(InitializationEngine context)
        {
            var events = ServiceLocator.Current.GetInstance<IContentEvents>();
            events.SavingContent -= Events_SavingContent;
        }
    }
}