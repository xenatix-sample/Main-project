-----------------------------------------------------------------------------------------------------------------------
-- Table:		Clinical.[DrugIngredients]
-- Author:		Sumana Sangapu	
-- Date:		12/10/2015
--
-- Purpose:		Store Ingredient details of the drugs
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/10/2015	Sumana Sangapu	Initial Creation

------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Clinical].[DrugIngredients](
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
	[ImportDateINT] [int] NULL,
 CONSTRAINT [PK_DrugIngredients] PRIMARY KEY CLUSTERED 
(
	[IngredientsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


