using EPiServer.Core;

namespace OxxCommerceStarterKit.Web.Models.Catalog
{
    public interface IDigitalCameraContent
    {
        XhtmlString Description { get; set; }
        XhtmlString Overview { get; set; }
        decimal Resolution { get; set; }
        string LensMount { get; set; }
        string CameraFormat { get; set; }
        string FileFormat { get; set; }
        string Connectivity { get; set; }
        string MemoryCardType { get; set; }
        string Battery { get; set; }
        XhtmlString FocusControl { get; set; }
    }
}