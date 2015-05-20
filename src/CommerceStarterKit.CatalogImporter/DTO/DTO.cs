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
    }

    public class Entry
    {
        public string name { get; set; }
        public string code { get; set; }
        public string urlSegment { get; set; }
        public string parent { get; set; }
        public List<Property> properties { get; set; }
        public string contentType { get; set; }
    }

    public class CatalogRoot
    {
        public string catalog { get; set; }
        public List<Node> nodes { get; set; }
        public List<Entry> entries { get; set; }
        public string defaultNodeType { get; set; }
        public string defaultEntryType { get; set; }
    }
}
