using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestClient.DTO;

namespace RestClient.Test
{
    [TestClass]
    public class OrganizationQuery_Test
    {

        string baseaddress = "https://tt02.altinn.basefarm.net/";
        string apikey = "7FB6140D-B194-4BF6-B3C8-257094FBF8C4";
        string thumbprint = "5d15d6e888632370e0223b779c4e0f0d9d45ded0";
        string orgno = "910021451";

        /*
        [TestMethod]
        public void GetOrganizationByOrgno_Test()
        {
            OrganizationQuery query = new OrganizationQuery();
            SetupConfiguration(query.RestClient);
            var result = query.GetOrganization(orgno);
            Console.WriteLine(result.Name);
        }

        [TestMethod]
        public void GetOrganizationsByEmail_Test()
        {
            OrganizationQuery query = new OrganizationQuery();
            query.PageSize = 50;
            SetupConfiguration(query.RestClient);
            var result = query.GetOrganizations(null, "eok@brreg.no");
            if (result._embedded != null && result._embedded.organizations != null)
                Console.WriteLine(result._embedded.organizations.Count);
        }

        [TestMethod]
        public void GetOfficialContacts_Test()
        {
            OrganizationQuery query = new OrganizationQuery();
            query.PageSize = 50;
            SetupConfiguration(query.RestClient);
            var result = query.GetOfficialContacts(orgno);
        }

        [TestMethod]
        public void GetPersonalContacts_Test()
        {
            OrganizationQuery query = new OrganizationQuery();
            query.PageSize = 50;
            SetupConfiguration(query.RestClient);
            var result = query.GetPersonalContacts(orgno);
        }
        */
        /*
        [TestMethod]
        public void TestTest()
        {
            OrganizationQuery query = new OrganizationQuery();
            query.PageSize = 50;
            SetupConfiguration(query.RestClient);
            query.TestTest();
        }
        */

        private void SetupConfiguration(AltinnRestClient restClient)
        {
            restClient.ApiKey = apikey;
            restClient.BaseAddress = baseaddress;
            restClient.Thumbprint = thumbprint;
        }

    }
}
