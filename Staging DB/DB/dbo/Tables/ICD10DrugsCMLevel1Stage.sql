
 -----------------------------------------------------------------------------------------------------------------------
-- Table:	    [dbo].[ICD10DrugsCMLevel1Stage]
-- Author:		Sumana Sangapu
-- Date:		12/03/2015
--
-- Purpose:		Holds the Drugs CM Level1 info in Stage tables
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/03/2015	Sumana Sangapu	 Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[ICD10DrugsCMLevel1Stage](
	[Substance] [varchar](100) NULL,
	[PoisoningAccidental] [varchar](100) NULL,
	[PoisoningIntentioanlSelfHarm] [varchar](100) NULL,
	[PoisoningAssault] [varchar](50) NULL,
	[PoisoningUndetermined] [varchar](50) NULL,
	[AdverseEffect] [varchar](50) NULL,
	[Underdosing] [varchar](50) NULL,
	[level1] [int] NULL,
	[PoisoningAccidental_1] [varchar](100) NULL,
	[PoisoningIntentioanlSelfHarm_1] [varchar](100) NULL,
	[PoisoningAssault_1] [varchar](100) NULL,
	[PoisoningUndetermined_1] [varchar](100) NULL,
	[AdverseEffect_1] [varchar](100) NULL,
	[Underdosing_1] [varchar](100) NULL,
	[AsofYear] [int] NULL,
	[ImportDate] [date] NULL,
	[ImportINT] [int] NULL
) ON [PRIMARY]

GO