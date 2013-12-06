using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Security.Cryptography.Xml;
using System.IO;
using System.Text.RegularExpressions;

namespace URLRedirectAPI.Classes.SAML
{

    class SamlSignedXml : SignedXml
    {
        private XmlNamespaceManager xmlNamespaceManager;

        public SamlSignedXml(XmlElement xmlElement, XmlNamespaceManager xmlNamespaceManager)
            : base(xmlElement)
        {
            this.xmlNamespaceManager = xmlNamespaceManager;
        }

        public override XmlElement GetIdElement(XmlDocument xmlDocument, string idValue)
        {
            //Console.WriteLine("//samlp:Response[@ID='{0}']", idValue);
            XmlElement xmlElement = (XmlElement)xmlDocument.SelectSingleNode(string.Format("//samlp:Response[@ID='{0}']", idValue), this.xmlNamespaceManager);
            if(xmlElement == null)
                xmlElement = (XmlElement)xmlDocument.SelectSingleNode(string.Format("//saml:Assertion[@ID='{0}']", idValue), this.xmlNamespaceManager);
            return xmlElement;
        }
    }

    public class Certificate
    {
        public X509Certificate2 cert;

        public void LoadCertificate(string certificate)
        {
            cert = new X509Certificate2();
            cert.Import(StringToByteArray(certificate));
        }

        public void LoadCertificate(byte[] certificate)
        {
            cert = new X509Certificate2();
            cert.Import(certificate);
        }

        private byte[] StringToByteArray(string st)
        {
            byte[] bytes = new byte[st.Length];
            for (int i = 0; i < st.Length; i++)
            {
                bytes[i] = (byte)st[i];
            }
            return bytes;
        }
    }


    public class Response
    {
        private XmlDocument xmlDoc;
        private XmlNamespaceManager xmlmanager;
        private AccountSettings accountSettings;
        private Certificate certificate;

        public Response(AccountSettings accountSettings)
        {
            this.accountSettings = accountSettings;
            certificate = new Certificate();
            certificate.LoadCertificate(accountSettings.Certificate);
        }

        public void LoadXml(string xml)
        {

            xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.LoadXml(xml);
            LoadXmlManager();
        }

        public void LoadXmlFromBase64(string response)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            LoadXml(enc.GetString(Convert.FromBase64String(response)));
        }

        public bool IsValid()
        {
            bool status = false;
            XmlNodeList nodeList = xmlDoc.SelectNodes("//ds:Signature", xmlmanager);

            SamlSignedXml signedXml = new SamlSignedXml(xmlDoc.DocumentElement, xmlmanager);
           
            foreach (XmlNode node in nodeList)
            {
                signedXml.LoadXml((XmlElement)node);
                status = signedXml.CheckSignature(certificate.cert, true);
                if (!status)
                    return false;
            }
            return status;
        }

        public string GetNameID()
        {
            XmlNode node = xmlDoc.SelectSingleNode("/samlp:Response/saml:Assertion/saml:Subject/saml:NameID", xmlmanager);
            return node.InnerText;
        }


        public User GetUser()
        {
            string nodeValue;
            XmlNode childNode;
            User myUser = new User(GetNameID());
            XmlNodeList nodes = xmlDoc.SelectNodes("/samlp:Response/saml:Assertion/saml:AttributeStatement/saml:Attribute", xmlmanager);

            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    if (node.HasChildNodes &&
                        node.Attributes != null &&
                        node.Attributes["Name"] != null)
                    {
                        if (string.Equals(node.Attributes["Name"].Value, "groups", StringComparison.OrdinalIgnoreCase) == true)
                        {
                            for (int i = 0; i < node.ChildNodes.Count; i++)
                            {
                                nodeValue = node.ChildNodes[i].InnerText;
                                if(string.IsNullOrEmpty(nodeValue) == false)
                                {
                                    myUser.GroupsList.Add(nodeValue, string.Empty);
                                }                             
                            }
                        }
                        else
                        {
                            childNode = node.FirstChild as XmlNode;
                            nodeValue = childNode.InnerText;
                            if (string.IsNullOrEmpty(nodeValue) == false)
                            {
                                myUser.CustomAttributes.Add(node.Attributes["Name"].Value, nodeValue);
                            }
                        }
                    }

                }              
            }

            // check if Employee Number is an Contractor and change starting 9 to 'a' in the value.
            string employeeNumber;
            Regex contractorRegEx = new Regex(@"(?<contractorIdentifier>^9)(?<EmpID>[\d]{4}$)"); 
            myUser.CustomAttributes.TryGetValue("employeeNumber",out employeeNumber);
            if ( employeeNumber!= null &&  contractorRegEx.IsMatch(employeeNumber))
            {
                myUser.CustomAttributes["employeeNumber"] = contractorRegEx.Replace(employeeNumber, "a${EmpID}");
            }

            return myUser;
        }

        private XmlNamespaceManager LoadXmlManager()
        {
            xmlmanager = new XmlNamespaceManager(xmlDoc.NameTable);
            xmlmanager.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);
            xmlmanager.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
            xmlmanager.AddNamespace("samlp", "urn:oasis:names:tc:SAML:2.0:protocol");
            return xmlmanager;
        }
    }




    public class AuthRequest
    {
        public string id;
        private string issue_instant;
        private AppSettings appSettings;
        private AccountSettings accountSettings;

        public enum AuthRequestFormat
        {
            Base64 = 1
        }

        public AuthRequest(AppSettings appSettings, AccountSettings accountSettings)
        {
            this.appSettings = appSettings;
            this.accountSettings = accountSettings;

            id = "_" + System.Guid.NewGuid().ToString();
            issue_instant = DateTime.Now.ToUniversalTime().ToString("yyyy-mm-ddTH:mm:ssZ");
        }

        public string GetRequest(AuthRequestFormat format)
        {
            using (StringWriter sw = new StringWriter())
            {
                XmlWriterSettings xws = new XmlWriterSettings();
                xws.OmitXmlDeclaration = true;

                using (XmlWriter xw = XmlWriter.Create(sw, xws))
                {
                    xw.WriteStartElement("samlp", "AuthnRequest", "urn:oasis:names:tc:SAML:2.0:protocol");
                    xw.WriteAttributeString("ID", id);
                    xw.WriteAttributeString("Version", "2.0");
                    xw.WriteAttributeString("IssueInstant", issue_instant);
                    xw.WriteAttributeString("ProtocolBinding", "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST");
                    xw.WriteAttributeString("AssertionConsumerServiceURL", appSettings.AssertionConsumerServiceUrl);

                    xw.WriteStartElement("saml", "Issuer", "urn:oasis:names:tc:SAML:2.0:assertion");
                    xw.WriteString(appSettings.Issuer);
                    xw.WriteEndElement();

                    xw.WriteStartElement("samlp", "NameIDPolicy", "urn:oasis:names:tc:SAML:2.0:protocol");
                    xw.WriteAttributeString("Format", "urn:oasis:names:tc:SAML:2.0:nameid-format:unspecified");
                    xw.WriteAttributeString("AllowCreate", "true");
                    xw.WriteEndElement();

                    xw.WriteStartElement("samlp", "RequestedAuthnContext", "urn:oasis:names:tc:SAML:2.0:protocol");
                    xw.WriteAttributeString("Comparison", "exact");
                    xw.WriteEndElement();

                    xw.WriteStartElement("saml", "AuthnContextClassRef", "urn:oasis:names:tc:SAML:2.0:assertion");
                    xw.WriteString("urn:oasis:names:tc:SAML:2.0:ac:classes:PasswordProtectedTransport");
                    xw.WriteEndElement();

                    xw.WriteEndElement();
                }

                if (format == AuthRequestFormat.Base64)
                {
                    byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(sw.ToString());
                    return System.Convert.ToBase64String(toEncodeAsBytes);
                }

                return null;
            }
        }
    }
}
