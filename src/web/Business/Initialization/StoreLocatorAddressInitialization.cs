using System;
using System.Linq;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Core;
using OxxCommerceStarterKit.Web.Models.PageTypes;
using OxxCommerceStarterKit.Web.Helpers;
using EPiServer;

namespace OxxCommerceStarterKit.Web.Business.Initialization
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class StoreLocatorAddressInitialization : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            var contentEvents = ServiceLocator.Current.GetInstance<IContentEvents>();
            contentEvents.SavedContent += ContentEvents_PublishedContent;
        }

        private void ContentEvents_PublishedContent(object sender, EPiServer.ContentEventArgs e)
        {
            if (e.Content is StoreLocationPage)
            {
                var storeLocation = e.Content as StoreLocationPage;
                // Update if not set already and warehouse code is added
                if (!string.IsNullOrEmpty(storeLocation.WarehouseCode) && string.IsNullOrEmpty(storeLocation.Address))
                {
                    // Find the warehouse
                    var warehouse = WarehouseHelper.GetWarehouse(storeLocation.WarehouseCode);
                    if (warehouse != null)
                    {
                        StoreLocationPage updatedPage = storeLocation.CreateWritableClone() as StoreLocationPage;
                        // Map properties to page
                        updatedPage.Address = SetupAddress(warehouse.ContactInformation.Line1, warehouse.ContactInformation.Line2);
                        updatedPage.City = warehouse.ContactInformation.City;
                        updatedPage.PostCode = warehouse.ContactInformation.PostalCode;
                        updatedPage.State = warehouse.ContactInformation.State;
                        updatedPage.Country = warehouse.ContactInformation.CountryName;
                        updatedPage.Telephone = warehouse.ContactInformation.DaytimePhoneNumber;
                        updatedPage.Fax = warehouse.ContactInformation.FaxNumber;
                        updatedPage.Email = warehouse.ContactInformation.Email;
                        // Update
                        var repository = ServiceLocator.Current.GetInstance<IContentRepository>();
                        repository.Save(updatedPage, EPiServer.DataAccess.SaveAction.ForceCurrentVersion);
                    }
                }
            }
        }
        
        private string SetupAddress(string line1, string line2)
        {
            string output = line1;

            if (!string.IsNullOrWhiteSpace(line2))
            {
                output += ", " + line2;
            }

            return output;
        }

        public void Preload(string[] parameters) { }

        public void Uninitialize(InitializationEngine context)
        {
            var contentEvents = ServiceLocator.Current.GetInstance<IContentEvents>();
            contentEvents.SavedContent -= ContentEvents_PublishedContent;
        }
    }
}