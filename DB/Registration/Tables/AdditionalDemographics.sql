-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Registration].[AdditionalDemographics]
-- Author:		Rajiv Ranjan
-- Date:		07/29/2015
--
-- Purpose:		Additional Demographics Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/29/2015	Rajiv Ranjan	 Initial creation.
-- 7/31/2015    John Crossen    Change schema from dbo to Registration
-- 08/05/2015   Rajiv Ranjan	 - Removed MRN field
-- 08/06/2015   Rajiv Ranjan	 - Fields is changed to nullable which is not mandatory.
-- 08/24/2015   Rajiv Ranjan	 - Added schoolDistrictId field
-- 08/28/2015   Rajiv Ranjan	 - Added OtherEthnicity
-- 12/31/2015   Gaurav Gupta	 - Added OtherLegalStatus,OtherPreferredLanguage,OtherSecondaryLanguage,OtherCitizenship,OtherEducation,OtherLivingArrangement,OtherVeteranStatus,OtherEmploymentStatus,OtherReligion
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 03/08/2016	Scott Martin	Changed FullCodeDNR to AdvancedDirective and added AdvancedDirectiveTypeID
-- 03/09/2016	Kyle Campbell	Added SchoolAttended, SchoolBeginDate, SchoolEndDate fields
-- 03/18/2016	Gurpreet Singh	Made RaceID and EthnicityID Nullable
-- 03/18/2016   Lokesh Singhal   Removed @RaceID
-- 05/13/2016	Arun Choudhary	Added PlaceOfEmployment, EmploymentBeginDate, EmploymentEndDate fields
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[AdditionalDemographics](
	[AdditionalDemographicID] BIGINT IDENTITY(1,1) NOT NULL,
	[ContactID] BIGINT NOT NULL,
	[AdvancedDirective] BIT NULL,
	[AdvancedDirectiveTypeID] INT NULL,
	[SmokingStatusID] INT NULL,
	[OtherRace] NVARCHAR(250)  NULL,
	[OtherEthnicity]  NVARCHAR(250)  NULL,
	[LookingForWork] BIT  NULL,
	[SchoolDistrictID] [int] NULL,
	[EthnicityID] [int] NULL,
	[PrimaryLanguageID] [int] NULL,
	[SecondaryLanguageID] [int] NULL,
	[InterpreterRequired] [BIT] NULL DEFAULT(0),
	[LegalStatusID] [int] NULL,
	[LivingArrangementID] [int] NULL,
	[CitizenshipID] [int] NULL,
	[MaritalStatusID] [int] NULL,
	[EmploymentStatusID] [int] NULL,
	[PlaceOfEmployment] NVARCHAR(250) NULL,
	[EmploymentBeginDate] DATE NULL,
	[EmploymentEndDate] DATE NULL,
	[ReligionID] [int] NULL,
	[VeteranID] [int] NULL,
	[EducationID] [int] NULL,
	[SchoolAttended] NVARCHAR(250) NULL,
	[SchoolBeginDate] DATE NULL,
	[SchoolEndDate] DATE NULL,
	[LivingWill] [bit] NULL,
	[PowerOfAttorney] [bit] NULL,
	[OtherLegalStatus] NVARCHAR(250)  NULL,
	[OtherPreferredLanguage] NVARCHAR(250)  NULL,
	[OtherSecondaryLanguage] NVARCHAR(250)  NULL,
	[OtherCitizenship] NVARCHAR(250)  NULL,
	[OtherEducation] NVARCHAR(250)  NULL,
	[OtherLivingArrangement] NVARCHAR(250)  NULL,
	[OtherVeteranStatus] NVARCHAR(250)  NULL,
	[OtherEmploymentStatus] NVARCHAR(250)  NULL,
	[OtherReligion] NVARCHAR(250)  NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [FK_AdditionalDemographics_ContactID] FOREIGN KEY ([ContactID]) REFERENCES [Registration].[Contact] (ContactID) ON DELETE CASCADE,
	CONSTRAINT [PK_AdditionalDemographics_AdditionalDemographicID] PRIMARY KEY CLUSTERED 
	(
		AdditionalDemographicID ASC
	)
	WITH (
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE Registration.AdditionalDemographics WITH CHECK ADD CONSTRAINT [FK_AdditionalDemographics_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.AdditionalDemographics CHECK CONSTRAINT [FK_AdditionalDemographics_UserModifedBy]
GO
ALTER TABLE Registration.AdditionalDemographics WITH CHECK ADD CONSTRAINT [FK_AdditionalDemographics_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.AdditionalDemographics CHECK CONSTRAINT [FK_AdditionalDemographics_UserCreatedBy]
GO
ALTER TABLE Registration.AdditionalDemographics WITH CHECK ADD CONSTRAINT [FK_AdditionalDemographics_AdvancedDirectiveTypeID] FOREIGN KEY ([AdvancedDirectiveTypeID]) REFERENCES [Reference].[AdvancedDirectiveType] ([AdvancedDirectiveTypeID])
GO
ALTER TABLE Registration.AdditionalDemographics CHECK CONSTRAINT [FK_AdditionalDemographics_AdvancedDirectiveTypeID]
GO

CREATE NONCLUSTERED INDEX [IX_AdditionalDemographics.Contact_IsActive]
ON [Registration].[AdditionalDemographics] ([ContactID],[IsActive])
INCLUDE ([EthnicityID],[LegalStatusID])
GO