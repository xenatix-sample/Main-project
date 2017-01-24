  -----------------------------------------------------------------------------------------------------------------------
-- Table:		dbo.[UserCredential_Staging_ErrorDetails]
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

CREATE TABLE [Synch].[UserCredential_Staging_ErrorDetails](
	[UserCredentialsID] [bigint] NULL,
	[UserGUID] [varchar](352) NULL,
	[CredentialID] [varchar](429) NULL,
	[LicenseNbr] [varchar](88) NULL,
	[StateIssuedByID] [varchar](22) NULL,
	[LicenseIssueDate] [datetime] NULL,
	[LicenseExpirationDate] [datetime] NULL,
	[IsActive] [varchar](44) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](33) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](33) NULL,
	[SystemCreatedOn] [datetime] NULL,
	[SystemModifiedOn] [datetime] NULL,
	[ErrorSource] nvarchar(50) NULL
) ON [PRIMARY]

GO


