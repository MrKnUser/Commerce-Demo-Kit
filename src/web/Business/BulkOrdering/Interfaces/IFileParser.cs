using System.Collections.Generic;
using System.Web;
using OxxCommerceStarterKit.Web.Models.ViewModels;

namespace OxxCommerceStarterKit.Web.Business.BulkOrdering.Interfaces
{
    public interface IFileParser
    {
        string ParseError { get; set; }
        IList<BulkUploadFileItem> ParseFile(HttpPostedFileBase file);
    }
}