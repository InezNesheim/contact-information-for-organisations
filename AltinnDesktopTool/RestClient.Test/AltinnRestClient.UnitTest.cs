using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace RestClient.Test
{
    [TestClass]
    public class UnitTest1
    {
        string baseaddress = "https://tt02.altinn.basefarm.net/";
        string apikey = "7FB6140D-B194-4BF6-B3C8-257094FBF8C4";
        string thumbprint = "5d15d6e888632370e0223b779c4e0f0d9d45ded0";

        [TestMethod]
        public void GetClient_Test()
        {
            AltinnRestClient client = new AltinnRestClient();

            client.BaseAddress = baseaddress;
            client.ApiKey = apikey;
            client.Thumbprint = thumbprint;

            // Authenticate
            // NOTE: Altinn returns 401 even if it is validated.
            string orgno = "910021451";
            string uriPart = "api/serviceowner/organizations?ForceEIAuthentication";
            try
            {
                client.Get(uriPart);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Get by orgno
            uriPart = "api/serviceowner/organizations/" + orgno;
            string result = "N/A";
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
