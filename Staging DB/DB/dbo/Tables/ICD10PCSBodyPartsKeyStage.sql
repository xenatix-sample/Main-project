 -----------------------------------------------------------------------------------------------------------------------
-- Table:	    [dbo].[ICD10PCSBodyPartsKeyStage]
-- Author:		Sumana Sangapu
-- Date:		12/15/2015
--
-- Purpose:		Holds the Body Parts Key info in Stage tables
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015	Sumana Sangapu	 Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[ICD10PCSBodyPartsKeyStage](
	[AxisID] [int] IDENTITY(1,1) NOT NULL,
	[PCSCodeID] [int] NOT NULL,
	[Codes] [int] NULL,
	[Pos] [int] NULL,
	[Value] [varchar](100) NULL,
	Title varchar(500) NULL,
	[BodyPartsDefinitionXML] [xml] NULL,
	[AsofYear] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO