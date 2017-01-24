 -----------------------------------------------------------------------------------------------------------------------
-- Table:		[Registration].[FASelfPayOverrideReasons]
-- Author:		Sumana Sangapu
-- Date:		09/14/2015
--
-- Purpose:		Holds Override Reasons details per Self Pay for the Self Pay Screen
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		Reference.SelfPayOverrideReasons
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/14/2015	Sumana Sangapu	TFS# 2245 - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[FASelfPayOverrideReasons](
	[FASelfPayOverrideReasonID] [bigint] NOT NULL IDENTITY(1,1),
	[SelfPayOverrideReasonID] [int] NOT NULL,
	[SelfPayID] [bigint] NOT NULL,
	[OverrideEffectiveDate] [date] NULL,
	[OverrideExpirationDate] [date] NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	 CONSTRAINT [PK_FASelfPayOverrideReasons_FASelfPayOverrideReasonID] PRIMARY KEY CLUSTERED 
	(
		[FASelfPayOverrideReasonID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

) ON [PRIMARY]

GO

ALTER TABLE [Registration].[FASelfPayOverrideReasons]  WITH CHECK ADD  CONSTRAINT [FK_FASelfPayOverrideReasons_SelfPayOverrideReasonID] FOREIGN KEY([SelfPayOverrideReasonID])
REFERENCES [Reference].[SelfPayOverrideReasons]  ([SelfPayOverrideReasonID])
GO

ALTER TABLE [Registration].[FASelfPayOverrideReasons] CHECK CONSTRAINT [FK_FASelfPayOverrideReasons_SelfPayOverrideReasonID]
GO

ALTER TABLE [Registration].[FASelfPayOverrideReasons]  WITH CHECK ADD  CONSTRAINT [FK_FASelfPayOverrideReasons_SelfPayID] FOREIGN KEY([SelfPayID])
REFERENCES [Registration].[FASelfPay]    ([SelfPayID])
GO

ALTER TABLE [Registration].[FASelfPayOverrideReasons] CHECK CONSTRAINT [FK_FASelfPayOverrideReasons_SelfPayID]
GO

ALTER TABLE Registration.FASelfPayOverrideReasons WITH CHECK ADD CONSTRAINT [FK_FASelfPayOverrideReasons_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.FASelfPayOverrideReasons CHECK CONSTRAINT [FK_FASelfPayOverrideReasons_UserModifedBy]
GO
ALTER TABLE Registration.FASelfPayOverrideReasons WITH CHECK ADD CONSTRAINT [FK_FASelfPayOverrideReasons_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.FASelfPayOverrideReasons CHECK CONSTRAINT [FK_FASelfPayOverrideReasons_UserCreatedBy]
GO
