using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestClient;
using RestClient.DTO;
using RestClient.Util;

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

        private const string organizations =
@"{
	""_links"": {
		""self"": {
			""href"": ""https://tt02.altinn.basefarm.net/api/serviceowner/organizations""
		}
	},
	""_embedded"": {
		""organizations"": [{
			""Name"": ""Ikke i Altinn register"",
			""OrganizationNumber"": ""00210    "",
			""Type"": null,
			""LastChanged"": null,
			""LastConfirmed"": null,
			""_links"": {
				""self"": {
					""href"": ""https://tt02.altinn.basefarm.net/api/serviceowner/organizations/00210    ""

                },
				""personalcontacts"": {
					""href"": ""https://tt02.altinn.basefarm.net/api/serviceowner/organizations/00210    /personalcontacts""
				},
				""officialcontacts"": {
					""href"": ""https://tt02.altinn.basefarm.net/api/serviceowner/organizations/00210    /officialcontacts""
				}
			}
		},
		{
			""Name"": ""SKD TEST DLS 004"",
			""OrganizationNumber"": ""007641060"",
			""Type"": ""KS"",
			""LastChanged"": ""2012-03-08T00:00:00"",
			""LastConfirmed"": null,
			""_links"": {
				""self"": {
					""href"": ""https://tt02.altinn.basefarm.net/api/serviceowner/organizations/007641060""
				},
				""personalcontacts"": {
					""href"": ""https://tt02.altinn.basefarm.net/api/serviceowner/organizations/007641060/personalcontacts""
				},
				""officialcontacts"": {
					""href"": ""https://tt02.altinn.basefarm.net/api/serviceowner/organizations/007641060/officialcontacts""
				}
			}
		},
		{
			""Name"": ""SKD TEST DLS 005"",
			""OrganizationNumber"": ""007978863"",
			""Type"": ""KTRF"",
			""LastChanged"": ""2012-03-08T00:00:00"",
			""LastConfirmed"": null,
			""_links"": {
				""self"": {
					""href"": ""https://tt02.altinn.basefarm.net/api/serviceowner/organizations/007978863""
				},
				""personalcontacts"": {
					""href"": ""https://tt02.altinn.basefarm.net/api/serviceowner/organizations/007978863/personalcontacts""
				},
				""officialcontacts"": {
					""href"": ""https://tt02.altinn.basefarm.net/api/serviceowner/organizations/007978863/officialcontacts""
				}
			}
		}
    ]
    }
    }
";

        [TestMethod]
        public void Organization_Test ()
        {
            Organization org = OrganizationQuery.ParseJson<Organization>(orgdata);
            Console.WriteLine(org.Name);
        }

        [TestMethod]
        public void OrganizationSerializer_Test()
        {
            //var result = Util.Deserializer.DeserializeOrganizations(organizations);
            var result = Deserializer.DeserializeHalJsonResource<Organization>(organizations);
            ;
        }
    }
}
