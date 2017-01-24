 -----------------------------------------------------------------------------------------------------------------------
-- Table:		[Registration].[FASelfPay]
-- Author:		Sumana Sangapu
-- Date:		09/14/2015
--
-- Purpose:		Holds SelfPay screen details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		Registration.Contact
--				Reference.SelfPayExceptions
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/14/2015	Sumana Sangapu	TFS# 2245 - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 03/21/2016	Kyle Campbell	Renamed PK Constraint to PK_FASelfPay_SelfPayID
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[FASelfPay](
	[SelfPayID] [bigint] NOT NULL IDENTITY (1,1),
	[ContactID] [bigint] NOT NULL,
	[DependentsServed] [smallint] NULL,
	[SlidingFeeScaleID] [bigint] NOT NULL,
	[SlidingFeePercentage] [decimal](6, 2) NULL,
	[UseRecordedUnknown] [bit] NULL,
	[MinFeeAmount] [decimal](15, 2) NULL,
	[UseMinAmount] [decimal](15, 2) NULL,
	[OOPMMF] [decimal](15, 2) NULL,
	[SelfPayExceptionID] [int] NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	 CONSTRAINT [PK_FASelfPay_SelfPayID] PRIMARY KEY CLUSTERED 
	(
		[SelfPayID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

) ON [PRIMARY]

GO

ALTER TABLE [Registration].[FASelfPay]  WITH CHECK ADD  CONSTRAINT [FK_FASelfPay_ContactID] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact]  ([ContactID])
GO

ALTER TABLE  [Registration].[FASelfPay] CHECK CONSTRAINT [FK_FASelfPay_ContactID]
GO


ALTER TABLE [Registration].[FASelfPay]  WITH CHECK ADD  CONSTRAINT [FK_FASelfPay_SelfPayExceptionID] FOREIGN KEY([SelfPayExceptionID])
REFERENCES  [Reference].[SelfPayExceptions]  ([SelfPayExceptionID])
GO

ALTER TABLE  [Registration].[FASelfPay] CHECK CONSTRAINT [FK_FASelfPay_SelfPayExceptionID]
GO

ALTER TABLE [Registration].[FASelfPay]  WITH CHECK ADD  CONSTRAINT [FK_FASelfPay_SlidingFeeScaleID] FOREIGN KEY([SlidingFeeScaleID])
REFERENCES  [Reference].[SlidingFeeScale]  ([SlidingFeeScaleID])
GO

ALTER TABLE  [Registration].[FASelfPay] CHECK CONSTRAINT [FK_FASelfPay_SlidingFeeScaleID]
GO

ALTER TABLE Registration.FASelfPay WITH CHECK ADD CONSTRAINT [FK_FASelfPay_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.FASelfPay CHECK CONSTRAINT [FK_FASelfPay_UserModifedBy]
GO
ALTER TABLE Registration.FASelfPay WITH CHECK ADD CONSTRAINT [FK_FASelfPay_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.FASelfPay CHECK CONSTRAINT [FK_FASelfPay_UserCreatedBy]
GO
