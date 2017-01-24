 -----------------------------------------------------------------------------------------------------------------------
-- Table:	    [dbo].[ICD10DrugsCMLevel2Stage]
-- Author:		Sumana Sangapu
-- Date:		12/03/2015
--
-- Purpose:		Holds the Drugs CM Level2 info in Stage tables
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/03/2015	Sumana Sangapu	 Initial Creation
-----------------------------------------------------------------------------------------------------------------------


CREATE TABLE [dbo].[ICD10DrugsCMLevel2Stage](
	[Substance] [varchar](100) NULL,
	[Substance1] [varchar](100) NULL,
	[level2] [int] NULL,
	[PoisoningAccidental_2] [varchar](100) NULL,
	[PoisoningIntentioanlSelfHarm_2] [varchar](100) NULL,
	[PoisoningAssault_2] [varchar](100) NULL,
	[PoisoningUndetermined_2] [varchar](100) NULL,
	[AdverseEffect_2] [varchar](100) NULL,
	[Underdosing_2] [varchar](100) NULL,
	[AsofYear] [int] NULL,
	[ImportDate] [date] NULL,
	[ImportINT] [int] NULL
) ON [PRIMARY]

GO


