
-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Synch].[ECIAdditionalDemographics_Staging_Error_Details]
-- Author:		Sumana Sangapu
-- Date:		06/24/2016
--
-- Purpose:		Validate lookup data in the Staging files used for Data Conversion
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/24/2016	Sumana Sangapu		Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Synch].[ECIAdditionalDemographics_Staging_Error_Details](
	[AdditionalDemographicID] [bigint] NOT NULL,
	[RegistrationAdditionalDemographicID] [bigint] NOT NULL,
	[ReferralDispositionStatusID] [int] NULL,
	[IsCPSInvolved] [bit] NOT NULL ,
	[IsChildHospitalized] [bit] NULL,
	[ExpectedHospitalDischargeDate] [date] NULL,
	[BirthWeightLbs] [smallint] NULL,
	[BirthWeightOz] [smallint] NULL,
	[IsTransfer] [bit] NULL ,
	[TransferFrom] [nvarchar](250) NULL,
	[TransferDate] [date] NULL,
	[IsOutOfServiceArea] [bit] NULL,
	[ReportingUnitID] [int] NULL,
	[ServiceCoordinatorID] [int] NULL,
	[ServiceCoordinatorPhoneID] [bigint] NULL,
	[IsActive] [bit] NOT NULL ,
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL ,
	[CreatedBy] [int] NOT NULL ,
	[CreatedOn] [datetime] NOT NULL ,
	[SystemCreatedOn] [datetime] NOT NULL,
	[SystemModifiedOn] [datetime] NOT NULL ,
	ErrorSource nvarchar(50) NULL
) ON [PRIMARY]

GO



