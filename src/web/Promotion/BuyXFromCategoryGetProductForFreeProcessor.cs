using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OxxCommerceStarterKit.Web.Promotion
{
    public class BuyXFromCategoryGetProductForFreeProcessor : EntryPromotionProcessorBase<BuyXFromCategoryGetProductForFree>
    {
        private readonly IContentLoader _contentLoader;
        private readonly ReferenceConverter _referenceConverter;

        public BuyXFromCategoryGetProductForFreeProcessor(IContentLoader contentLoader, ReferenceConverter referenceConverter)
        {
            _contentLoader = contentLoader;
            _referenceConverter = referenceConverter;
        }

        protected override PromotionItems GetPromotionItems(BuyXFromCategoryGetProductForFree promotionData)
        {
            return
                new PromotionItems(
                    promotionData,
                    new CatalogItemSelection(new [] { promotionData.Category }, CatalogItemSelectionType.Specific, true),
                    new CatalogItemSelection(new[] { promotionData.Category }, CatalogItemSelectionType.Specific, true));
        }

        protected override RewardDescription Evaluate(BuyXFromCategoryGetProductForFree promotionData, PromotionProcessorContext context)
        {
            var items = GetLineItemsInOrder(context.OrderForm);
            var lineItemCategories = items.Select(i => new
            {
                i.Quantity,
                NodesForEntry = GetNodesForEntry(i.Code),
                i.Code,
                LineItem = i
            });

            var category = _contentLoader.Get<NodeContent>(promotionData.Category);
            var applicableLineItems = lineItemCategories.Where(lineItemCategory => lineItemCategory.NodesForEntry.Contains(category.Code));
            var numberOfItemsInPromotionCategory = applicableLineItems.Sum(lineItemCategory => lineItemCategory.Quantity);
            var redemptions = GetRedemptions(promotionData, context, applicableLineItems.Select(x => x.Code));
            var fulfillment = GetFulfillment(numberOfItemsInPromotionCategory, promotionData.Threshold);
            return RewardDescription.CreateFreeItemReward(fulfillment, redemptions, promotionData, "Got something for free");
        }

        protected override bool CanBeFulfilled(BuyXFromCategoryGetProductForFree promotionData, PromotionProcessorContext context)
        {
            var items = GetLineItemsInOrder(context.OrderForm);
            var lineItemCategories = items.Select(i => new
            {
                i.Quantity,
                NodesForEntry = GetNodesForEntry(i.Code),
                i.Code,
                LineItem = i
            });

            var category = _contentLoader.Get<NodeContent>(promotionData.Category);
            var applicableLineItems = lineItemCategories.Where(lineItemCategory => lineItemCategory.NodesForEntry.Contains(category.Code));
            var numberOfItemsInPromotionCategory = applicableLineItems.Sum(lineItemCategory => lineItemCategory.Quantity);
            var fulfillment = GetFulfillment(numberOfItemsInPromotionCategory, promotionData.Threshold);
            return fulfillment == FulfillmentStatus.Fulfilled;
        }

        protected AffectedEntries GetAffectedEntries(BuyXFromCategoryGetProductForFree promotionData, PromotionProcessorContext context, IEnumerable<string> applicableCodes)
        {
            var requiredQuantity = promotionData.Threshold;
            var affectedEntries = context.EntryPrices.ExtractEntries(applicableCodes, requiredQuantity);

            if (affectedEntries == null)
            {
                return null;
            }
            return affectedEntries.SetDiscountRange(requiredQuantity - 1, 1);
        }

        private IEnumerable<RedemptionDescription> GetRedemptions(BuyXFromCategoryGetProductForFree promotionData, PromotionProcessorContext context, IEnumerable<string> applicableCodes)
        {
            var redemptions = new List<RedemptionDescription>();
            var maxRedemptions = GetMaxRedemptions(promotionData.RedemptionLimits);
            for (int i = 0; i < maxRedemptions; i++)
            {
                var affectedEntries = GetAffectedEntries(promotionData, context, applicableCodes);
                if (affectedEntries == null)
                {
                    break;
                }
                redemptions.Add(CreateRedemptionDescription(affectedEntries));
            }

            return redemptions;
        }

        private IEnumerable<ILineItem> GetLineItemsInOrder(IOrderForm orderForm)
        {
            return orderForm.Shipments.SelectMany(i => i.LineItems);
        }


        private FulfillmentStatus GetFulfillment(decimal qualifyingProducts, int threshold)
        {
            if (qualifyingProducts >= threshold)
            {
                return FulfillmentStatus.Fulfilled;
            }
            if (qualifyingProducts > 0)
            {
                return FulfillmentStatus.PartiallyFulfilled;
            }
            return FulfillmentStatus.NotFulfilled;
        }

        private IEnumerable<string> GetNodesForEntry(string entryCode)
        {
            var referenceConverter = ServiceLocator.Current.GetInstance<ReferenceConverter>();
            var nodeId = _contentLoader.Get<VariationContent>(referenceConverter.GetContentLink(entryCode)).ParentLink;
            var node = _contentLoader.Get<NodeContent>(nodeId);
            return ParentNodes(node);
        }

        private IEnumerable<string> ParentNodes(NodeContent currentNode)
        {
            List<string> resultList = new List<string>();
            ParentNodesRecurcive(currentNode, resultList);
            return resultList;
        }

        private void ParentNodesRecurcive(NodeContent currentNode, List<string> resultList)
        {
            var links = ServiceLocator.Current.GetInstance<ILinksRepository>();
            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            var nodeRelations = links.GetRelationsBySource<NodeRelation>(currentNode.ContentLink).Select(x => x.Target).ToList();
            nodeRelations.Add(currentNode.ParentLink);
            foreach (ContentReference reference in nodeRelations)
            {
                try
                {
                    var n = contentLoader.Get<NodeContent>(reference);
                    resultList.Add(n.Code);
                    ParentNodesRecurcive(n, resultList);
                }
                catch { }
            }
        }
    }
}
