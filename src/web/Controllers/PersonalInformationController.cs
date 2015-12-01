﻿/*
Commerce Starter Kit for EPiServer

All rights reserved. See LICENSE.txt in project root.

Copyright (C) 2013-2014 Oxx AS
Copyright (C) 2013-2014 BV Network AS

*/

using System.Web.Mvc;
using EPiServer.ServiceLocation;
using EPiServer.Core;
using Mediachase.Commerce;
using OxxCommerceStarterKit.Core.Objects;
using OxxCommerceStarterKit.Core.Repositories;
using OxxCommerceStarterKit.Web.Business;
using OxxCommerceStarterKit.Web.Models.PageTypes;
using OxxCommerceStarterKit.Web.Models.ViewModels;
using System.Web.Security;
using EPiServer.Web.Routing;

namespace OxxCommerceStarterKit.Web.Controllers
{
	public class PersonalInformationController : PageControllerBase<PersonalInformationPage>
	{
        private readonly UrlResolver _urlResolver;

        [RequireSSL]
		public ActionResult Index(PersonalInformationPage currentPage)
		{
			PersonalInformationViewModel model = new PersonalInformationViewModel(currentPage);

			if (Request.IsAjaxRequest())
			{
				return PartialView(model);
			}

			return View(model);
		}

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect(_urlResolver.GetUrl(ContentReference.StartPage));
        }


        [HttpPost]
		[RequireSSL]
		public ActionResult Index(PersonalInformationPage currentPage, PersonalSettingsForm personalSettingsForm)
        {
            PersonalInformationViewModel model = new PersonalInformationViewModel(currentPage);
			var currentMarket = ServiceLocator.Current.GetInstance<ICurrentMarket>();

            model.PersonalSettingsForm = personalSettingsForm;


            ContactRepository contactRepository = new ContactRepository();
            contactRepository.Save(model.PersonalSettingsForm.ContactInformation);

            CustomerAddressRepository addressRepository = new CustomerAddressRepository();
			
			personalSettingsForm.BillingAddress.CheckAndSetCountryCode();
			personalSettingsForm.BillingAddress.IsPreferredBillingAddress = true;
            addressRepository.Save(personalSettingsForm.BillingAddress);

			personalSettingsForm.ShippingAddress.CheckAndSetCountryCode();
			personalSettingsForm.ShippingAddress.IsPreferredShippingAddress = true;
            addressRepository.Save(personalSettingsForm.ShippingAddress);

            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }


            //Save data
            return View(model);
        }

	    public PersonalInformationController(UrlResolver urlResolver)
	    {
	        _urlResolver = urlResolver;
	    }
    }
}
