using System.Collections.Generic;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Orders.Managers;
using Mediachase.Commerce.Website.Helpers;
using OxxCommerceStarterKit.Web.Models.PageTypes;

namespace OxxCommerceStarterKit.Web.Models.ViewModels
{
    public class BulkUploadViewModel : PageViewModel<BulkUploadModelPage>
    {
        public BulkUploadViewModel()
            : base()
        {
            SectionToDisplay = DisplaySection.UploadFile;
        }

        public BulkUploadViewModel(BulkUploadModelPage currentPage)
            : base(currentPage)
        {
            SectionToDisplay = DisplaySection.UploadFile;
        }

        public enum DisplaySection
        {
            UploadFile,
            ConfirmResults
        }

        public DisplaySection SectionToDisplay { get; set; }
        public IList<BulkUploadFileItem> ParsedUploadItems { get; set; }
        public string Error { get; set; }
    }
}