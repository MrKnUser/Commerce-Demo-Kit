using System.Collections.Generic;

namespace CommerceStarterKit.CatalogImporter.DTO
{
    public class Property
    {
        public string language { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Node
    {
        public string urlSegment { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string parent { get; set; }
        public List<Property> properties { get; set; }
        public string contentType { get; set; }
        public List<Image> images { get; set; }
    }

    public class Entry
    {
        public string name { get; set; }
        public string code { get; set; }
        public string urlSegment { get; set; }
        public string parent { get; set; }
        public List<Property> properties { get; set; }
        public string contentType { get; set; }
        public List<Image> images { get; set; }
        public List<Price> prices { get; set; }
    }

    public class Image
    {
        public string path { get; set; }
        public string groupName { get; set; }
    }

    public class Defaults
    {
        public string defaultNodeType { get; set; }
        public string defaultEntryType { get; set; }
        public int variationMinQuantity { get; set; }
        public int variationMaxQuantity { get; set; }
        public bool variationEnableTracking { get; set; }
        public int variationDefaultInventoryStock { get; set; }
    }

    public class Price
    {
        public decimal price { get; set; }
        public string currency { get; set; }
        public string marketId { get; set; }
    }

    public class CatalogRoot
    {
        public string catalog { get; set; }
        public List<Node> nodes { get; set; }
        public List<Entry> entries { get; set; }
        public Defaults defaults { get; set; }
    }
}
