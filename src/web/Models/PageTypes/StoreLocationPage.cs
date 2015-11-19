using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using OxxCommerceStarterKit.Core.Attributes;
using EPiServer.Find;
using EPiServer.Shell.ObjectEditing;
using OxxCommerceStarterKit.Web.Business.UIDescriptor;

namespace OxxCommerceStarterKit.Web.Models.PageTypes
{
    [ContentType(DisplayName = "Store Location", GUID = "a023a560-7b70-4fca-b3b4-e945c82d38af", Description = "Presents a store location.  Either enter Warehouse code or Address details", GroupName = WebGlobal.GroupNames.Default)]
    [SiteImageUrl(thumbnail: EditorThumbnail.Content)]
    public class StoreLocationPage : SitePage
    {

        [Display(GroupName = SystemTabNames.Content, Name = "Location Name",
            Order = 1)]
        [CultureSpecific]
        public virtual string LocationName { get; set; }

        [Display(
            Name = "Body",
            GroupName = SystemTabNames.Content,
            Order = 2)]
        [CultureSpecific]
        public virtual XhtmlString BodyText { get; set; }

        [Display(
        Name = "Content area bottom",
        GroupName = SystemTabNames.Content,
        Order = 10)]
        [CultureSpecific]
        public virtual ContentArea BodyContent { get; set; }

        [Display(
        GroupName = WebGlobal.GroupNames.Contact,
        Order = 1
            )]
        public virtual string Telephone { get; set; }

        [Display(
        GroupName = WebGlobal.GroupNames.Contact,
        Order = 2
            )]
        public virtual string Fax { get; set; }

        [Display(
        GroupName = WebGlobal.GroupNames.Contact,
        Order = 3
            )]
        [EmailAddress]
        public virtual string Email { get; set; }

        [Display(
        Name = "Warehouse Code",
        GroupName = WebGlobal.GroupNames.Location,
        Order = 1
            )]
        public virtual string WarehouseCode { get; set; }


        [Display(GroupName = WebGlobal.GroupNames.Location,
                    Order = 2
                    )]
        public virtual string Address { get; set; }

        [Display(
            GroupName = WebGlobal.GroupNames.Location,
            Order = 3)]
        public virtual string City { get; set; }

        [Display(
            GroupName = WebGlobal.GroupNames.Location,
            Order = 4)]
        public virtual string PostCode { get; set; }

        [Display(
            GroupName = WebGlobal.GroupNames.Location,
            Order = 5)]
        public virtual string State { get; set; }

        [Display(
            GroupName = WebGlobal.GroupNames.Location,
            Order = 6)]
        public virtual string Country { get; set; }

        [Display(Name = "Select Location",
                    GroupName = WebGlobal.GroupNames.Location,
                    Order = 7)]
        [EditorDescriptor(EditorDescriptorType = typeof(CoordinatesEditorDescriptor))]
        public virtual string GeoLocation { get; set; }


        [Ignore]
        public GeoLocation Coordinates
        {
            get
            {
                if (string.IsNullOrWhiteSpace(GeoLocation))
                {
                    return null;
                }
                int splitter = GeoLocation.IndexOf(',');
                if (splitter <= 0)
                {
                    return null;
                }

                double latitude = 0, longitude = 0;
                latitude = Convert.ToDouble(GeoLocation.Substring(0, splitter));
                longitude = Convert.ToDouble(GeoLocation.Substring(splitter + 1));

                return new GeoLocation(latitude, longitude);
            }
        }

    }
}