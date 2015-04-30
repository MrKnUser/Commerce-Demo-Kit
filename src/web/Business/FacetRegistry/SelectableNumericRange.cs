using EPiServer.Find.Api.Facets;
using Newtonsoft.Json;

namespace OxxCommerceStarterKit.Web.Business.FacetRegistry
{
    /// <summary>
    /// A selectable numeric range
    /// </summary>
    /// <remarks>
    /// Note! We cannot inherit from NumericRange due to Json deserializing issues
    /// </remarks>
    public class SelectableNumericRange : ISelectable
    {
        public SelectableNumericRange()
        {
            
        }

        public SelectableNumericRange(NumericRange numericRange)
        {
            From = numericRange.From;
            To = numericRange.To;
        }

        private string _id;

        public string Id
        {
            get
            {
                if(string.IsNullOrEmpty(_id))
                {
                    string from = From == null ? "MIN" : From.ToString();
                    string to = To == null ? "MAX" : To.ToString();
                    return from + "-" + to;
                }
                return _id;
            }
            set { _id = value; }
        }

        public bool Selected { get; set; }

        [JsonProperty("from", NullValueHandling = NullValueHandling.Ignore)]
        public double? From { get; set; }

        [JsonProperty("to", NullValueHandling = NullValueHandling.Ignore)]
        public double? To { get; set; }

        public NumericRange ToNumericRange()
        {
            return new NumericRange(From, To);
        }

    }
}