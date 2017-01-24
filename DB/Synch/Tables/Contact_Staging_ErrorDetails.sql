 -----------------------------------------------------------------------------------------------------------------------
-- Table:		Synch.[Contact_Staging_ErrorDetails]
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

CREATE TABLE [Synch].[Contact_Staging_ErrorDetails](
	[ContactID] [bigint] NULL,
	[MRN] [bigint] NULL,
	[MPI] [varchar](50) NULL,
	[ContactTypeID] [varchar](50) NULL,
	[ClientTypeID] [varchar](50) NULL,
	[FirstName] [varchar](50) NULL,
	[Middle] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[SuffixID] [varchar](50) NULL,
	[GenderID] [varchar](50) NULL,
	[TitleID] [varchar](50) NULL,
	[SequestedByID] [varchar](50) NULL,
	[DOB] [datetime] NULL,
	[DOBStatusID] [varchar](50) NULL,
	[SSN] [bigint] NULL,
	[SSNStatusID] [varchar](50) NULL,
	[DriverLicense] [varchar](50) NULL,
	[DriverLicenseStateID] [varchar](50) NULL,
	[DeceasedDate] [datetime] NULL,
	[PreferredContactMethodID] [varchar](50) NULL,
	[ReferralSourceID] [varchar](50) NULL,
	[IsPregnate] [varchar](50) NULL,
	[GestationalAge] [varchar](50) NULL,
	[IsActive] [varchar](50) NULL,
	[ModifiedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[SystemCreatedOn] [datetime] NULL,
	[SystemModifiedOn] [datetime] NULL,
	[RegistrationDate] [varchar](50) NULL,
	[RegistrationTime] [varchar](50) NULL,
	PreferredName varchar(50) NULL,
	[ErrorSource] varchar(50) NULL
) ON [PRIMARY]

GO


