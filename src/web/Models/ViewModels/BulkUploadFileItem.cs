namespace OxxCommerceStarterKit.Web.Models.ViewModels
{
    public class BulkUploadFileItem
    {
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public bool CouldLookupProduct { get; set; }
    }
}