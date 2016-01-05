using System.Web;

namespace OxxCommerceStarterKit.Web.Business.BulkOrdering.Interfaces
{
    public interface IFileValidator
    {
        bool FileIsValid(HttpPostedFileBase fileUpload);
    }
}