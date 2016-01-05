using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using OxxCommerceStarterKit.Core.Attributes;

namespace OxxCommerceStarterKit.Web.Models.PageTypes
{
    [ContentType(GUID = "CBD17DFC-F984-486A-9FFC-2E4D3E1A9459",
        DisplayName = "Bulk cart create page",
        GroupName = WebGlobal.GroupNames.Commerce,
        Order = 100,
        AvailableInEditMode = true,
        Description = "Used in B2B scenarios where users want to bulk upload a CSV order and add these items to the cart")]
    [SiteImageUrl(thumbnail: EditorThumbnail.Commerce)]
    public class BulkUploadModelPage : CommerceSampleModulePage
    {
        [Display(
            Name = "Instruction header",
            Description = "Header for the instructions that appear in the right hand column",
            GroupName = "User Instructions",
            Order = 10)]
        [CultureSpecific]
        public virtual string InstructionsHeader { get; set; }

        [Display(
            Name = "Instruction body",
            Description = "Body of the instructions",
            GroupName = "User Instructions",
            Order = 20)]
        [CultureSpecific]
        public virtual XhtmlString InstructionsBody { get; set; }

        [Display(
            Name = "Error header",
            Description = "Message that appears above the list of error messages",
            GroupName = "Errors",
            Order = 10)]
        [CultureSpecific]
        public virtual string ErrorHeader { get; set; }

        [Display(
            Name = "Invalid file",
            Description = "Error message displayed when no valid file could be found",
            GroupName = "Errors",
            Order = 20)]
        [CultureSpecific]
        public virtual string InvalidFile { get; set; }

        [Display(
            Name = "No file parser",
            Description = "Error message displayed when a file parser could not be found",
            GroupName = "Errors",
            Order = 30)]
        [CultureSpecific]
        public virtual string NoParserFound { get; set; }

        [Display(
            Name = "File parse exception",
            Description = "Error message displayed when an error is returned from the file parser",
            GroupName = "Errors",
            Order = 40)]
        [CultureSpecific]
        public virtual string ParseFileError { get; set; }

        

        [Display(
            Name = "File upload heading",
            Description = "Message that appears above the file upload selector",
            GroupName = "Page messages",
            Order = 10)]
        [CultureSpecific]
        public virtual string FileUploadMessage { get; set; }

        [Display(
            Name = "Review your items heading",
            Description = "Heading to be shown once items have been uploaded",
            GroupName = "Page messages",
            Order = 20)]
        [CultureSpecific]
        public virtual string ReviewItems { get; set; }

        [Display(
            Name = "Review your items sub-heading",
            Description = "Sub-heading to be shown once items have been uploaded",
            GroupName = "Page messages",
            Order = 30)]
        [CultureSpecific]
        public virtual XhtmlString ReviewItemsSubHeading { get; set; }


        [Display(
            Name = "Upload File",
            GroupName = "Button Text",
            Order = 10)]
        [CultureSpecific]
        public virtual string UploadFile { get; set; }

        [Display(
            Name = "Confirm and move to Cart",
            GroupName = "Button Text",
            Order = 20)]
        [CultureSpecific]
        public virtual string ConfirmAndMoveToCart { get; set; }

        [Display(
            Name = "Clear and start again",
            GroupName = "Button Text",
            Order = 30)]
        [CultureSpecific]
        public virtual string ClearAndStartAgain { get; set; }


        [Display(
            Name = "Product Code",
            GroupName = "Table text",
            Order = 10)]
        [CultureSpecific]
        [Required]
        public virtual string ProductCode { get; set; }

        [Display(
            Name = "Description",
            GroupName = "Table text",
            Order = 20)]
        [CultureSpecific]
        [Required]
        public virtual string Description { get; set; }

        [Display(
            Name = "Amount",
            GroupName = "Table text",
            Order = 30)]
        [CultureSpecific]
        [Required]
        public virtual string Amount { get; set; }

        [Display(
            Name = "Status",
            GroupName = "Table text",
            Order = 40)]
        [CultureSpecific]
        [Required]
        public virtual string UploadStatus { get; set; }

        [Display(
            Name = "Product Found",
            GroupName = "Table text",
            Order = 50)]
        [CultureSpecific]
        [Required]
        public virtual string ProductFound { get; set; }

        [Display(
            Name = "Product Not Found",
            GroupName = "Table text",
            Order = 60)]
        [CultureSpecific]
        [Required]
        public virtual string ProductNotFound { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            InstructionsHeader = "Instructions for use";
            InstructionsBody = new XhtmlString(@"<p>Create a .csv file with two columns:</p><ol><li>The product code</li><li>The quantity of the product to order</li></ol><p>Select the file and click the upload button. Review the item list, once happy click the green confirmation button to move the items to the cart and get a total price.</p>");

            ErrorHeader = "Please correct the following:";
            InvalidFile = "File type is not valid. CSV is the only allowed file type";
            NoParserFound = "Could not upload file as no matching parser for the file type could be found";
            ParseFileError = "Error parsing file. The error returned is: ";

            ReviewItems = "Review your items";
            ReviewItemsSubHeading = new XhtmlString(@"<strong>Note:</strong> Your price will be shown in the cart");

            UploadFile = "Upload file";
            ConfirmAndMoveToCart = "Confirm and move to cart";
            ClearAndStartAgain = "Clear and start again";

            ProductCode = "Product Code";
            Description = "Description";
            Amount = "Quantity";
            UploadStatus = "Status";
            ProductFound = "OK";
            ProductNotFound = "Product code not found (will be ignored)";
        }

    }
}