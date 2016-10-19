using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestClient;

namespace IntegrationUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private const string Baseaddress = "https://tt02.altinn.basefarm.net/";

        private const string Apikey = "7FB6140D-B194-4BF6-B3C8-257094FBF8C4";

        private const string Thumbprint = "5d15d6e888632370e0223b779c4e0f0d9d45ded0";

        [TestMethod]
        public void GetClientTest()
        {
            var client = new AltinnRestClient(UnitTest1.Baseaddress, Apikey, UnitTest1.Thumbprint);

            // Authenticate
            // NOTE: Altinn returns 401 even if it is validated.
            const string Orgno = "910021451";
            var uriPart = "api/serviceowner/organizations?ForceEIAuthentication";
            try
            {
                client.Get(uriPart);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Get by orgno
            uriPart = "api/serviceowner/organizations/" + Orgno;
            var result = "N/A";
            try
            {
                result = client.Get(uriPart);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine(result);
        }
    }
}
