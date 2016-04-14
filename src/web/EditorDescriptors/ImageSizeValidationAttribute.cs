using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using OxxCommerceStarterKit.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OxxCommerceStarterKit.Web.EditorDescriptors
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ImageSizeValidationAttribute : ValidationAttribute
    {
        public int MaxHeight { get; set; }

        public int MaxWidth { get; set; }

        public int MinHeight { get; set; }

        public int MinWidth { get; set; }

        public override bool IsValid(object value)
        {
            return ValidateContentArea(value as ContentArea);
        }

        private bool ValidateContentArea(ContentArea contentArea)
        {
            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            
            if (contentArea.Items.Count > 0)
            {
                foreach (var item in contentArea.Items)
                {
                    ImageFile file = null;
                    if (contentLoader.TryGet<ImageFile>(item.ContentLink, out file))
                    {
                        var dimensions = ImageBlobUtility.GetDimensions(file.BinaryData);
                        if (dimensions.Height < MinHeight || dimensions.Height > MaxHeight)
                        {
                            ErrorMessage = string.Format("Image {0} must be within a height of {1} and {2} pixels.", file.Name, MinHeight, MaxHeight);
                            return false;
                        }
                        else if (dimensions.Width < MinWidth || dimensions.Width > MaxWidth)
                        {
                            ErrorMessage = string.Format("Image {0} must be within a width of {1} and {2} pixels.", file.Name, MinWidth, MaxWidth);
                            return false;
                        }

                    }                    
                }
            }

            return true;
        }
    }
}
