 -----------------------------------------------------------------------------------------------------------------------
-- Table:		dbo.[Address_Staging_ErrorDetails]
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


CREATE TABLE [Synch].[Address_Staging_ErrorDetails](
	[AddressID] [bigint] NULL,
	[AddressTypeID] [varchar](165) NULL,
	[Name] [varchar](255) NULL,
	[Line1] [varchar](264) NULL,
	[Line2] [varchar](255) NULL,
	[City] [varchar](220) NULL,
	[County] [varchar](253) NULL,
	[StateProvince] [varchar](22) NULL,
	[Zip] [varchar](max) NULL,
	[Country] [varchar](33) NULL,
	[Lattitude] [varchar](255) NULL,
	[Longitude] [varchar](255) NULL,
	[ComplexName] [varchar](255) NULL,
	[GateCode] [varchar](255) NULL,
	IsVerified varchar(50) NULL,
	[IsActive] [varchar](44) NULL,
	[ModifiedBy] [varchar](33) NULL,
	[ModifiedOn] [datetime] NULL,
	[CreatedBy] [varchar](33) NULL,
	[CreatedOn] [datetime] NULL,
	[SystemCreatedOn] [datetime] NULL,
	[SystemModifiedOn] [datetime] NULL,
	[ErrorSource] nvarchar(50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


