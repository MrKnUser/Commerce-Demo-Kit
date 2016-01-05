using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using EPiServer.Cms.Shell;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc.Html;
using OxxCommerceStarterKit.Core.Repositories;
using OxxCommerceStarterKit.Core.Repositories.Interfaces;
using OxxCommerceStarterKit.Core.Services;
using OxxCommerceStarterKit.Web.Business.BulkOrdering.Impl;
using OxxCommerceStarterKit.Web.Business.BulkOrdering.Impl.FileParsers;
using OxxCommerceStarterKit.Web.Business.BulkOrdering.Interfaces;
using OxxCommerceStarterKit.Web.Business.Rendering; 
using StructureMap;

namespace OxxCommerceStarterKit.Web.Business.BulkOrdering.Initialization
{
    [InitializableModule]
    [ModuleDependency(typeof(InitializableModule))]
    public class RegisterImplementationsInit : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Container.Configure(ConfigureContainer);
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(context.Container));
        }

        private static void ConfigureContainer(ConfigurationExpression container)
        {
            container.For<IFileParser>()
                .Use<CsvFileParser>()
                .Named(".csv-parser");

            // More work required before easily being able to parse Excel files!
            //container.For<IFileParser>()
            //    .Use<ExcelFileParser>()
            //    .Named(".xlsx-parser");

            //container.For<IFileParser>()
            //    .Use<ExcelFileParser>()
            //    .Named(".xls-parser");

            container.For<IFileValidator>()
                .Use<FileValidator>();

            container.For<IProductLookup>()
                .Use<ProductLookup>();
        }

        public void Initialize(InitializationEngine context) { }

        public void Uninitialize(InitializationEngine context) { }

    }
}