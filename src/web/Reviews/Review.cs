using System;
using EPiServer.Core;
using EPiServer.DataAnnotations;

namespace OxxCommerceStarterKit.Web.Reviews
{
    [ContentType(GUID = "6901086D-0B9C-43CC-B9AD-F4D6387FAD74", AvailableInEditMode = true)]
    public class Review : ContentData, IContent
    {
        public virtual int Rating { get; set; }
        public virtual string ProductNumber { get; set; }
        public virtual int ContentId { get; set; }
        public virtual string Heading { get; set; }
        [BackingType(typeof(PropertyLongString))]
        public virtual string Text { get; set; }
        public virtual string UserDisplayName { get; set; }
        public virtual string UserId { get; set; }
        public virtual DateTime ReviewDate { get; set; }

        //IContent implementation
        public string Name { get; set; }
        public ContentReference ContentLink { get; set; }
        public ContentReference ParentLink { get; set; }
        public Guid ContentGuid { get; set; }
        public int ContentTypeID { get; set; }
        public bool IsDeleted { get; set; }
    }
}
