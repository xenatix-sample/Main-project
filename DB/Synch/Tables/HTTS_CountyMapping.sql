-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [Synch].[CMHC_ClientRegistration]
-- Author:		John Crossen
-- Date:		 04/08/2016
--
-- Purpose:		Table for CMHC Interface -- County Mapping File
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/08/2016	John Crossen - TFS # 9194 Initial creation.

-----------------------------------------------------------------------------------------------------------------------


CREATE TABLE [Synch].[HTTS_CountyMapping](
	HTTS_CountyMappingID BIGINT NOT NULL IDENTITY(1,1),
	[CountyOfResidence] [varchar](50) NULL,
	[Status] [varchar](11) NULL,
	[Description] [varchar](143) NULL,
	CONSTRAINT [PK_HTTS_CountyMapping] PRIMARY KEY CLUSTERED ([HTTS_CountyMappingID] ASC)
) ON [PRIMARY]

GO
