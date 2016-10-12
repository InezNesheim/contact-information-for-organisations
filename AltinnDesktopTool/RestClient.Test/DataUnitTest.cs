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

        private const string perscontacts =
@"
{
	""_links"": {
		""self"": {
			""href"": ""https://tt02.altinn.basefarm.net/api/serviceowner/organizations/910021451/personalcontacts""
		}
	},
	""_embedded"": {
		""personalcontacts"": [{
			""PersonalContactId"": ""r50022994"",
			""Name"": ""ROLF BJØRN               "",
			""SocialSecurityNumber"": ""06117701547"",
			""MobileNumber"": ""47419641"",
			""MobileNumberChanged"": ""2016-10-11T08:15:33.987"",
			""EMailAddress"": ""erlend.oksvoll@brreg.no"",
			""EMailAddressChanged"": ""2016-10-11T08:15:33.987"",
			""_links"": {
				""roles"": {
					""href"": ""https://tt02.altinn.basefarm.net/api/serviceowner/organizations/910021451/personalcontacts/r50022994/roles""

                }
			}
		},
		{
			""PersonalContactId"": ""r50041943"",
			""Name"": ""DRAGE TARALD"",
			""SocialSecurityNumber"": ""11106700992"",
			""MobileNumber"": ""98008410"",
			""MobileNumberChanged"": ""2016-06-22T14:17:11.23"",
			""EMailAddress"": ""aen@brreg.no"",
			""EMailAddressChanged"": ""2016-06-22T14:17:11.23"",
			""_links"": {
				""roles"": {
					""href"": ""https://tt02.altinn.basefarm.net/api/serviceowner/organizations/910021451/personalcontacts/r50041943/roles""
				}
			}
		}]
	}
}
";

        private const string officialcontacts =
@"
{
	""_links"": {
		""self"": {
			""href"": ""https://tt02.altinn.basefarm.net/api/serviceowner/organizations/910021451/officialcontacts""
		}
	},
	""_embedded"": {
		""officialcontacts"": [{
			""MobileNumber"": ""12121313"",
			""MobileNumberChanged"": ""2016-10-11T08:15:33.987"",
			""EMailAddress"": ""petter@gmail.com"",
			""EMailAddressChanged"": null
        },
        {
			""MobileNumber"": ""12121414"",
			""MobileNumberChanged"": ""2016-03-21T02:30:00"",
			""EMailAddress"": ""pål@gmail.com"",
			""EMailAddressChanged"": ""2016-03-21T02:32:25""
        }]
	}
}
";


        [TestMethod]
        public void OrganizationsSerializer_Test()
        {
            //var result = Util.Deserializer.DeserializeOrganizations(organizations);
            var result = Deserializer.DeserializeHalJsonResourceList<Organization>(organizations);
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void OrganizationSerializer_Test()
        {
            //var result = Util.Deserializer.DeserializeOrganizations(organizations);
            var result = Deserializer.DeserializeHalJsonResource<Organization>(orgdata);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PersonalContactsSerializer_Test()
        {
            //var result = Util.Deserializer.DeserializeOrganizations(organizations);
            var result = Deserializer.DeserializeHalJsonResourceList<PersonalContact>(perscontacts);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void OfficialContactsSerializer_Test()
        {
            //var result = Util.Deserializer.DeserializeOrganizations(organizations);
            var result = Deserializer.DeserializeHalJsonResourceList<OfficialContact>(officialcontacts);
            Assert.AreEqual(2, result.Count);
        }
    }
}
