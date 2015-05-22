using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using OxxCommerceStarterKit.Web.EditorDescriptors;

namespace OxxCommerceStarterKit.Web.Models.PageTypes.Payment
{
    public abstract class BasePaymentPage : PageData
    {
        [CultureSpecific]
        [EditorDescriptor(EditorDescriptorType = typeof(PaymentMethodEditorDescriptor))]
        public virtual string PaymentMethod { get; set; }


        [Display(
            GroupName = SystemTabNames.Content,
            Order = 19)]
        [CultureSpecific(true)]
        public virtual  XhtmlString Description { get; set; }


    }
}