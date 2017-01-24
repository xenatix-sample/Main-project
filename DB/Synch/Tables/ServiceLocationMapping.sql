-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [Synch].[[ServiceLocationMapping]]
-- Author:		John Crossen
-- Date:		 04/08/2016
--
-- Purpose:		Table for CMHC Interface -- Service Location Mapping
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/20/2016	John Crossen - TFS # 10142 Initial creation.
-- 05/16/2016	Kyle Campbell	TFS #10884	Add CMHC_Program_Team and aXis_Program_Unit_DetailID columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Synch].[ServiceLocationMapping](
	ServiceLocationMappingID BIGINT  IDENTITY(1,1) NOT NULL,
	[aXis_Company] [VARCHAR](100) NULL,
	[aXis_Division] [VARCHAR](100) NULL,
	[aXis_Program ] [VARCHAR](100) NULL,
	[aXis_Program_Unit] [VARCHAR](100) NULL,
	aXis_Program_Unit_DetailID BIGINT NULL,
	CMHC_Program_Team VARCHAR(100) NULL,
	[CMHC_RU] [VARCHAR](100) NULL,
	[CMHC_Description] [VARCHAR](100) NULL
CONSTRAINT [PK_ServiceLocationMapping] PRIMARY KEY CLUSTERED 
(
	ServiceLocationMappingID ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



