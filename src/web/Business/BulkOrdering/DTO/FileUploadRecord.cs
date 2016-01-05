using FileHelpers;

namespace OxxCommerceStarterKit.Web.Business.BulkOrdering.DTO
{
    [DelimitedRecord(",")]
    [IgnoreFirst]
    public class FileUploadRecord
    {
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
    }
}