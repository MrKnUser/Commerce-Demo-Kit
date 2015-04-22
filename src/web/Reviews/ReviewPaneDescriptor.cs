using System;
using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Shell;

namespace OxxCommerceStarterKit.Web.Reviews
{
    [ServiceConfiguration(typeof(IContentRepositoryDescriptor))]
    public class ReviewPaneDescriptor : ContentRepositoryDescriptorBase
    {
        public static string RepositoryKey { get { return "reviews"; } }
        public override string Key { get { return RepositoryKey; } }
        public override string Name { get { return "Product reviews"; } }

        public override IEnumerable<Type> ContainedTypes
        {
            get { return new[] { typeof(Review), typeof(ContentFolder) }; }
        }

        public override IEnumerable<Type> CreatableTypes
        {
            get { return new List<Type> { typeof(Review) }; }
        }

        public override IEnumerable<ContentReference> Roots
        {
            get { return new ContentReference[0]; }
        }

        public override IEnumerable<Type> MainNavigationTypes
        {
            get { return new[] { typeof(ContentFolder) }; }
        }
    }
}
