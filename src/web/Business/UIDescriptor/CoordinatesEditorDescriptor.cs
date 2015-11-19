using EPiServer.Shell.ObjectEditing.EditorDescriptors;


namespace OxxCommerceStarterKit.Web.Business.UIDescriptor
{
    [EditorDescriptorRegistration(TargetType = typeof(string), UIHint = UIHint)]
    public class CoordinatesEditorDescriptor : EditorDescriptor
    {
        public const string UIHint = "CoordinatesEditorDescriptor";

        public CoordinatesEditorDescriptor()
        {
            ClientEditingClass = "tedgustaf.googlemaps.Editor";
        }
    }
}
