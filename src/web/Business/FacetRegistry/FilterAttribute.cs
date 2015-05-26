using System;
using Castle.DynamicProxy;
using EPiServer.BaseLibrary;
using EPiServer.Framework.Localization;
using ArgumentException = System.ArgumentException;

namespace OxxCommerceStarterKit.Web.Business.FacetRegistry
{
    [AttributeUsage(validOn:AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FilterAttribute : Attribute
    {
        private string _displayName;

        public virtual string PropertyName { get; set; }
        public virtual string LanguageKey { get; set; }
        
        public virtual string DisplayName
        {
            get
            {
                if(string.IsNullOrEmpty(_displayName))
                {
                    // Get from language file first
                    if (string.IsNullOrEmpty(LanguageKey) == false)
                    {
                        _displayName = LocalizationService.Current.GetString(LanguageKey);
                    }

                    // Fall back to name of property
                    if(string.IsNullOrEmpty(PropertyName))
                    {
                        throw new ArgumentException("Neither DisplayName, LanguageKey or PropertyName has been set.");
                    }

                    _displayName = PropertyName;
                }
                return _displayName;
            }
            set { _displayName = value; }
        }

    }
}