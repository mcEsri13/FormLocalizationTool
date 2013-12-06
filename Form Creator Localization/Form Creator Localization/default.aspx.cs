using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using URLRedirectAPI.Classes.SAML;
using URLRedirectAPI.Classes.Utilities;

namespace Form_Creator_Localization
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            OKTAAuthentication();
        }

        private void OKTAAuthentication()
        {
            //OKTA
            //Get Credentials..
            string ceritifcate = ConfigurationManager.AppSettings["OktaCertificate"];
            string targetUrl = ConfigurationManager.AppSettings["IdpSsoTargetUrl"];
            string consumerUrl = ConfigurationManager.AppSettings[AppSettings.AssertionConsumerServiceUrlKey];
            string issuer = ConfigurationManager.AppSettings["Issuer"].ToString();

            //Get Settings
            AccountSettings accountSettings = new AccountSettings(ceritifcate, targetUrl);
            AuthRequest request = new AuthRequest(new AppSettings(consumerUrl, issuer), accountSettings);

            //Set Request...
            var redirectUrl = accountSettings.IdpSsoTargetUrl + "?SAMLRequest=" + Server.UrlEncode(request.GetRequest(AuthRequest.AuthRequestFormat.Base64));
            string relayState = Utils.ParseRelayState(Request);
            redirectUrl = redirectUrl + "&RelayState=" + Server.UrlEncode(relayState);

            Response.Redirect(redirectUrl);
        }
    }
}