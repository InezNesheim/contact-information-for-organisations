﻿using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RestClient;
using RestClient.DTO;

namespace IntegrationUnitTest
{
    /// <summary>
    /// Test class for integration tests of the RestQuery class.
    /// </summary>
    [TestClass]
    public class RestQueryTest
    {
        private readonly IRestQueryConfig config = new ConfigForTest
        {
            BaseAddress = "https://tt02.altinn.basefarm.net/api/serviceowner/",
            ApiKey = "7FB6140D-B194-4BF6-B3C8-257094FBF8C4",
            ThumbPrint = "5d15d6e888632370e0223b779c4e0f0d9d45ded0",
            IgnoreSslErrors = false
        };

        /// <summary>
        /// Scenario: 
        ///   Attempt to retrieve a specific organization based on its organization number.
        /// Expected Result: 
        ///   The organization is returned.
        /// Success Criteria: 
        ///   The organization details match the expected values.
        /// </summary>
        /// <param name="orgno">The organization number to retrieve.</param>
        [TestCategory("Integration")]
        [TestCategory("RestClient")]
        [TestMethod]
        [DataRow("910021451")]
        public void GetOrganizationByOrgnoTest(string orgno)
        {
            // Arrange
            IRestQuery query = new RestQuery(this.config);

            // Act
            Organization org = query.Get<Organization>(orgno);

            // Assert
            Assert.IsNotNull(org);
            Assert.IsTrue(!string.IsNullOrEmpty(org.Name));
        }

        /// <summary>
        /// Scenario: 
        ///   Get a list of organizations filtered by an email address.
        /// Expected Result: 
        ///   A list of organizations is returned
        /// Success Criteria: 
        ///   The list of organization have at least one element and it has a name.
        /// </summary>
        /// <param name="email">The email address to use in the search.</param>
        [TestCategory("Integration")]
        [TestCategory("RestClient")]
        [TestMethod]
        [DataRow("eok@brreg.no")]
        [DataRow("erlend.oksvoll@brreg.no")]
        [DataRow("aen@brreg.no")]
        public void GetOrgnizationsByEmailTest(string email)
        {
            // Arrange
            IRestQuery query = new RestQuery(this.config);

            // Act
            IList<Organization> orglist = query.Get<Organization>(new KeyValuePair<string, string>("email", email));

            // Assert
            Assert.IsNotNull(orglist);
            Assert.IsTrue(orglist.Count > 0 && !string.IsNullOrEmpty(orglist[0].Name));
        }

        /// <summary>
        /// Scenario: 
        ///   Attempt to retrieve all personal contacts on an organization.
        /// Expected Result: 
        ///   A list of personal contacts.
        /// Success Criteria: 
        ///   The list of personal contacts has at least one element.
        /// </summary>
        /// <param name="link">The specific resource URL to request data from.</param>
        [TestCategory("Integration")]
        [TestCategory("RestClient")]
        [TestMethod]
        [DataRow("https://tt02.altinn.basefarm.net/api/serviceowner/organizations/910021451/personalcontacts")]
        public void GetPersonalContactsTest(string link)
        {
            // Arrange
            IRestQuery query = new RestQuery(this.config);

            // Act
            IList<PersonalContact> list = query.GetByLink<PersonalContact>(link);

            // Assert
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);
        }

        /// <summary>
        /// Scenario: 
        ///   Attempt to retrieve a list of official contacts on an organization.
        /// Expected Result: 
        ///   A list of official contacts.
        /// Success Criteria: 
        ///   The list of official contacts has at least on entry.
        /// </summary>
        /// <param name="link">The specific resource URL to request data from.</param>
        [TestCategory("Integration")]
        [TestCategory("RestClient")]
        [TestMethod]
        [DataRow("https://tt02.altinn.basefarm.net/api/serviceowner/organizations/910021451/officialcontacts")]
        public void GetOfficialContactsTest(string link)
        {
            // Arrange
            IRestQuery query = new RestQuery(this.config);

            // Act
            IList<OfficialContact> list = query.GetByLink<OfficialContact>(link);

            // Assert
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);
        }
    }

    /// <summary>
    /// Class for storing environment based configuration settings used by the integration tests.
    /// </summary>
    public class ConfigForTest : IRestQueryConfig
    {
        /// <summary>
        /// Gets or sets the api key for the environment that this configuration is connected to.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the base address for the REST api for the environment that this configuration is connected to.
        /// </summary>
        public string BaseAddress { get; set; }

        /// <summary>
        /// Gets or sets the certificate thumbprint for the environment that this configuration is connected to.
        /// </summary>
        public string ThumbPrint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the client should ignore SSL errors.
        /// </summary>
        public bool IgnoreSslErrors { get; set; }

        /// <summary>
        /// Gets or sets the timeout for the REST api call for the environment that this configuration is connected to.
        /// </summary>
        public int Timeout { get; set; }
    }
}
