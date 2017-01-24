/****** Object:  Table [dbo].[ICD10DrugsCMLevel3Stage]    Script Date: 12/4/2015 11:26:23 AM ******/

CREATE TABLE [dbo].[ICD10NeoplasmsCMLevel3Stage](
	[Title] [varchar](100) NULL,
	[Title1] [varchar](100) NULL,
	[level2] [int] NULL,
	[Substance2] [varchar](100) NULL,
	[level3] [int] NULL,
	[Substance3] [varchar](100) NULL,
	[MalignantPrimary_3] [varchar](100) NULL,
	[MalignantSecondary_3] [varchar](100) NULL,
	[CaInSitu_3] [varchar](100) NULL,
	[Benign_3] [varchar](100) NULL,
	[UncertainBehavior_3] [varchar](100) NULL,
	[UnspecifiedBehavior_3] [varchar](100) NULL,
	[AsOfYear] [int] NULL,
	[ImportDate] [date] NULL,
	[ImportINT] [int] NULL
) ON [PRIMARY]

GO


