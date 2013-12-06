namespace URLRedirectAPI.Classes.SAML
{
    public class AccountSettings
    {
        public string Certificate { get; set; }
        public string IdpSsoTargetUrl { get; set; }


        public const string IdpSsoTargetUrlKey = "IdpSsoTargetUrl";

        public const string CertificateKey = "OktaCertificate";

        public AccountSettings(string certificate, string idp_sso_target_url)
        {
            Certificate = certificate;
            IdpSsoTargetUrl = idp_sso_target_url;
        }
    }
}
