﻿using System;
using System.Linq;
using System.Net.Http;
using FluentAssertions;
using LiberisLabs.CompaniesHouse.Tests.ResourceBuilders;
using LiberisLabs.CompaniesHouse.UriBuilders;
using Moq;
using NUnit.Framework;
using CompanyProfile = LiberisLabs.CompaniesHouse.Response.CompanyProfile.CompanyProfile;

namespace LiberisLabs.CompaniesHouse.Tests.CompaniesHouseCompanyProfileClientTests
{
    [TestFixture]
    public class CompaniesHouseCompanyProfileClientTests
    {
        private CompaniesHouseCompanyProfileClient _client;

        private CompaniesHouseClientResponse<CompanyProfile> _result;
        private ResourceBuilders.CompanyProfile _companyProfile;

        [TestCaseSource(nameof(TestCases))]
        public void GivenACompaniesHouseCompanyProfileClient_WhenGettingACompanyProfile(CompaniesHouseCompanyProfileClientTestCase testCase)
        {
            _companyProfile = new CompanyProfileBuilder().Build(testCase);
            var resource = new CompanyProfileResourceBuilder(_companyProfile)
                                .Create();

            var uri = new Uri("https://wibble.com/search/companies");

            HttpMessageHandler handler = new StubHttpMessageHandler(uri, resource);
            var httpClientFactory = new Mock<IHttpClientFactory>();
            httpClientFactory.Setup(x => x.CreateHttpClient())
                .Returns(new HttpClient(handler));

            var uriBuilder = new Mock<ICompanyProfileUriBuilder>();
            uriBuilder.Setup(x => x.Build(It.IsAny<string>()))
                .Returns(uri);

            _client = new CompaniesHouseCompanyProfileClient(httpClientFactory.Object, uriBuilder.Object);

            _result = _client.GetCompanyProfileAsync("abc").Result;

            _result.Data.ShouldBeEquivalentTo(_companyProfile);
        }


        public static CompaniesHouseCompanyProfileClientTestCase[] TestCases()
        {
            var allLastAccountsTypes = EnumerationMappings.PossibleLastAccountsTypes.Keys
                .Select(x => new CompaniesHouseCompanyProfileClientTestCase
                {
                    LastAccountsType = x,
                    CompanyStatus = EnumerationMappings.PossibleCompanyStatuses.Keys.First(),
                    CompanyStatusDetail = EnumerationMappings.PossibleCompanyStatusDetails.Keys.First(),
                    Jurisdiction = EnumerationMappings.PossibleJurisdictions.Keys.First(),
                    Type = EnumerationMappings.ExpectedCompanyTypesMap.Keys.First()
                });

            var allCompanyStatuses = EnumerationMappings.PossibleCompanyStatuses.Keys
                .Select(x => new CompaniesHouseCompanyProfileClientTestCase
                {
                    LastAccountsType = EnumerationMappings.PossibleLastAccountsTypes.Keys.First(),
                    CompanyStatus = x,
                    CompanyStatusDetail = EnumerationMappings.PossibleCompanyStatusDetails.Keys.First(),
                    Jurisdiction = EnumerationMappings.PossibleJurisdictions.Keys.First(),
                    Type = EnumerationMappings.ExpectedCompanyTypesMap.Keys.First()
                });

            var allCompanyStatusDetails = EnumerationMappings.PossibleCompanyStatusDetails.Keys
                .Select(x => new CompaniesHouseCompanyProfileClientTestCase
                {
                    LastAccountsType = EnumerationMappings.PossibleLastAccountsTypes.Keys.First(),
                    CompanyStatus = EnumerationMappings.PossibleCompanyStatuses.Keys.First(),
                    CompanyStatusDetail = x,
                    Jurisdiction = EnumerationMappings.PossibleJurisdictions.Keys.First(),
                    Type = EnumerationMappings.ExpectedCompanyTypesMap.Keys.First()
                });

            var allJurisdictions = EnumerationMappings.PossibleJurisdictions.Keys
                .Select(x => new CompaniesHouseCompanyProfileClientTestCase
                {
                    LastAccountsType = EnumerationMappings.PossibleLastAccountsTypes.Keys.First(),
                    CompanyStatus = EnumerationMappings.PossibleCompanyStatuses.Keys.First(),
                    CompanyStatusDetail = EnumerationMappings.PossibleCompanyStatusDetails.Keys.First(),
                    Jurisdiction = x,
                    Type = EnumerationMappings.ExpectedCompanyTypesMap.Keys.First()
                });

            var allCompanyTypes = EnumerationMappings.ExpectedCompanyTypesMap.Keys
                .Select(x => new CompaniesHouseCompanyProfileClientTestCase
                {
                    LastAccountsType = EnumerationMappings.PossibleLastAccountsTypes.Keys.First(),
                    CompanyStatus = EnumerationMappings.PossibleCompanyStatuses.Keys.First(),
                    CompanyStatusDetail = EnumerationMappings.PossibleCompanyStatusDetails.Keys.First(),
                    Jurisdiction = EnumerationMappings.PossibleJurisdictions.Keys.First(),
                    Type = x
                });

            return allLastAccountsTypes.Concat(allCompanyStatuses)
                .Concat(allCompanyStatusDetails)
                .Concat(allJurisdictions)
                .Concat(allCompanyTypes)
                .ToArray();
        }

    }
}
