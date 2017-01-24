 
-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [dbo].[PackageContentsStage]
-- Author:		Sumana Sangapu
-- Date:		11/24/2015
--
-- Purpose:		PackageContents staging table
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/24/2015	Sumana Sangapu	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[PackageContentsStage](
	[PackageContentsID] [bigint] IDENTITY(1,1) NOT NULL,
	[SetID] [uniqueidentifier] NULL,
	[NumeratorValue] [varchar](50) NULL,
	[NumeratorUnit] [varchar](50) NULL,
	[DenominatorValue] [varchar](50) NULL,
	[DenominatorUnit] [varchar](50) NULL,
	[ProductCode] [varchar](50) NULL,
	[ProductCodeSystem] [varchar](50) NULL,
	[ProductFormCode] [varchar](50) NULL,
	[ProductFormCodeSystem] [varchar](50) NULL,
	[ProductFormDisplayName] [varchar](50) NULL,
	[IsActive] [bit] NULL,
	[ImportDate] [date] NULL,
	[ImportDateINT] [int] NULL
CONSTRAINT [PK_PackageContentsStage] PRIMARY KEY CLUSTERED 
(
	[PackageContentsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

--CREATE UNIQUE NONCLUSTERED INDEX [IX_PackageContentsStage] ON [dbo].[PackageContentsStage]
--(
--	[SetID] ASC,
--	[ProductCode] ASC,
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
--GO