using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestClient.DTO;
using System.Reflection;

namespace RestClient
{

    public class RestQueryStub : IRestQuery
    {
        private static PropertyInfo Prop_Org_Name = typeof(Organization).GetProperty("Name");
        private static PropertyInfo Prop_Org_LastChanged = typeof(Organization).GetProperty("LastChanged");
        private static PropertyInfo Prop_Org_Type = typeof(Organization).GetProperty("Type");
        private static PropertyInfo Prop_Org_Official_Contact = typeof(Organization).GetProperty("OfficialContactsLink");
        private static PropertyInfo Prop_Org_Personal_Contact = typeof(Organization).GetProperty("PersonalContactsLink");
        private static PropertyInfo Prop_Org_OrganizationNumber = typeof(Organization).GetProperty("OrganizationNumber");

        private static PropertyInfo PropOffCont_Email = typeof(OfficialContact).GetProperty("EmailAddress");
        private static PropertyInfo PropOffCont_Email_Changed = typeof(OfficialContact).GetProperty("EmailAddressChanged");
        private static PropertyInfo PropOffCont_Phone = typeof(OfficialContact).GetProperty("MobileNumber");
        private static PropertyInfo PropOffCont_Phone_Changed = typeof(OfficialContact).GetProperty("MobileNumberChanged");

        private static PropertyInfo PropPersCont_Email = typeof(PersonalContact).GetProperty("EmailAddress");
        private static PropertyInfo PropPersCont_Email_Changed = typeof(PersonalContact).GetProperty("EmailAddressChanged");
        private static PropertyInfo PropPersCont_Phone = typeof(PersonalContact).GetProperty("MobileNumber");
        private static PropertyInfo PropPersCont_Phone_Changed = typeof(PersonalContact).GetProperty("MobileNumberChanged");
        private static PropertyInfo PropPersCont_PersonalContactId = typeof(PersonalContact).GetProperty("PersonalContactId");
        private static PropertyInfo PropPersCont_Name = typeof(PersonalContact).GetProperty("Name");
        private static PropertyInfo PropPersCont_SocialSecurityNumber = typeof(PersonalContact).GetProperty("SocialSecurityNumber");


        public RestQueryStub()
        {
        }


        /// <summary>
        /// Supports only Organization in this Stub
        /// </summary>
        /// <typeparam name="T">Must be organization</typeparam>
        /// <param name="id">Organization Number</param>
        /// <returns></returns>
        public T Get<T>(string id)
        {
            T org = Activator.CreateInstance<T>();

            if (id == "070238225")
                CreateOrg1(org);
            else if (id == "010007690")
                CreateOrg2(org);
            else if (id == "010007763")
                CreateOrg3(org);
            else if (id == "010007828")
                CreateOrg4(org);

            return org;
        }


        /// <summary>
        /// Returns a list of organizations
        /// </summary>
        /// <typeparam name="T">Must be Organization</typeparam>
        /// <param name="filter"></param>
        /// <returns>List</returns>
        public IList<T> Get<T>(KeyValuePair<string, string> filter)
        {
            T org1 = Activator.CreateInstance<T>();
            T org2 = Activator.CreateInstance<T>();
            T org3 = Activator.CreateInstance<T>();
            T org4 = Activator.CreateInstance<T>();

            CreateOrg1(org1);
            CreateOrg2(org2);
            CreateOrg3(org3);
            CreateOrg4(org4);

            return new List<T>()
            {
                org1, org2, org3, org4
            };
        }

        public IList<T> GetByLink<T>(string url)
        {
            T contact1 = Activator.CreateInstance<T>();
            T contact2 = Activator.CreateInstance<T>();
            T contact3 = Activator.CreateInstance<T>();

            List<T> list = new List<T>()
            {
                contact1, contact2, contact3
            };

            if (url.Contains("official"))
            {
                CreateOffContact1(contact1);
                CreateOffContact2(contact2);
                CreateOffContact3(contact3);
            }
            else
            {
                CreatePersContact1(contact1);
                CreatePersContact2(contact2);
                CreatePersContact3(contact3);
            }

            return list;
        }



        private void SetProp(object o, string name, object value)
        {
            PropertyInfo propertyInfo = o.GetType().GetProperty(name, value.GetType());
            if (propertyInfo != null)
                propertyInfo.SetValue(o, value);
        }


        private void CreateOrg1(object org)
        {
            Prop_Org_Name.SetValue(org, "SKD TEST DLS 022");
            Prop_Org_LastChanged.SetValue(org, new DateTime(2012, 03, 08));
            Prop_Org_Type.SetValue(org, "DA");
            Prop_Org_Official_Contact.SetValue(org, "https://tt02.altinn.basefarm.net/api/serviceowner/organizations/070238225/officialcontacts");
            Prop_Org_Personal_Contact.SetValue(org, "https://tt02.altinn.basefarm.net/api/serviceowner/organizations/070238225/personalcontacts");
            Prop_Org_OrganizationNumber.SetValue(org, "070238225");
        }

        private void CreateOrg2(object org)
        {
            Prop_Org_Name.SetValue(org, "SVATSUM OG JAR");
            Prop_Org_LastChanged.SetValue(org, new DateTime(2010, 01, 01));
            Prop_Org_Type.SetValue(org, "ANS");
            Prop_Org_Official_Contact.SetValue(org, "https://tt02.altinn.basefarm.net/api/serviceowner/organizations/010007690/officialcontacts");
            Prop_Org_Personal_Contact.SetValue(org, "https://tt02.altinn.basefarm.net/api/serviceowner/organizations/010007690/personalcontacts");
            Prop_Org_OrganizationNumber.SetValue(org, "010007690");
        }

        private void CreateOrg3(object org)
        {
            Prop_Org_Name.SetValue(org, "RISDAL OG KVAMSØY");
            Prop_Org_LastChanged.SetValue(org, new DateTime(2014, 07, 01));
            Prop_Org_Type.SetValue(org, "IS");
            Prop_Org_Official_Contact.SetValue(org, "https://tt02.altinn.basefarm.net/api/serviceowner/organizations/010007763/officialcontacts");
            Prop_Org_Personal_Contact.SetValue(org, "https://tt02.altinn.basefarm.net/api/serviceowner/organizations/010007763/personalcontacts");
            Prop_Org_OrganizationNumber.SetValue(org, "010007763");
        }

        private void CreateOrg4(object org)
        {
            Prop_Org_Name.SetValue(org, "SKARTVEIT OG NITTEDAL");
            Prop_Org_LastChanged.SetValue(org, new DateTime(2015, 12, 20));
            Prop_Org_Type.SetValue(org, "IS");
            Prop_Org_Official_Contact.SetValue(org, "https://tt02.altinn.basefarm.net/api/serviceowner/organizations/010007828/officialcontacts");
            Prop_Org_Personal_Contact.SetValue(org, "https://tt02.altinn.basefarm.net/api/serviceowner/organizations/010007828/personalcontacts");
            Prop_Org_OrganizationNumber.SetValue(org, "010007828");
        }

        private void CreateOffContact1(object offcont)
        {
            PropOffCont_Email.SetValue(offcont, "petter@gmail.com");
            PropOffCont_Email_Changed.SetValue(offcont, new DateTime(2009, 06, 06));
            PropOffCont_Phone.SetValue(offcont, "12121313");
            PropOffCont_Phone_Changed.SetValue(offcont, new DateTime(2007, 12, 24));
        }

        private void CreateOffContact2(object offcont)
        {
            PropOffCont_Email.SetValue(offcont, "pål@gmail.com");
            PropOffCont_Email_Changed.SetValue(offcont, new DateTime(2014, 01, 18));
            PropOffCont_Phone.SetValue(offcont, "12121414");
            PropOffCont_Phone_Changed.SetValue(offcont, new DateTime(2012, 11, 11));
        }
        private void CreateOffContact3(object offcont)
        {
            PropOffCont_Email.SetValue(offcont, "espen@gmail.com");
            PropOffCont_Email_Changed.SetValue(offcont, new DateTime(2016, 10, 04));
            PropOffCont_Phone.SetValue(offcont, "12121515");
            PropOffCont_Phone_Changed.SetValue(offcont, new DateTime(2016, 10, 04));
        }

        private void CreatePersContact1(object cont)
        {
            PropPersCont_Email.SetValue(cont, "rolf-bjørn@gmail.com");
            PropPersCont_Email_Changed.SetValue(cont, new DateTime(2009, 06, 06));
            PropPersCont_Phone.SetValue(cont, "47419641");
            PropPersCont_Phone_Changed.SetValue(cont, new DateTime(2007, 12, 24));
            PropPersCont_PersonalContactId.SetValue(cont, "r50022994");
            PropPersCont_SocialSecurityNumber.SetValue(cont, "06117701547");
            PropPersCont_Name.SetValue(cont, "ROLF BJØRN");
        }

        private void CreatePersContact2(object cont)
        {
            PropPersCont_Email.SetValue(cont, "drage@gmail.com");
            PropPersCont_Email_Changed.SetValue(cont, new DateTime(2015, 02, 14));
            PropPersCont_Phone.SetValue(cont, "98008410");
            PropPersCont_Phone_Changed.SetValue(cont, new DateTime(2014, 10, 10));
            PropPersCont_PersonalContactId.SetValue(cont, "r50041943");
            PropPersCont_SocialSecurityNumber.SetValue(cont, "11106700992");
            PropPersCont_Name.SetValue(cont, "DRAGE TARALD");
        }
        private void CreatePersContact3(object cont)
        {
            PropPersCont_Email.SetValue(cont, "donald-duck@gmail.com");
            PropPersCont_Email_Changed.SetValue(cont, new DateTime(1966, 01, 01));
            PropPersCont_Phone.SetValue(cont, "13131313");
            PropPersCont_Phone_Changed.SetValue(cont, new DateTime(1997, 01, 01));
            PropPersCont_PersonalContactId.SetValue(cont, "r13042941");
            PropPersCont_SocialSecurityNumber.SetValue(cont, "06128801558");
            PropPersCont_Name.SetValue(cont, "DONALD DUCK TRUMP");
        }
    }
}
