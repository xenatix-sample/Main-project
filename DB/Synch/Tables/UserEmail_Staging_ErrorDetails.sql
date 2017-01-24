  -----------------------------------------------------------------------------------------------------------------------
-- Table:		dbo.[UserEmail_Staging_ErrorDetails]
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

CREATE TABLE [Synch].[UserEmail_Staging_ErrorDetails](
	[UserEmailID] [bigint] NULL,
	[UserGUID] [varchar](352) NULL,
	[EmailID] [bigint] NULL,
	[EmailPermissionID] [varchar](55) NULL,
	[IsPrimary] [varchar](44) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](33) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](33) NULL,
	[SystemCreatedOn] [datetime] NULL,
	[SystemModifiedOn] [datetime] NULL,
	[IsActive] [varchar](44) NULL,
	[ErrorSource] nvarchar(50) NULL
) ON [PRIMARY]

GO


