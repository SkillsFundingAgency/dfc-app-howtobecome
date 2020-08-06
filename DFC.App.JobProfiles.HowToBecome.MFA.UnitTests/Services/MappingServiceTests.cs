using AutoMapper;
using DFC.App.JobProfiles.HowToBecome.Data.Enums;
using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.Models.DataModels;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels;
using DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.AutoMapperProfile;
using DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Services;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace DFC.App.JobProfiles.HowToBecome.MFA.UnitTests.Services
{
    [Trait("Messaging Function", "Mapping Service Tests")]
    public class MappingServiceTests
    {
        private const string TestJobName = "Test Job name";
        private const int SequenceNumber = 123;
        private const string SocCodeId = "99";
        private const string Title = "Title 1";
        private const string IntroText = "Intro Text";
        private const string RouteSubjects = "Route Subject 1";
        private const string FurtherRouteInformation = "Further route info 1";
        private const string EntryRequirementTitle1 = "Entry Requirement 1";
        private const string EntryRequirementInfo1 = "Entry requirement info 1";
        private const string EntryRequirementTitle2 = "Entry Requirement 2";
        private const string EntryRequirementInfo2 = "Entry requirement info 2";
        private const string MoreInformationTitle1 = "Title 1";
        private const string MoreInformationText1 = "More info text 1";
        private const string MoreInformationUrl1 = "http://test1.com";
        private const string MoreInformationTitle2 = "Title 2";
        private const string MoreInformationText2 = "More info text 2";
        private const string MoreInformationUrl2 = "http://test2.com";
        private const string VolunteeringText1 = "Volunteering 1";
        private const string OtherRoutes1 = "Other Routes 1";
        private const string DirectApplication1 = "Direct application 1";
        private const string Work1 = "Work 1";
        private const string FurtherMoreInformation1 = "Further Information 1";
        private const string CareerTips1 = "Career Tips 1";
        private const string ProfessionalAndIndustryBodies1 = "Professional Indemnity 1";
        private const string RegistrationTitle2 = "Registration 2";
        private const string RegistrationDescription2 = "Registration 2 description";
        private const string RegistrationTitle1 = "Registration 1";
        private const string RegistrationDescription1 = "Registration 1 description";
        private const string RouteRequirement1 = "Route requirement 1";

        private static readonly Guid JobProfileId = Guid.NewGuid();
        private static readonly DateTime LastModified = DateTime.UtcNow.AddDays(-1);
        private static readonly Guid EntryRequirementId1 = Guid.NewGuid();
        private static readonly Guid EntryRequirementId2 = Guid.NewGuid();
        private static readonly Guid MoreInformationId1 = Guid.NewGuid();
        private static readonly Guid MoreInformationId2 = Guid.NewGuid();
        private static readonly Guid RegistrationId1 = Guid.NewGuid();
        private static readonly Guid RegistrationId2 = Guid.NewGuid();

        private readonly IMappingService mappingService;

        public MappingServiceTests()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new HowToBecomeSegmentModelProfile());
            });

            var mapper = new Mapper(config);

            mappingService = new MappingService(mapper);
        }

        [Fact]
        public void MapToSegmentModelWhenJobProfileMessageSentThenItIsMappedCorrectly()
        {
            var fullJPMessage = BuildJobProfileMessage();
            var message = JsonConvert.SerializeObject(fullJPMessage);
            var expectedResponse = BuildExpectedResponse();

            // Act
            var actualMappedModel = mappingService.MapToSegmentModel(message, SequenceNumber);

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualMappedModel);
        }

        [Theory]
        [InlineData ("Title", "", "title")]
        [InlineData("Title", "Override Title", "Override Title")]
        public void HtbTitleIsOverridenCorrectly(string title, string widgetContentTitle, string expectedTitle)
        {
            //Arrange
            var jobProfileMessage = new JobProfileMessage() { DynamicTitlePrefix = "No Prefix", Title = title, WidgetContentTitle = widgetContentTitle };
            var message = JsonConvert.SerializeObject(jobProfileMessage);

            //Act
            var actualMappedModel = mappingService.MapToSegmentModel(message, SequenceNumber);

            //Asserts
            actualMappedModel.Data.Title.Should().Be(expectedTitle);
        }

        private static JobProfileMessage BuildJobProfileMessage()
        {
            return new JobProfileMessage
            {
                Title = Title,
                JobProfileId = JobProfileId,
                LastModified = LastModified,
                CanonicalName = TestJobName,
                SocLevelTwo = SocCodeId,
                DynamicTitlePrefix = "No Prefix",
                HowToBecomeData = new SitefinityHowToBecomeMessage
                {
                    IntroText = IntroText,
                    Registrations = new List<SitefinityRegistrations>
                    {
                        new SitefinityRegistrations
                        {
                            Id = RegistrationId1,
                            Title = RegistrationTitle1,
                            Info = RegistrationDescription1,
                        },
                        new SitefinityRegistrations
                        {
                            Id = RegistrationId2,
                            Title = RegistrationTitle2,
                            Info = RegistrationDescription2,
                        },
                    },
                    FurtherRoutes = new SitefinityFurtherRoutes
                    {
                        Volunteering = VolunteeringText1,
                        OtherRoutes = OtherRoutes1,
                        DirectApplication = DirectApplication1,
                        Work = Work1,
                    },
                    FurtherInformation = new SitefinityFurtherInformation
                    {
                        FurtherInformation = FurtherMoreInformation1,
                        CareerTips = CareerTips1,
                        ProfessionalAndIndustryBodies = ProfessionalAndIndustryBodies1,
                    },
                    RouteEntries = new List<SitefinityRouteEntries>
                    {
                        new SitefinityRouteEntries
                        {
                            RouteSubjects = RouteSubjects,
                            RouteRequirement = RouteRequirement1,
                            RouteName = (int)RouteName.University,
                            FurtherRouteInformation = FurtherRouteInformation,
                            MoreInformationLinks = new List<SitefinityMoreInformationLinks>
                            {
                                new SitefinityMoreInformationLinks
                                {
                                    Id = MoreInformationId1,
                                    Title = MoreInformationTitle1,
                                    Text = MoreInformationText1,
                                    Url = MoreInformationUrl1,
                                },
                                new SitefinityMoreInformationLinks
                                {
                                    Id = MoreInformationId2,
                                    Title = MoreInformationTitle2,
                                    Text = MoreInformationText2,
                                    Url = MoreInformationUrl2,
                                },
                            },
                            EntryRequirements = new List<SitefinityEntryRequirement>
                            {
                                new SitefinityEntryRequirement
                                {
                                    Title = EntryRequirementTitle1, Id = EntryRequirementId1,
                                    Info = EntryRequirementInfo1,
                                },
                                new SitefinityEntryRequirement
                                {
                                    Title = EntryRequirementTitle2, Id = EntryRequirementId2,
                                    Info = EntryRequirementInfo2,
                                },
                            },
                        },
                    },
                },
            };
        }

        private static HowToBecomeSegmentModel BuildExpectedResponse()
        {
            return new HowToBecomeSegmentModel
            {
                CanonicalName = TestJobName,
                DocumentId = JobProfileId,
                SequenceNumber = SequenceNumber,
                SocLevelTwo = SocCodeId,
                Etag = null,
                Data = new HowToBecomeSegmentDataModel
                {
                    Title = Title.ToLowerInvariant(),
                    LastReviewed = LastModified,
                    EntryRouteSummary = IntroText,
                    EntryRoutes = new EntryRoutes
                    {
                        CommonRoutes = new List<CommonRoutes>
                        {
                            new CommonRoutes
                            {
                                RouteName = RouteName.University,
                                Subject = RouteSubjects,
                                EntryRequirementPreface = RouteRequirement1,
                                FurtherInformation = FurtherRouteInformation,
                                EntryRequirements = new List<EntryRequirement>
                                {
                                    new EntryRequirement
                                    {
                                        Id = EntryRequirementId1,
                                        Description = EntryRequirementInfo1,
                                    },
                                    new EntryRequirement
                                    {
                                        Id = EntryRequirementId2,
                                        Description = EntryRequirementInfo2,
                                    },
                                },
                                AdditionalInformation = new List<AdditionalInformation>
                                {
                                    new AdditionalInformation
                                    {
                                        Id = MoreInformationId1,
                                        Text = MoreInformationText1,
                                        Link = MoreInformationUrl1,
                                    },
                                    new AdditionalInformation
                                    {
                                        Id = MoreInformationId2,
                                        Text = MoreInformationText2,
                                        Link = MoreInformationUrl2,
                                    },
                                },
                            },
                        },
                        OtherRoutes = OtherRoutes1,
                        DirectApplication = DirectApplication1,
                        Work = Work1,
                        Volunteering = VolunteeringText1,
                    },
                    MoreInformation = new MoreInformation
                    {
                        FurtherInformation = FurtherMoreInformation1,
                        ProfessionalAndIndustryBodies = ProfessionalAndIndustryBodies1,
                        CareerTips = CareerTips1,
                    },
                    Registrations = new List<Registration>
                    {
                        new Registration { Id = RegistrationId1, Title = RegistrationTitle1, Description = RegistrationDescription1 },
                        new Registration { Id = RegistrationId2, Title = RegistrationTitle2, Description = RegistrationDescription2 },
                    },
                },
            };
        }
    }
}
