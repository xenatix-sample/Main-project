 -----------------------------------------------------------------------------------------------------------------------
-- Table:	    [dbo].[ICD10PCSBodySystemsStage]
-- Author:		Sumana Sangapu
-- Date:		12/15/2015
--
-- Purpose:		Holds the Body Systems info in Stage tables
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015	Sumana Sangapu	 Initial Creation
----------------------------------------------------------------------------------------------------------------------- 
CREATE TABLE [dbo].[ICD10PCSBodySystemsStage](
	[PCSCodeID] [int] IDENTITY(1,1) NOT NULL,
	[POS] [int] NULL,
	[Value] [varchar](100) NULL,
	[Title] [varchar](100) NULL,
	[Label] [varchar](100) NULL,
	[Code] [varchar](50) NULL,
	[LongDescription] [varchar](500) NULL,
	[AsofYear] [int] NULL
) ON [PRIMARY]

GO


