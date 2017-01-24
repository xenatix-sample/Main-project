
CREATE TABLE [dbo].[ICD10NeoplasmsCMLevel1Stage](
	[Substance] [varchar](100) NULL,
	[MalignantPrimary] [varchar](100) NULL,
	[MalignantSecondary] [varchar](100) NULL,
	[CaInSitu] [varchar](50) NULL,
	[Benign] [varchar](50) NULL,
	[UncertainBehavior] [varchar](50) NULL,
	[UnspecifiedBehavior] [varchar](50) NULL,
	[level1] [int] NULL,
	[MalignantPrimary_1] [varchar](100) NULL,
	[MalignantSecondary_1] [varchar](100) NULL,
	[CaInSitu_1] [varchar](100) NULL,
	[Benign_1] [varchar](100) NULL,
	[UncertainBehavior_1] [varchar](100) NULL,
	[UnspecifiedBehavior_1] [varchar](100) NULL,
	[AsofYear] [int] NULL,
	[ImportDate] [date] NULL,
	[ImportINT] [int] NULL
) ON [PRIMARY]

GO
