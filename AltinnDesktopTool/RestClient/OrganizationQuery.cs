using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestClient.DTO;

namespace RestClient
{
    public class OrganizationQuery
    {
        //private string _baseUri = "api/serviceowner/organizations";

        private string _AuthenticateUri = "api/serviceowner/organizations?ForceEIAuthentication";
        private string _GetOrganizationByOrgnoUri = "api/serviceowner/organizations/{0}";
        private string _GetOrganizationsByPhoneOrEmailUri = "api/serviceowner/organizations?{0}={1}$top={2}$skip={3}";
        private string _GetOfficialContacts = "api/serviceowner/organizations/{0}/officialcontacts";
        private string _GetPersonalContacts = "api/serviceowner/organizations/{0}/personalcontacts";

        private string _lasturi = null; // used for paging
        private string _email;
        private string _phone;        

        private bool _isAuthenticated = false;

        public AltinnRestClient RestClient { get; set; }
        public int PageSize { get; set; }
        public string LastUri { get { return _lasturi; } }

        public OrganizationQuery()
        {
            RestClient = new AltinnRestClient();
        }

        public OrganizationsResult GetOrganizations(string email, string phone = null)
        {
            EnsureAuthenticated();
            _email = email;
            _phone = phone;
            return GetOrganizations(0);
        }

        public OrganizationsResult GetOrganizations(int pageno)
        {
            int skip = PageSize * pageno;

            if (!string.IsNullOrEmpty(_email))
                _lasturi = string.Format(_GetOrganizationsByPhoneOrEmailUri, "email", _email, PageSize, skip);
            else
                _lasturi = string.Format(_GetOrganizationsByPhoneOrEmailUri, "phone", _phone, PageSize, skip);

            string json = RestClient.Get(_lasturi);

            Console.WriteLine(json);

            return ParseJson<OrganizationsResult>(json);
        }


        public Organization GetOrganization(string orgno)
        {
            EnsureAuthenticated();
            string json = null;
            if (orgno != null)
            {
                _lasturi = string.Format(_GetOrganizationByOrgnoUri, orgno);
                json = RestClient.Get(_lasturi);
                Console.WriteLine(json);
            }
            return ParseJson<Organization>(json);
        }


        public OfficialContactsResult GetOfficialContacts(string orgno)
        {
            EnsureAuthenticated();
            string json = null;
            if (orgno != null)
            {
                _lasturi = string.Format(_GetOfficialContacts, orgno);
                json = RestClient.Get(_lasturi);
                Console.WriteLine(json);
            }
            return ParseJson<OfficialContactsResult>(json);
        }

        public PersonalContactsResult GetPersonalContacts(string orgno)
        {
            EnsureAuthenticated();
            string json = null;
            if (orgno != null)
            {
                _lasturi = string.Format(_GetPersonalContacts, orgno);
                json = RestClient.Get(_lasturi);
                Console.WriteLine(json);
            }
            return ParseJson<PersonalContactsResult>(json);
        }


        /*
        public string TestTest()
        {
            EnsureAuthenticated();
            string json = RestClient.Get(_baseUri);
            Console.WriteLine(json);
            return json;
        }
        */


        public static T ParseJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }


        private void EnsureAuthenticated()
        {
            if (!_isAuthenticated)
            {
                try
                {
                    RestClient.Get(_AuthenticateUri);
                }
                catch
                {
                }
                _isAuthenticated = true;
            }

        }

    }

}
