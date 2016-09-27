using System;
using EPiServer.Core;
using EPiServer.PlugIn;
using EPiServer.Scheduler;
using EPiServer;
using EPiServer.ServiceLocation;
using EPiServer.Commerce.Marketing;
using System.Collections.Generic;
using EPiServer.Security;

namespace OxxCommerceStarterKit.Web.Jobs
{
    [ScheduledPlugIn(DisplayName = "Clear Promotions")]
    public class ClearPromotionsScheduledJob : ScheduledJobBase
    {
        private bool _stopSignaled;

        public ClearPromotionsScheduledJob()
        {
            IsStoppable = true;
        }

        /// <summary>
        /// Called when a user clicks on Stop for a manually started job, or when ASP.NET shuts down.
        /// </summary>
        public override void Stop()
        {
            _stopSignaled = true;
        }

        /// <summary>
        /// Called when a scheduled job executes
        /// </summary>
        /// <returns>A status message to be stored in the database log and visible from admin mode</returns>
        public override string Execute()
        {
            //Call OnStatusChanged to periodically notify progress of job for manually started jobs
            OnStatusChanged(String.Format("Starting execution of {0}", this.GetType()));

            //Add implementation
            IContentLoader loader = ServiceLocator.Current.GetInstance<IContentLoader>();
            IContentRepository repository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var campaigns = loader.GetChildren<SalesCampaign>(SalesCampaignFolder.CampaignRoot.ToReferenceWithoutVersion());

            foreach (SalesCampaign campaign in campaigns)
            {
                IEnumerable<PromotionData> promotions = loader.GetChildren<PromotionData>(campaign.ContentLink);
                foreach (PromotionData promotionData in promotions)
                {
                    try
                    {
                        repository.Delete(promotionData.ContentLink, true, AccessLevel.Administer);
                    }
                    catch (Exception ex)
                    { }
                }

                try
                {
                    repository.Delete(campaign.ContentLink, true, AccessLevel.Administer);
                }
                catch (Exception ex)
                { }
            }

            //For long running jobs periodically check if stop is signaled and if so stop execution
            if (_stopSignaled)
            {
                return "Stop of job was called";
            }

            return "Change to message that describes outcome of execution";
        }
    }
}
