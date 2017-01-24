 
-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [dbo].[IngredientsStage]
-- Author:		Sumana Sangapu
-- Date:		11/24/2015
--
-- Purpose:		Ingredients staging table
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/24/2015	Sumana Sangapu	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[IngredientsStage](
	[IngredientsID] [bigint] IDENTITY(1,1) NOT NULL,
	[SetID] [uniqueidentifier] NULL,
	[ClassCode] [varchar](50) NULL,
	[NumeratorValue] [varchar](50) NULL,
	[NumeratorUnit] [varchar](50) NULL,
	[DenominatorValue] [varchar](50) NULL,
	[DenominatorUnit] [varchar](50) NULL,
	[SubstanceCode] [varchar](50) NULL,
	[SubstanceName] [varchar](50) NULL,
	[IsActive] [bit] NULL,
	[ImportDate] [date] NULL,
	[ImportDateINT] [int] NULL
CONSTRAINT [PK_IngredientsStage] PRIMARY KEY CLUSTERED 
(
	[IngredientsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

--CREATE UNIQUE NONCLUSTERED INDEX [IX_IngredientsStage] ON [dbo].[IngredientsStage]
--(
--	[SetId] ASC,
--	[ClassCode] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
--GO

