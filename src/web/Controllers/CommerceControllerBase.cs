﻿/*
Commerce Starter Kit for EPiServer

All rights reserved. See LICENSE.txt in project root.

Copyright (C) 2013-2014 Oxx AS
Copyright (C) 2013-2014 BV Network AS

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EPiServer;
using EPiServer.Commerce.Catalog;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Commerce.SpecializedProperties;
using EPiServer.Core;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;

using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Core;
using Mediachase.Commerce.Inventory;
using OxxCommerceStarterKit.Core;
using OxxCommerceStarterKit.Core.Extensions;
using OxxCommerceStarterKit.Web.Business;
using OxxCommerceStarterKit.Web.Helpers;
using OxxCommerceStarterKit.Web.Models.Catalog;
using OxxCommerceStarterKit.Web.Models.ViewModels;
using System.Web;

namespace OxxCommerceStarterKit.Web.Controllers
{
    public class CommerceControllerBase<T> : ContentController<T> where T : CatalogContentBase
    {
        private static Injected<IContentLoader> _contentLoaderService ;
        private static Injected<InventoryLoader> _inventoryLoaderService;
        private static Injected<ReadOnlyPricingLoader> _readonlyPricingLoaderService;
        private static Injected<ICurrentMarket> _icurrentMarketService;
        private static Injected<ILinksRepository> _linksRepositoryService;
        private static Injected<IWarehouseInventoryService> _inventoryService;
 
        

        protected IContentLoader ContentLoader
        {
            get { return _contentLoaderService.Service; }
        }

        protected InventoryLoader InventoryLoader
        {
            get { return _inventoryLoaderService.Service; }
        }

        protected ReadOnlyPricingLoader PricingLoader
        {
            get { return _readonlyPricingLoaderService.Service; }
        }

        protected ICurrentMarket CurrentMarket
        {
            get { return _icurrentMarketService.Service; }
        }

        protected ILinksRepository LinksRepository
        {
            get { return _linksRepositoryService.Service; }
        }

        protected IWarehouseInventoryService InventoryService
        {
            get { return _inventoryService.Service; }
        }

   

   
        protected static ILogger _log = LogManager.GetLogger();

        public void InitializeCatalogViewModel<TViewModel>(TViewModel model)
           where TViewModel : ICatalogViewModel<CatalogContentBase>
        {
            model.ChildCategories = model.ChildCategories ?? GetCatalogChildNodes(model.CatalogContent.ContentLink);
            model.Products = model.Products ?? CreateLazyProductContentViewModels(model.CatalogContent);
            model.Variants = model.Variants ?? CreateLazyVariantContentViewModels(model.CatalogContent);            
        }

        public void InitializeVariationViewModel<TViewModel>(TViewModel model)
            where TViewModel : IVariationViewModel<VariationContent>
        {
            model.Inventory = model.Inventory ?? GetInventory(model.CatalogContent, Constants.Warehouse.DefaultWarehouseCode);
            model.AllWarehouseInventory = model.AllWarehouseInventory ?? RetrieveByClosest(model.CatalogContent);
            model.Price = model.Price ?? GetPrice(model.CatalogContent);
            model.ParentEntry = model.CatalogContent.GetParent();
            model.ContentWithAssets = model.CatalogContent.CommerceMediaCollection.Any()
                ? model.CatalogContent
                : model.ParentEntry;            
        }

        private IEnumerable<WarehouseInventoryViewModel> RetrieveByClosest(VariationContent content)
        {
            IEnumerable<Inventory> inventories = GetAllWarehouseInventory(content);
            IEnumerable<WarehouseInventoryViewModel> sortedListing = SortByClosest(GetAllWarhousesByInventory(inventories));
            return sortedListing;
        }

        private IEnumerable<WarehouseInventoryViewModel> SortByClosest(IEnumerable<WarehouseInventoryViewModel> warehouses)
        {
            return warehouses;
        }

        private IEnumerable<WarehouseInventoryViewModel> GetAllWarhousesByInventory(IEnumerable<Inventory> inventories)
        {
            IList<WarehouseInventoryViewModel> allWarehouseInventories = new List<WarehouseInventoryViewModel>(inventories.Count());
            if (inventories.Count() > 0)
            {
                foreach (Inventory inventory in inventories)
                {
                    var warehouse = WarehouseHelper.GetWarehouse(inventory.WarehouseCode);
                    if (warehouse != null && warehouse.IsActive)
                    {
                        WarehouseInventoryViewModel model = new WarehouseInventoryViewModel()
                        {
                            WarehouseCode = warehouse.Code,
                            WarehouseName = HttpUtility.HtmlEncode(warehouse.Name),
                            WarehouseContact = HttpUtility.HtmlEncode(string.Format("{0} - {1}", warehouse.ContactInformation.FullName,
                                                                warehouse.ContactInformation.Email)),
                            WarehouseAddress = HttpUtility.HtmlEncode(string.Format("{0} {1}", warehouse.ContactInformation.Line1,
                                                                 warehouse.ContactInformation.City))

                        };
                        model.InStockLevel = inventory.InStockQuantity;
                        model.ReservedLevel = inventory.ReservedQuantity;
                        model.IsAvailable = (inventory.InStockQuantity - inventory.ReservedQuantity > 0);

                        allWarehouseInventories.Add(model);
                    }
                }
            }

            return allWarehouseInventories;
        }

        protected Lazy<IEnumerable<NodeContent>> GetCatalogChildNodes(ContentReference contentLink)
        {
            return new Lazy<IEnumerable<NodeContent>>(() =>
                ContentLoader.GetChildren<NodeContent>(contentLink)
                .ToList());
        }

        protected ICatalogViewModel<CatalogContentBase> CreateFashionCatalogViewModel(CatalogContentBase catalogContent)
        {
            var activator = new Activator<ICatalogViewModel<CatalogContentBase>>();
            var model = activator.Activate(typeof(CatalogContentViewModel<>), catalogContent);
            InitializeCatalogViewModel(model);
            return model;
        }

        private LazyProductViewModelCollection CreateLazyProductContentViewModels(CatalogContentBase catalogContent)
        {
            return new LazyProductViewModelCollection(() =>
            {
                var products = GetChildrenAndRelatedEntries<ProductContent>(catalogContent);
                return products.Select(x => CreateProductViewModel(x));
            });
        }
        protected LazyVariationViewModelCollection CreateLazyVariantContentViewModels(CatalogContentBase catalogContent)
        {
            return new LazyVariationViewModelCollection(() =>
            {
                var variants = GetChildrenAndRelatedEntries<VariationContent>(catalogContent);
                return variants.Select(x => CreateVariationViewModel<VariationContent>(x));
            });
        }

        public IProductViewModel<ProductContent> CreateProductViewModel(ProductContent productContent)
        {
            var activator = new Activator<IProductViewModel<ProductContent>>();
            var model = activator.Activate(typeof(ProductViewModel<>), productContent);
            InitializeCatalogViewModel(model);
            return model;
        }

        public IVariationViewModel<TContent> CreateVariationViewModel<TContent>(TContent variationContent)
            where TContent : VariationContent            
        {
            var activator = new Activator<IVariationViewModel<TContent>>();
            var model = activator.Activate(typeof(VariationViewModel<TContent>), variationContent);
            InitializeVariationViewModel(model);
            return model;
        }

        public ContentArea CreateRelatedProductsContentArea(EntryContentBase catalogContent, string associationType)
        {
            IEnumerable<Association> associations = LinksRepository.GetAssociations(catalogContent.ContentLink);
            ContentArea relatedEntriesCA = new ContentArea();
            List<EntryContentBase> relatedEntires = Enumerable.Where(associations, p => p.Group.Name.Equals(associationType))
                    .Select(a => ContentLoader.Get<EntryContentBase>(a.Target)).ToList();
            foreach (var relatedEntire in relatedEntires)
            {
                ContentAreaItem caItem = new ContentAreaItem();
                caItem.ContentLink = relatedEntire.ContentLink;
                relatedEntriesCA.Items.Add(caItem);
            }
            return relatedEntriesCA;
        }

        private IEnumerable<TEntryContent> GetChildrenAndRelatedEntries<TEntryContent>(CatalogContentBase catalogContent)
            where TEntryContent : EntryContentBase
        {
            var variantContentItems = ContentLoader.GetChildren<TEntryContent>(catalogContent.ContentLink).ToList();

            var variantContainer = catalogContent as IVariantContainer;
            if (variantContainer != null)
            {
                variantContentItems.AddRange(GetRelatedEntries<TEntryContent>(variantContainer));
            }

            return variantContentItems.Where(e => e.IsAvailableInCurrentMarket(CurrentMarket));
        }

        private IEnumerable<TEntryContent> GetRelatedEntries<TEntryContent>(IVariantContainer content)
            where TEntryContent : EntryContentBase
        {
            var relatedItems = content.GetVariantRelations(LinksRepository).Select(x => x.Target);
            return ContentLoader.GetItems(relatedItems, LanguageSelector.AutoDetect()).OfType<TEntryContent>();
        }

        private IEnumerable<Inventory> GetAllWarehouseInventory(VariationContent catalogContent)
        {
            //make sure we don't get any null reference exception when accessing Inventories.
            var inventories = Enumerable.Empty<Inventory>();
            
            if (catalogContent != null && catalogContent.IsAvailableInCurrentMarket())
            {
                inventories = catalogContent.GetStockPlacement(InventoryLoader);
            }
            
            return inventories;

        }

        private Lazy<Inventory> GetInventory(IStockPlacement stockPlacement, string warehouseCode)
        {
            return new Lazy<Inventory>(() => stockPlacement.GetStockPlacement(InventoryLoader).FirstOrDefault(x => x.WarehouseCode == warehouseCode), true);
        }

        protected Price GetPrice(IPricing pricing)
        {
            return pricing.GetPrices(PricingLoader).FirstOrDefault(x => x.MarketId == CurrentMarket.GetCurrentMarket().MarketId);
        }

        protected bool IsSellable(VariationContent variationContent)
        {
            var inventory = InventoryService.GetTotal(new CatalogKey(AppContext.Current.ApplicationId, variationContent.Code));

            return HasPrice(variationContent) &&
                inventory != null &&
                inventory.InStockQuantity - inventory.ReservedQuantity > 0;
        }

        protected bool HasPrice(VariationContent variationContent)
        {
            var price = variationContent
                .GetPrices(PricingLoader)
                .FirstOrDefault(x => x.MarketId == CurrentMarket.GetCurrentMarket().MarketId);
            return price != null &&
                price.UnitPrice != null;
        }

        /// <summary>
        /// Get the size guide for the product
        /// </summary>
        /// <param name="currentContent"></param>
        /// <returns></returns>
        protected int GetSizeGuide(FashionProductContent currentContent)
        {
            int output = UrlHelpers.GetReferenceFromUrl(currentContent.SizeGuide);
            if (output > 0)
            {
                return output;
            }

            var ancestors = ContentLoader.GetAncestors(currentContent.ContentLink);
            foreach (var ancestor in ancestors)
            {
                var content = ContentLoader.Get<IContent>(ancestor.ContentLink);
                if (content is FashionStoreSubLandingNodeContent)
                {
                    output = UrlHelpers.GetReferenceFromUrl(((FashionStoreSubLandingNodeContent)content).SizeGuide);
                }
                else if (content is FashionStoreLandingNodeContent)
                {
                    output = UrlHelpers.GetReferenceFromUrl(((FashionStoreLandingNodeContent)content).SizeGuide);
                }
                if (output > 0)
                {
                    return output;
                }
            }
            return -1;
        }

        protected virtual PriceModel GetPriceModel(VariationContent currentContent)
        {
            PriceModel priceModel = new PriceModel();
            priceModel.Price = GetPrice(currentContent);
            priceModel.DiscountDisplayPrice = currentContent.GetDiscountDisplayPrice(currentContent.GetDefaultPrice());
            priceModel.CustomerClubDisplayPrice = currentContent.GetCustomerClubDisplayPrice();
            return priceModel;
        }


    }
}
