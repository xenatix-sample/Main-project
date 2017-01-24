   -----------------------------------------------------------------------------------------------------------------------
-- Table:		dbo.[AdditionalDemographics_Staging_ErrorDetails]
-- Author:		Sumana Sangapu
-- Date:		05/19/2016
--
-- Purpose:		Table to hold the error details from the validation of lookup data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/19/2016	Sumana Sangapu		Initial creation
-----------------------------------------------------------------------------------------------------------------------


CREATE TABLE [Synch].[AdditionalDemographics_Staging_ErrorDetails](
	[AdditionalDemographicID] [bigint] NULL,
	[ContactID] [bigint] NULL,
	[AdvancedDirective] [varchar](255) NULL,
	[AdvancedDirectiveTypeID] [varchar](255) NULL,
	[SmokingStatusID] [varchar](255) NULL,
	[OtherRace] [varchar](176) NULL,
	[OtherEthnicity] [varchar](255) NULL,
	[LookingForWork] [varchar](255) NULL,
	[SchoolDistrictID] [varchar](539) NULL,
	[EthnicityID] [varchar](209) NULL,
	[PrimaryLanguageID] [varchar](110) NULL,
	[SecondaryLanguageID] [varchar](255) NULL,
	[InterpreterRequired] [varchar](55) NULL,
	[LegalStatusID] [varchar](407) NULL,
	[LivingArrangementID] [varchar](253) NULL,
	[CitizenshipID] [varchar](132) NULL,
	[MaritalStatusID] [varchar](242) NULL,
	[EmployementStatusID] [varchar](220) NULL,

	[ReligionID] [varchar](255) NULL,
	[VeteranID] [varchar](231) NULL,
	[EducationID] [varchar](255) NULL,
	[SchoolAttending] [varchar](330) NULL,
	[SchoolBeginDate] [varchar](255) NULL,
	[SchoolEndDate] [varchar](255) NULL,
	[LivingWill] [varchar](255) NULL,
	[PowerOfAttorney] [varchar](255) NULL,
	[OtherLegalStatus] [varchar](255) NULL,
	[OtherPreferredLanguage] [varchar](255) NULL,
	[OtherSecondaryLanguage] [varchar](255) NULL,
	[OtherCitizenship] [varchar](255) NULL,
	[OtherEducation] [varchar](255) NULL,
	[OtherLivingArrangement] [varchar](187) NULL,
	[OtherVeteranStatus] [varchar](255) NULL,
	[OtherEmploymentStatus] [varchar](255) NULL,
	[OtherReligion] [varchar](255) NULL,
	[IsActive] [varchar](44) NULL,
	[ModifiedBy] [varchar](33) NULL,
	[ModifiedOn] [datetime] NULL,
	[CreatedBy] [varchar](33) NULL,
	[CreatedOn] [datetime] NULL,
	[SystemCreatedOn] [datetime] NULL,
	[SystemModifiedOn] [datetime] NULL,
	PlaceOfEmployment nvarchar(50) NULL,
	EmploymentBeginDate date NULL,
	EmploymentEndDate date NULL,
	ErrorSource varchar(50) NULL
) ON [PRIMARY]

GO