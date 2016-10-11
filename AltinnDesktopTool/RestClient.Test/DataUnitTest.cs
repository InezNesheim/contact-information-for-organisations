using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestClient;
using RestClient.DTO;

namespace RestClient.Test
{
    [TestClass]
    public class DataUnitTest
    {

        private const string orgdata =
@"{
	""Name"": ""sample string 60"",
	""OrganizationNumber"": ""sample string 17"",
	""Type"": ""sample string 57"",
	""LastChanged"": ""2016-10-10T09:16:30.0784402+02:00"",
	""LastConfirmed"": ""2016-10-10T09:16:30.0784402+02:00"",
	""OfficialContacts"": {
		""_links"": {
			""self"": {
				""href"": ""sample string 27""
			}
		},
	},
	""_links"": {
		""self"": {
			""href"": ""sample string 28""
		}
	}
}
";

        [TestMethod]
        public void Organization_Test ()
        {
            Organization org = OrganizationQuery.ParseJson<Organization>(orgdata);
            Console.WriteLine(org.Name);
        }
    }
}
