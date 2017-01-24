-----------------------------------------------------------------------------------------------------------------------
-- Table:		Clinical.[DrugPackageContents]
-- Author:		Sumana Sangapu	
-- Date:		12/10/2015
--
-- Purpose:		Store  PackageContents details of the drugs
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/10/2015	Sumana Sangapu	Initial Creation

------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Clinical].[DrugPackageContents](
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
	[ImportDateINT] [int] NULL,
 CONSTRAINT [PK_DrugPackageContents] PRIMARY KEY CLUSTERED 
(
	[PackageContentsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO