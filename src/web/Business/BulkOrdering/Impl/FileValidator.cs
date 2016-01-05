using System.IO;
using System.Web;
using OxxCommerceStarterKit.Web.Business.BulkOrdering.Interfaces;

namespace OxxCommerceStarterKit.Web.Business.BulkOrdering.Impl
{
    public class FileValidator : IFileValidator
    {
        public bool FileIsValid(HttpPostedFileBase fileUpload)
        {
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                string extension = Path.GetExtension(fileUpload.FileName);
                switch (extension)
                {
                    case ".csv":
                    //case ".xlsx":
                    //case ".xls":
                        return true;

                    default:
                        return false;
                }
            }

            return false;
        }
    }
}