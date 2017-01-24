
-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [dbo].[DrugInformationStage]
-- Author:		Sumana Sangapu
-- Date:		11/24/2015
--
-- Purpose:		Drugs staging table
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/24/2015	Sumana Sangapu	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[DrugInformationStage](
	[DrugID] [bigint] IDENTITY(1,1) NOT NULL,
	[SetId] [uniqueidentifier] NULL,
	[Category] [varchar](100) NULL,
	[Packager] [varchar](100) NULL,
	[NDCCode] [varchar](100) NULL,
	[ProductName] [varchar](100) NULL,
	[ProductForm] [varchar](100) NULL,
	[GenericMedicine] [varchar](255) NULL,
	[Ingredients] [xml] NULL,
	[AsContents] [xml] NULL,
	[IsActive] [bit] NULL,
	[ImportDate] [date] NULL,
	[ImportDateINT] [int] NULL
CONSTRAINT [PK_DrugInformationStage] PRIMARY KEY CLUSTERED 
(
	[DrugID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

--CREATE UNIQUE NONCLUSTERED INDEX [IX_DrugInformationStage] ON [dbo].[DrugInformationStage]
--(
--	[SetId] ASC,
--	[NDCCode] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
--GO
