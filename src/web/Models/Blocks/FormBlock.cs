using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.XForms;
using OxxCommerceStarterKit.Core.Attributes;

namespace OxxCommerceStarterKit.Web.Models.Blocks
{
    /// <summary>
    /// Used to insert an XForm
    /// </summary>
    [ContentType(
        GroupName = WebGlobal.GroupNames.Specialized,
        GUID = "FA326346-4D4C-4E82-AFE8-C36279006179",
        DisplayName = "Form",
        Description = "A dynamic input form")]
    [SiteImageUrl]
    public class FormBlock : SiteBlockData
    {
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 1)]
        [CultureSpecific]
        public virtual string Heading { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 2)]
        [CultureSpecific]
        public virtual XForm Form { get; set; }
    }
}
