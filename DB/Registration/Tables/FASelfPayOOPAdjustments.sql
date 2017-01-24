 -----------------------------------------------------------------------------------------------------------------------
-- Table:		[Registration].[FASelfPayOOPAdjustments]
-- Author:		Sumana Sangapu
-- Date:		09/14/2015
--
-- Purpose:		Holds OOP Adjustment details per Self Pay for the Self Pay Screen
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		Reference.SelfPayOOPAdjustments
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/14/2015	Sumana Sangapu	TFS# 2245 - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[FASelfPayOOPAdjustments](
	[FASelfPayOOPAdjustmentID] [bigint] NOT NULL IDENTITY(1,1),
	[SelfPayOOPAdjustmentID] [int] NOT NULL,
	[SelfPayID] [bigint] NOT NULL,
	[OOPAdjEffectiveDate] [date] NOT NULL,
	[OOPAdjExpirationDate] [date] NULL,
	[OOPComments] [nvarchar](1000) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	 CONSTRAINT [PK_FASelfPayOOPAdjustments_FASelfPayOOPAdjustmentID] PRIMARY KEY CLUSTERED 
	(
		[FASelfPayOOPAdjustmentID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

) ON [PRIMARY]

GO

ALTER TABLE [Registration].[FASelfPayOOPAdjustments]  WITH CHECK ADD  CONSTRAINT [FK_FASelfPayOOPAdjustments_OOPAdjustmentID] FOREIGN KEY([SelfPayOOPAdjustmentID])
REFERENCES [Reference].[SelfPayOOPAdjustments]  ([SelfPayOOPAdjustmentID])
GO

ALTER TABLE [Registration].[FASelfPayOOPAdjustments] CHECK CONSTRAINT [FK_FASelfPayOOPAdjustments_OOPAdjustmentID]
GO

ALTER TABLE [Registration].[FASelfPayOOPAdjustments]  WITH CHECK ADD  CONSTRAINT [FK_FASelfPayOOPAdjustments_SelfPayID] FOREIGN KEY([SelfPayID])
REFERENCES [Registration].[FASelfPay]    ([SelfPayID])
GO

ALTER TABLE [Registration].[FASelfPayOOPAdjustments] CHECK CONSTRAINT [FK_FASelfPayOOPAdjustments_SelfPayID]
GO

ALTER TABLE Registration.FASelfPayOOPAdjustments WITH CHECK ADD CONSTRAINT [FK_FASelfPayOOPAdjustments_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.FASelfPayOOPAdjustments CHECK CONSTRAINT [FK_FASelfPayOOPAdjustments_UserModifedBy]
GO
ALTER TABLE Registration.FASelfPayOOPAdjustments WITH CHECK ADD CONSTRAINT [FK_FASelfPayOOPAdjustments_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.FASelfPayOOPAdjustments CHECK CONSTRAINT [FK_FASelfPayOOPAdjustments_UserCreatedBy]
GO
