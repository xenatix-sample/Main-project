-----------------------------------------------------------------------------------------------------------------------
-- Table:		[dbo].[ErrorDetails]
-- Author:		Sumana Sangapu
-- Date:		03/02/2016
--
-- Purpose:		Holds the Error rows generated from SSIS package for AD Sync Service
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/02/2016	Sumana Sangapu	 Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[ErrorDetails](
	[ErrorDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[BatchID] [bigint] NULL,
	[PackageName] [nvarchar](100) NULL,
	[PackageTask] [nvarchar](50) NULL,
	[ErrorDescription] [varchar](1000) NULL,
	[ErrorCode] [int] NULL,
	[ErrorXML] [xml] NULL,
	[CreatedOn] [datetime] NULL
) ON [PRIMARY]

GO

