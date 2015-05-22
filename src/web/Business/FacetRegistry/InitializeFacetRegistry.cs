using EPiServer.Find.Api.Facets;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace OxxCommerceStarterKit.Web.Business.FacetRegistry
{
    
    [ModuleDependency(typeof(ConfigureFacetRegistryInContainer))]
    public class InitializeFacetRegistry : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            IFacetRegistry registry = ServiceLocator.Current.GetInstance<IFacetRegistry>();
            registry.FacetDefinitions.Add(new FacetStringDefinition()
            {
                Name = "Category",
                FieldName = "CategoryName"
            });
            registry.FacetDefinitions.Add(new FacetStringDefinition()
            {
                Name = "CountryFacet",
                FieldName = "Country"
            });

            registry.FacetDefinitions.Add(new FacetStringDefinition()
            {
                Name = "RegionFacet",
                FieldName = "Region"
            });

            FacetNumericRangeDefinition priceRanges = new FacetNumericRangeDefinition()
            {
                Name = "SalesPriceFilter",
                FieldName = "DefaultPriceAmount",
                BackingType = typeof(int)

            };
            priceRanges.Range.Add(new SelectableNumericRange() { To = 100 });
            priceRanges.Range.Add(new SelectableNumericRange() { From = 100, To = 200 });
            priceRanges.Range.Add(new SelectableNumericRange() { From = 200, To = 500 });
            priceRanges.Range.Add(new SelectableNumericRange() { From = 500 });
            registry.FacetDefinitions.Add(priceRanges);

            registry.FacetDefinitions.Add(new FacetStringListDefinition()
            {
                Name = "ColorFacet",
                FieldName = "Color"
            });

            registry.FacetDefinitions.Add(new FacetStringListDefinition()
            {
                Name = "SizesFacet",
                FieldName = "Sizes"
            });

            registry.FacetDefinitions.Add(new FacetStringDefinition()
            {
                Name = "VintageFacet",
                FieldName = "Vintage"
            });

            
            registry.FacetDefinitions.Add(new FacetStringDefinition()
            {
                Name = "ClosureFacet",
                FieldName = "Closure"
            });

            registry.FacetDefinitions.Add(new FacetStringDefinition()
            {
                Name = "BrandFacet",
                FieldName = "Brand"
            });

            registry.FacetDefinitions.Add(new FacetStringDefinition()
            {
                Name = "Connectivity",
                FieldName = "Connectivity"
            });



        }

        public void Uninitialize(InitializationEngine context)
        {
            
        }

        public void Preload(string[] parameters)
        {
            
        }
    }
}