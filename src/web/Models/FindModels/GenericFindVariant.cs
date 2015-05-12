using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Find;
using OxxCommerceStarterKit.Core.Models;

namespace OxxCommerceStarterKit.Web.Models.FindModels
{
    public class GenericFindVariant
    {
        [Id]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public List<PriceAndMarket> Prices { get; set; }
        public string Code { get; set; }
        public decimal Stock { get; set; }
    }
}