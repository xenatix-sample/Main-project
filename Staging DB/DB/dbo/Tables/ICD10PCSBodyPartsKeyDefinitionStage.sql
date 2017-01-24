 -----------------------------------------------------------------------------------------------------------------------
-- Table:	    [dbo].[ICD10DrugsCMLevel1Stage]
-- Author:		Sumana Sangapu
-- Date:		12/10/2015
--
-- Purpose:		Holds the BodyParts Key Definition info in Stage tables
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015	Sumana Sangapu	 Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE dbo.ICD10PCSBodyPartsKeyDefinitionStage(
	[AxisID] [int] NULL,
	[PCSCodeID] [int] NOT NULL,
	[Codes] [int] NULL,
	[Pos] [int] NULL,
	[Value] [varchar](100) NULL,
	Label varchar(100) NULL,
	LabelCode varchar(50) NULL,
	AsOFYear int NULL
) ON [PRIMARY] 
GO
 