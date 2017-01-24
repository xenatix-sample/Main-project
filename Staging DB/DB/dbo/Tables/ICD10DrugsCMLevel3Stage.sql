 
 -----------------------------------------------------------------------------------------------------------------------
-- Table:	    [dbo].[ICD10DrugsCMLevel3Stage]
-- Author:		Sumana Sangapu
-- Date:		12/03/2015
--
-- Purpose:		Holds the Drugs CM Level3 info in Stage tables
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/03/2015	Sumana Sangapu	 Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[ICD10DrugsCMLevel3Stage](
	[Substance] [varchar](100) NULL,
	[Substance1] [varchar](100) NULL,
	[level2] [int] NULL,
	[Substance2] [varchar](100) NULL,
	[level3] [int] NULL,
	[Substance3] [varchar](100) NULL,
	[PoisoningAccidental_3] [varchar](100) NULL,
	[PoisoningIntentioanlSelfHarm_3] [varchar](100) NULL,
	[PoisoningAssault_3] [varchar](100) NULL,
	[PoisoningUndetermined_3] [varchar](100) NULL,
	[AdverseEffect_3] [varchar](100) NULL,
	[Underdosing_3] [varchar](100) NULL,
	[AsOfYear] [int] NULL,
	[ImportDate] [date] NULL,
	[ImportINT] [int] NULL
) ON [PRIMARY]

GO


