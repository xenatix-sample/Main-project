 -----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[SlidingFeeScale]
-- Author:		Sumana Sangapu
-- Date:		09/14/2015
--
-- Purpose:		Holds Sliding Fee Scales for all programs. This information is used to obtain the fee in the Self Pay screen.
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		Reference.Program
--				
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/14/2015	Sumana Sangapu	TFS# 2245 - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-----------------------------------------------------------------------------------------------------------------------


CREATE TABLE [Reference].[SlidingFeeScale](
	[SlidingFeeScaleID] [bigint] NOT NULL IDENTITY(1,1),
	[ProgramID] [int] NOT NULL,
	[EffectiveDate] [date] NOT NULL,
	[FamilySize] [int] NOT NULL,
	[StartingScale] [decimal](15, 0) NULL,
	[EndingScale] [decimal](15, 0) NULL,
	[MonthlyMaximumCharge] [decimal](15, 2) NOT NULL,
	[FinanceFrequencyID] [int] NULL,
	[FPL] [nvarchar](255) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [PK_SlidingFeeScale_SlidingFeeScaleID] PRIMARY KEY CLUSTERED 
	(
		[SlidingFeeScaleID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]


) ON [PRIMARY]

GO

ALTER TABLE [Reference].[SlidingFeeScale]  WITH CHECK ADD  CONSTRAINT [FK_SlidingFeeScale_ProgramID] FOREIGN KEY([ProgramID])
REFERENCES [Reference].[Program]  ([ProgramID])
GO

ALTER TABLE [Reference].[SlidingFeeScale] CHECK CONSTRAINT [FK_SlidingFeeScale_ProgramID]
GO

ALTER TABLE [Reference].[SlidingFeeScale]  WITH CHECK ADD  CONSTRAINT [FK_SlidingFeeScale_FinanceFrequencyID] FOREIGN KEY([FinanceFrequencyID])
REFERENCES [Reference].[FinanceFrequency]   ([FinanceFrequencyID])
GO

ALTER TABLE [Reference].[SlidingFeeScale] CHECK CONSTRAINT [FK_SlidingFeeScale_FinanceFrequencyID]
GO

ALTER TABLE Reference.SlidingFeeScale WITH CHECK ADD CONSTRAINT [FK_SlidingFeeScale_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.SlidingFeeScale CHECK CONSTRAINT [FK_SlidingFeeScale_UserModifedBy]
GO
ALTER TABLE Reference.SlidingFeeScale WITH CHECK ADD CONSTRAINT [FK_SlidingFeeScale_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.SlidingFeeScale CHECK CONSTRAINT [FK_SlidingFeeScale_UserCreatedBy]
GO
