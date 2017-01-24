-----------------------------------------------------------------------------------------------------------------------
-- Table:		ECI.AdditionalDemographics
-- Author:		Scott Martin
-- Date:		1/5/2015
--
-- Purpose:		Store Additional ECI Demographic Data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/5/2016	Scott Martin		Initial Creation
-- 1/5/2016	Scott Martin		Removed RaceID, LanguageID, EthnicityID, SchoolDistrictID and IsInterpreterRequired.
--								Added column RegistrationAdditionalDemographicID. Changed the type for AdditionalDemographicID
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
----------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE ECI.AdditionalDemographics(
	[AdditionalDemographicID] [bigint] IDENTITY(1,1) NOT NULL,
	[RegistrationAdditionalDemographicID] [bigint] NOT NULL,
	[ReferralDispositionStatusID] [int] NULL,
	[IsCPSInvolved] [bit] NOT NULL DEFAULT(0),
	[IsChildHospitalized] [bit] NULL DEFAULT(0),
	[ExpectedHospitalDischargeDate] [date] NULL,
	[BirthWeightLbs] [smallint] NULL,
	[BirthWeightOz] [smallint] NULL,
	[IsTransfer] [bit] NULL DEFAULT(0),
	[TransferFrom] NVARCHAR(250) NULL,
	[TransferDate] [date] NULL,
	[IsOutOfServiceArea] [bit] NULL DEFAULT(0),
	[ReportingUnitID] [int] NULL,
	[ServiceCoordinatorID] [int] NULL,
	[ServiceCoordinatorPhoneID] [bigint] NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_AdditionalDemographics] PRIMARY KEY CLUSTERED 
(
	[AdditionalDemographicID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ECI].[AdditionalDemographics]  WITH CHECK ADD  CONSTRAINT [FK_AdditionalDemographics_RegistrationAdditionalDemographicID] FOREIGN KEY([RegistrationAdditionalDemographicID])
REFERENCES [Registration].[AdditionalDemographics] ([AdditionalDemographicID])
GO

ALTER TABLE [ECI].[AdditionalDemographics] CHECK CONSTRAINT [FK_AdditionalDemographics_RegistrationAdditionalDemographicID]
GO

ALTER TABLE [ECI].[AdditionalDemographics]  WITH CHECK ADD  CONSTRAINT [FK_AdditionalDemographics_ReferralDispositionStatusID] FOREIGN KEY([ReferralDispositionStatusID])
REFERENCES [Reference].[ReferralDispositionStatus] ([ReferralDispositionStatusID])
GO

ALTER TABLE [ECI].[AdditionalDemographics] CHECK CONSTRAINT [FK_AdditionalDemographics_ReferralDispositionStatusID]
GO

ALTER TABLE [ECI].[AdditionalDemographics]  WITH CHECK ADD  CONSTRAINT [FK_AdditionalDemographics_ServiceCoordinatorID] FOREIGN KEY([ServiceCoordinatorID])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [ECI].[AdditionalDemographics] CHECK CONSTRAINT [FK_AdditionalDemographics_ServiceCoordinatorID]
GO

ALTER TABLE [ECI].[AdditionalDemographics]  WITH CHECK ADD  CONSTRAINT [FK_AdditionalDemographics_ServiceCoordinatorPhoneID] FOREIGN KEY([ServiceCoordinatorPhoneID])
REFERENCES [Core].[Phone] ([PhoneID])
GO

ALTER TABLE [ECI].[AdditionalDemographics] CHECK CONSTRAINT [FK_AdditionalDemographics_ServiceCoordinatorPhoneID]
GO

ALTER TABLE ECI.AdditionalDemographics WITH CHECK ADD CONSTRAINT [FK_AdditionalDemographics_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.AdditionalDemographics CHECK CONSTRAINT [FK_AdditionalDemographics_UserModifedBy]
GO
ALTER TABLE ECI.AdditionalDemographics WITH CHECK ADD CONSTRAINT [FK_AdditionalDemographics_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.AdditionalDemographics CHECK CONSTRAINT [FK_AdditionalDemographics_UserCreatedBy]
GO
