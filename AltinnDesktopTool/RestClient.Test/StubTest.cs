using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestClient;
using RestClient.DTO;

namespace RestClient.Test
{
    [TestClass]
    public class StubTest
    {
        [TestMethod]
        public void OrganizationSearchStub_Test()
        {
            IRestQuery query = new RestQueryStub();
            var list = query.Get<Organization>(new System.Collections.Generic.KeyValuePair<string, string>("email", "pål@gmail.com"));
            Assert.IsTrue(list.Count == 4, "Organization stub failed");
        }

        [TestMethod]
        public void OrganizationGetStub_Test()
        {
            IRestQuery query = new RestQueryStub();
            var org = query.Get<Organization>("070238225");
            Assert.IsTrue(org != null && org.OrganizationNumber == "070238225", "GetOrganization Stub by Id fails");
        }

        [TestMethod]
        public void OfficialContactStub_Test()
        {
            IRestQuery query = new RestQueryStub();
            var list = query.GetByLink<OfficialContact>("https://tt02.altinn.basefarm.net/api/serviceowner/organizations/070238225/officialcontacts");
            Assert.IsTrue(list.Count == 3, "Official Contact stub failed");
        }

        [TestMethod]
        public void PersonalContactStub_Test()
        {
            IRestQuery query = new RestQueryStub();
            var list = query.GetByLink<PersonalContact>("https://tt02.altinn.basefarm.net/api/serviceowner/organizations/070238225/personalcontacts");
            Assert.IsTrue(list.Count == 3, "Personal Contact stub failed");
        }

    }

}
