  -----------------------------------------------------------------------------------------------------------------------
-- Table:		dbo.[UserPhone_Staging_ErrorDetails]
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

CREATE TABLE [Synch].[UserPhone_Staging_ErrorDetails](
	[UserPhoneID] [varchar](50) NULL,
	[UserGUID] [varchar](50) NULL,
	[PhoneID] [varchar](50) NULL,
	[PhonePermissionID] [varchar](50) NULL,
	[IsPrimary] [varchar](50) NULL,
	[IsActive] [varchar](50) NULL,
	[ModifiedBy] [varchar](50) NULL,
	[ModifiedOn] [varchar](50) NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedOn] [varchar](50) NULL,
	[SystemCreatedOn] [varchar](50) NULL,
	[SystemModifiedOn] [varchar](50) NULL ,
	[ErrorSource] nvarchar(50) NULL
) ON [PRIMARY]

GO


