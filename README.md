# Commerce Starterkit
An EPiServer Commerce project you can use for demos, learning from and possibly base your next web site on.

![Start Page](https://raw.githubusercontent.com/BVNetwork/CommerceStarterKit/master/doc/img/screenshots/readme-start-page.png)

## Getting Started
1. Clone the repository
2. Download the databases from [Dropbox](https://dl.dropboxusercontent.com/u/3403147/2015-11-09-epicphoto.zip) (temporary location for now) 
3. Unzip the databases to the /db/ folder
4. Open the solution in Visual Studio 2013
5. Run it (also see [How to Start Commerce Manager](#how-to-start-commerce-manager))
6. Default admin account for web and manager sites is: **admin \ st0re**
7. Configure dependencies for more features 

**Note!** In case you update the source and want to keep your existing database, please read the section on [how to upgrade the CMS and Commerce databases](#how-to-upgrade-the-databases).

## Configuring Dependencies
The Commerce Starter Kit has these dependencies:

### EPiServer Find
EPiServer Find is used in most product lists, as the wine and fashion lists, and also by the configurable search block. The main search and the search-as-you-type function also uses Find.

You will have a hard time using the starter kit without Find.

**Configuration**

1. Go to [http://find.episerver.com](http://find.episerver.com), log in, and create a new developer index (with English and Norwegian languages).
2. Add the `<episerver.find ... >` configuration to web.config. Search for it, it is already in the web.config, but with invalid configuration settings. 
2. Go to [http://localhost:49883/episerver/CMS/Admin/Default.aspx](http://localhost:49883/episerver/CMS/Admin/Default.aspx "Admin mode"), find the "Index Product Catalog" Scheduled Job and run it.
3. Verify that the product list is showing correctly by visiting the Wine list (http://localhost:49883/en/wine/all-wines/)

### DIBS
The payment step of the checkout requires a DIBS demo account. If your company does not already have a demo account with DIBS, you need to sign up for one: [http://www.dibspayment.com/demo-signup](http://www.dibspayment.com/demo-signup) The starter kit is using the [DIBS D2 platform](http://tech.dibspayment.com/dibs_payment_window).

You need the following settings from your DIBS account:

* API UserId (this is your merchant ID)
* HMAC key for MAC calculation (K)

Configure payment settings:

1. Log in to Commerce Manager
2. Open the Administration menu
3. Expand Order System / Payments / English (repeat for "norsk")
4. Select Parameters tab
5. Configure the settings according to the values above

**NOTE!** You need to configure the DIBS account for both English and "norsk" in Commerce Manager. 

This can be found on the [DIBS administration web site](https://payment.architrade.com/login/login.action)

You can find a [list of test cards](http://tech.dibspayment.com/toolbox/test_information_cards) on the DIBS web site. Use these to test the checkout process.

### Postnord
For the Norwegian market, you can select a pickup location for the order. This is based on the shipping address. The availble pick up locations are retrieved by calling a public Postnord web service.

[Register for Web Service Access](http://www.postnordlogistics.no/en/e-services/widgets-and-web-services/Pages/Register-as-webservice-widget-consumer.aspx) on the Postnord web site.

Add your consumerId to the `PostNord.ConsumerId` appSettings value in `web.config`:

    <appSettings>
    	<add key="PostNord.ConsumerId" value="your-consumer-id-here" />
    <appSettings>

Without the Postnord integration, you should still be able to do a checkout, but you will not be able to demonstrate the pickup location feature.

**Note!** The Postnord feature will be removed from the source in the future.

### Google Analytics
The starter kit has extended ecommerce tracking, which will start tracking when you configure the EPiServer Google Analytics Add-on.

Make sure your Google Analytics account is using Universal Analytics and has the Enhanced Ecommerce feature enabled.

By default, the following is tracked:

* Page views
* Add to cart
* View cart
* Checkout
* Payment
* Configurable Search Blocks 

# Nice to Know
Various helpful stuff

## How to Start Commerce Manager
When you start the project in Visual Studio, it will only start the the default site (the designated Startup Project). This is typically the web site and not Commerce Manager.

To start Commerce Manager, right click the CommerceStarterKit.Commerce project in Visual Studio, View, View in Browser (or Ctrl+Shift+W)

## Hot to start the B2B Site
There is a B2B site included, which is set up to run on localhost:49900 in IIS Express by default. Your local IIS Express (the one that runs when you start the project in Visual Studio) will not know how to resolve this addess. You can add this binding yourself by editing the applicationhost.config file for IIS Express.

If you use Visual Studio 2015, open `SoltutionDir\.vs\config\applicationhost.config`

If you use Visual Studio 2013, open `%USERPROFILE%\My Documents\IISExpress\config\applicationhost.config`

Find a section that looks like this and add the second binding as shown below:
```xml
<site name="CommerceStarterKit.Web" id="3">
    <application path="/" applicationPool="Clr4IntegratedAppPool">
        <virtualDirectory path="/" physicalPath="...\src\web" />
    </application>
    <bindings>
        <binding protocol="http" bindingInformation="*:49883:localhost" />
        <!-- The B2B site -->
        <binding protocol="http" bindingInformation="*:49900:localhost" />
    </bindings>
</site>
```
You need to restart IIS Express after the change.

## How to Upgrade the Databases
Now and then the CMS and Commerce databases need to be upgraded. This is typically done using the `update-epidatabase` command in the Package Manager Console in Visual Studio. 

The latest version of the code is synced with the downloadable databases. If you haven't changed any content (that you'd care to keep), just download new versions and replace the databases you've got.

If you update these databases yourself using the `update-epidatabase` command
from the Package Manager Console, you need to change the connectionstring to
a full path instead of the "magic" one using the DataDirectory setting:

	Data Source=(LocalDb)\v11.0;AttachDbFilename=|DataDirectory|CommerceStarterKit-Web.mdf; ... 

The `update-epidatabase` command will read the `connectionstrings.config` file and try to locate the databases in the `app_Data` directory below your web site. Since the DataDirectory is changed in memory during the startup of the web sites, they know how to find the databases. This is not the case for the `update-epidatabase` command.

Change both AttachDbFilename settings:

	AttachDbFilename=c:\path\to\your\CommerceStarterKit-Web.mdf and
	AttachDbFilename=c:\path\to\your\CommerceStarterKit-CM.mdf

After you have run the `update-epidatabase` you can revert the connectionstring changes.

