using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestClient.DTO;


namespace RestClient.Test
{
    [TestClass]
    public class RestQueryTest
    {
        IRestQueryConfig config = new ConfigForTest()
        {
            BaseAddress = "https://tt02.altinn.basefarm.net/api/serviceowner",
            ApiKey = "7FB6140D-B194-4BF6-B3C8-257094FBF8C4",
            ThumbPrint = "5d15d6e888632370e0223b779c4e0f0d9d45ded0",
        };

        [TestMethod]
        [DataRow("910021451")]
        public void GetOrganizationByOrgno_Test(string orgno)
        {
            IRestQuery query = new RestQuery(config);
            var org = query.Get<Organization>(orgno);
            Assert.IsNotNull(org);
            Assert.IsTrue(!string.IsNullOrEmpty(org.Name));
        }


        [TestMethod]
        [DataRow("eok@brreg.no")]
        [DataRow("erlend.oksvoll@brreg.no")]
        [DataRow("aen@brreg.no")]

        public void GetOrgnizationsByEmail_Test(string email)
        {
            IRestQuery query = new RestQuery(config);
            var orglist = query.Get<Organization>(new System.Collections.Generic.KeyValuePair<string, string>("email", email));
            Assert.IsNotNull(orglist);
            Assert.IsTrue(orglist.Count > 0 && !string.IsNullOrEmpty(orglist[0].Name));
        }


        [TestMethod]
        [DataRow("https://tt02.altinn.basefarm.net/api/serviceowner/organizations/910021451/personalcontacts")]
        public void GetPersonalContacts_Test(string link)
        {
            IRestQuery query = new RestQuery(config);
            var list = query.GetByLink<PersonalContact>(link);
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);
        }


        [TestMethod]
        [DataRow("https://tt02.altinn.basefarm.net/api/serviceowner/organizations/910021451/officialcontacts")]
        public void GetOfficialContacts_Test(string link)
        {
            IRestQuery query = new RestQuery(config);
            var list = query.GetByLink<OfficialContact>(link);
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);
        }


    }


    public class ConfigForTest : IRestQueryConfig
    {
        public string ApiKey { get; set; }

        public string BaseAddress { get; set; }

        public string ThumbPrint { get; set; }

        public int Timeout { get; set; }
    }

}
