using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Marketing;
using EPiServer.ServiceLocation;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Catalog.Objects;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Pricing;
using OxxCommerceStarterKit.Core;

namespace OxxCommerceStarterKit.Web.Promotion
{

    public class BuyXFromCategoryGetProductForFreeResult : IPromotionResult
    {
        private readonly VariationContent _variation;
        private OrderGroup _orderGroup;

        public BuyXFromCategoryGetProductForFreeResult(FulfillmentStatus status, string description) : this(status, description, null, null)
        {
        }

        public BuyXFromCategoryGetProductForFreeResult(FulfillmentStatus status, string description, VariationContent variation, OrderGroup group)
        {
            this.Status = status;
            this.Description = description;
            this._variation = variation;
            this._orderGroup = group;
        }

        public IEnumerable<PromotionInformation> ApplyReward()
        {
            if (this._variation != null)
            {
                if (!_orderGroup.OrderForms[0].LineItems.Any(i => i.Code == _variation.Code))
                {
                    var entry = CatalogContext.Current.GetCatalogEntry(_variation.Code);
                    LineItem item = CreateLineItem(entry, _orderGroup);

                    item.PlacedPrice = 0;

                    _orderGroup.OrderForms[0].LineItems.Add(item);

                }
                
                PromotionInformation iteratorVariable1 = new PromotionInformation
                {
                    Description = this.Description,
                    SavedAmount = 100,
                    ContentLink = this._variation.ContentLink,
                    IsActive = true
                };
                yield return iteratorVariable1;
            }
            else
            {
                PromotionInformation iteratorVariable0 = new PromotionInformation
                {
                    Description = this.Description
                };
                yield return iteratorVariable0;
            }
        }

        private LineItem CreateLineItem(Entry entry, IOrderGroup g)
        {
            var priceService = ServiceLocator.Current.GetInstance<IPriceService>();

            LineItem item = new LineItem();
            if (entry.ParentEntry != null)
            {
                item.DisplayName = string.Format("{0}: {1}", entry.ParentEntry.Name, entry.Name);
                item.ParentCatalogEntryId = entry.ParentEntry.ID;
            }
            else
            {
                item.DisplayName = entry.Name;                
            }
            item[Constants.Metadata.LineItem.ImageUrl] =
                "/globalassets/catalogs/photo/accessories/memory-card/sandisk-extreme-pro/824140.jpg?preset=listsmall";
            MarketId marketId = g.Market.MarketId.Value;
            IPriceValue value2 = priceService.GetDefaultPrice(marketId, FrameworkContext.Current.CurrentDateTime, new CatalogKey(entry), g.Currency);
            item.Code = entry.ID;

            item.Quantity = 1;
           

            if (value2 != null)
            {
                item.ListPrice = value2.UnitPrice.Amount;
                item.PlacedPrice = value2.UnitPrice.Amount;
                item.ExtendedPrice = value2.UnitPrice.Amount;
            }
            else
            {
                item.ListPrice = item.PlacedPrice;
            }
            
            return item;
        }

        public ILineItem AffectedItem { get; private set; }

        public string Description { get; private set; }

        public FulfillmentStatus Status { get; private set; }   
    }
}

