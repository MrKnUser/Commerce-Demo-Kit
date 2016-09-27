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
            var spendGetDiscount = e.Content as SpendAmountGetOrderDiscount;
            if (spendGetDiscount == null)
            {
                return;
            }
            spendGetDiscount.Condition.PartiallyFulfilledThreshold = 0.75m;
        }

        private static void UpdateThresholds()
        {
            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var campaigns = contentLoader.GetChildren<SalesCampaign>(SalesCampaignFolder.CampaignRoot);
            foreach (var salesCampaign in campaigns)
            {
                var freeShippingPromotions = contentLoader.GetChildren<SpendAmountGetOrderDiscount>(salesCampaign.ContentLink);
                foreach (var spendAmountGetFreeShipping in freeShippingPromotions)
                {
                    var clone = spendAmountGetFreeShipping.CreateWritableClone() as SpendAmountGetOrderDiscount;
                    clone.Condition.PartiallyFulfilledThreshold = 0.75m;
                    contentRepository.Save(clone, SaveAction.Publish, AccessLevel.NoAccess);
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