﻿using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Commerce.Marketing;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Orders;

namespace OxxCommerceStarterKit.Web.Promotion
{
    public class BuyXFromCategoryGetProductForFreeProcessor : PromotionProcessorBase<BuyXFromCategoryGetProductForFree>
    {
        private readonly IContentLoader _contentLoader;

        public BuyXFromCategoryGetProductForFreeProcessor(IContentLoader contentLoader)
        {           
            _contentLoader = contentLoader;
        }

        public override IPromotionResult Evaluate(IOrderGroup orderGroup, BuyXFromCategoryGetProductForFree promotion)
        {
            IEnumerable<ILineItem> items = GetLineItemsInOrder(orderGroup);

            var lineItemCategories = items.Select(i => new {i.Quantity, NodesForEntry = GetNodesForEntry(i.Code)});

            decimal numberOfItemsInPromotionCategory = 0;

            NodeContent category = _contentLoader.Get<NodeContent>(promotion.Category);

            foreach (var lineItemCategory in lineItemCategories)
            {
                if (lineItemCategory.NodesForEntry.Contains(category.Code))
                {
                    numberOfItemsInPromotionCategory += lineItemCategory.Quantity;
                }
            }
                     
            FulfillmentStatus fulfillment = this.GetFulfillment(numberOfItemsInPromotionCategory, promotion.Threshold);

            return PromotionResult(orderGroup, promotion, fulfillment);
        }

        private IPromotionResult PromotionResult(IOrderGroup orderGroup, BuyXFromCategoryGetProductForFree promotion,
            FulfillmentStatus fulfillment)
        {
            BuyXFromCategoryGetProductForFreeResult result = null;

            switch (fulfillment)
            {
                case FulfillmentStatus.NotFulfilled:
                    result = new BuyXFromCategoryGetProductForFreeResult(fulfillment,
                        "The promotion is not fulfilled.");
                    break;

                case FulfillmentStatus.PartiallyFulfilled:
                    result = new BuyXFromCategoryGetProductForFreeResult(fulfillment,
                        "The promotion is somewhat fulfilled.");
                    break;
                case FulfillmentStatus.Fulfilled:
                {
                    VariationContent content = _contentLoader.Get<VariationContent>(promotion.FreeProduct);
                    result = new BuyXFromCategoryGetProductForFreeResult(fulfillment,
                        string.Format("The promotion is fulfilled and it has been applied to item {0}", content.Name),
                        content, orderGroup as OrderGroup);
                    break;
                }
            }
            return result;
        }

        private IEnumerable<ILineItem> GetLineItemsInOrder(IOrderGroup orderGroup)
        {
            return orderGroup.Forms.SelectMany(f => f.Shipments).SelectMany(i => i.LineItems);
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
                catch
                {
                    //
                }
            }
        }
    }

  
       


    
}
