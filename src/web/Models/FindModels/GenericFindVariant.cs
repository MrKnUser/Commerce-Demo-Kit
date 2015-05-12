using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Find;
using OxxCommerceStarterKit.Core.Models;

namespace OxxCommerceStarterKit.Web.Models.FindModels
{
    public class GenericFindVariant : FindProduct
    {
  
        public string Size { get; set; }
        public List<PriceAndMarket> Prices { get; set; }

        public decimal Stock { get; set; }
    }
}