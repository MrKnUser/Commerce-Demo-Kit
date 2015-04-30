using EPiServer.Find.Api.Facets;

namespace OxxCommerceStarterKit.Web.Business.FacetRegistry
{
    public class SelectableNumericRange : NumericRange, ISelectable
    {
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
    }
}