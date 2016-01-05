using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using FileHelpers;
using OxxCommerceStarterKit.Web.Business.BulkOrdering.DTO;
using OxxCommerceStarterKit.Web.Business.BulkOrdering.Interfaces;
using OxxCommerceStarterKit.Web.Models.ViewModels;

namespace OxxCommerceStarterKit.Web.Business.BulkOrdering.Impl.FileParsers
{
    public class CsvFileParser : IFileParser
    {
        IProductLookup _productLoopup;

        public CsvFileParser(IProductLookup productLoopup)
        {
            this._productLoopup = productLoopup;
        }

        public string ParseError { get; set; }

        public IList<BulkUploadFileItem> ParseFile(HttpPostedFileBase file)
        {
            try
            {
                // Parse out the data
                IList<BulkUploadFileItem> parsedItems = new List<BulkUploadFileItem>();

                using (var reader = new StreamReader(file.InputStream))
                {
                    var engine = new FileHelperEngine<FileUploadRecord>();
                    var records = engine.ReadStream(reader);

                    foreach (var record in records)
                    {
                        parsedItems.Add(new BulkUploadFileItem()
                        {
                            ProductCode = record.ProductCode,
                            Description = string.Empty,
                            Quantity = record.Quantity,
                            CouldLookupProduct = true
                        });
                    }

                    _productLoopup.LookupProducts(parsedItems);

                    // Now add descriptions
                    foreach (var uploadFileItem in parsedItems)
                    {
                        
                    }
                }
                return parsedItems;
            }
            catch (Exception ex)
            {
                ParseError = "Could not parse CSV file, the error returned is: " + ex.ToString();
                return null;
            }
        }
    }
}