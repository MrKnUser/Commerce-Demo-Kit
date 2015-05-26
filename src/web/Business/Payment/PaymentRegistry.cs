using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using OxxCommerceStarterKit.Web.Models.PageTypes;
using OxxCommerceStarterKit.Web.Models.PageTypes.Payment;
using OxxCommerceStarterKit.Web.Models.ViewModels;

namespace OxxCommerceStarterKit.Web.Business.Payment
{
    [ServiceConfiguration]
    public class PaymentRegistry
    {
        private readonly IContentRepository _contentRepository;
        private readonly UrlResolver _urlResolver;

        public PaymentRegistry(IContentRepository contentRepository, UrlResolver urlResolver)
        {
            _contentRepository = contentRepository;
            _urlResolver = urlResolver;
        }

        public virtual BasePaymentPage GetPaymentContentPageByMethodId(Guid paymentMethodId)
        {
            return GetPaymentContentPageByMethodId(paymentMethodId.ToString());
        }

        public virtual BasePaymentPage GetPaymentContentPageByMethodId(string paymentMethodId)
        {
            var paymentPages = GetPaymentContentPages();
            foreach (var p in paymentPages)
            {
                if (p.PaymentMethod.Equals(paymentMethodId))
                {
                    return p;
                }
            }
            return null;
        }

        public virtual IEnumerable<BasePaymentPage> GetPaymentContentPages()
        {
            var startPage = _contentRepository.Get<HomePage>(ContentReference.StartPage);
            var paymentPages = _contentRepository.GetChildren<BasePaymentPage>(startPage.Settings.PaymentContainerPage);
            return paymentPages;
        }


        /// <summary>
        /// Gets the url to the page handling a specific payment. It is this page's
        /// responsibility to either show the payment controls inline, or redirect
        /// to an external page to handle the payment details
        /// </summary>
        /// <param name="paymentMethodId">The payment method identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">methodId</exception>
        public virtual string GetPaymentMethodPageUrl(string paymentMethodId)
        {
            if (paymentMethodId == null) throw new ArgumentNullException("paymentMethodId");
            var page = GetPaymentContentPageByMethodId(paymentMethodId);
            if (page != null)
            {
                var url = _urlResolver.GetUrl(page.ContentLink);
                return url;
            }
            return null;
        }

        public bool PaymentMethodRequiresSocialSecurityNumber(PaymentSelection paymentSelection)
        {
            // TODO: Needs to check against known payment methods that require SSN, like Klarna
            return false;
            //return paymentSelection.SelectedPayment == new Guid("8dca4a96-a5bb-4e85-82a4-2754f04c2117") ||
            //       paymentSelection.SelectedPayment == new Guid("c2ea88f8-c702-4331-819e-0e77e7ac5450");
        }

    }
}