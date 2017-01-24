 -----------------------------------------------------------------------------------------------------------------------
-- Table:	    [dbo].[ICD10PCSXMLStage]
-- Author:		Sumana Sangapu
-- Date:		12/15/2015
--
-- Purpose:		Holds the PCS XML info in Stage tables
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015	Sumana Sangapu	 Initial Creation
----------------------------------------------------------------------------------------------------------------------- 

CREATE TABLE [dbo].[ICD10PCSXMLStage](
	[PCSCodeID] [int] IDENTITY(1,1) NOT NULL,
	[BodySystemsXML] [xml] NULL,
	[BodyPartsKeyXML] [xml] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


