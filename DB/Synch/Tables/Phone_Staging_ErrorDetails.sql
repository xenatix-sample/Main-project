  -----------------------------------------------------------------------------------------------------------------------
-- Table:		dbo.[Phone_Staging_ErrorDetails]
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
CREATE TABLE [Synch].[Phone_Staging_ErrorDetails](
	[PhoneID] [bigint] NULL,
	[PhoneTypeID] [varchar](44) NULL,
	[Number] [varchar](154) NULL,
	[Extension] [varchar](255) NULL,
	[IsActive] [varchar](44) NULL,
	[ModifiedBy] [varchar](33) NULL,
	[ModifiedOn] [datetime] NULL,
	[CreatedBy] [varchar](33) NULL,
	[CreatedOn] [datetime] NULL,
	[SystemCreatedOn] [datetime] NULL,
	[SystemModifiedOn] [datetime] NULL,
	[ErrorSource] nvarchar(50) NULL
) ON [PRIMARY]

GO


