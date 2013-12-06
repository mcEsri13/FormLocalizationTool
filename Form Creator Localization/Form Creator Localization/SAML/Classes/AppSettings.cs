namespace URLRedirectAPI.Classes.SAML
{
    public class AppSettings
    {
        public string AssertionConsumerServiceUrl { get; set; }
        public string Issuer { get; set; }

        public const string AssertionConsumerServiceUrlKey = "AssertionConsumerUrl";

        public const string IssuerKey = "Issuer";

        public AppSettings(string assertionConsumerServiceURL, string issuer)
        {
            AssertionConsumerServiceUrl = assertionConsumerServiceURL;
            Issuer = issuer;
        }
    }
}
