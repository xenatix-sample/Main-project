-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Registration].[[ReferralContact]]
-- Author:		John Crossen
-- Date:		10/08/2015
--
-- Purpose:		Lookup for [[ReferralContact] details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/08/2015	John Crossen	TFS# 2661 - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[ReferralContact](
	[ReferralContactID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[ReferralID] [BIGINT] NOT NULL,
	[ContactID] [BIGINT] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ReferralContact] PRIMARY KEY CLUSTERED 
(
	[ReferralContactID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Registration].[ReferralContact]  WITH CHECK ADD  CONSTRAINT [FK_ReferralContact_Contact] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [Registration].[ReferralContact] CHECK CONSTRAINT [FK_ReferralContact_Contact]
GO

CREATE UNIQUE NONCLUSTERED INDEX [UI_ReferralContact_ContactID_ReferralID] ON [Registration].[ReferralContact]
(
	[ReferralID] ASC,
	[ContactID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE Registration.ReferralContact WITH CHECK ADD CONSTRAINT [FK_ReferralContact_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ReferralContact CHECK CONSTRAINT [FK_ReferralContact_UserModifedBy]
GO
ALTER TABLE Registration.ReferralContact WITH CHECK ADD CONSTRAINT [FK_ReferralContact_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ReferralContact CHECK CONSTRAINT [FK_ReferralContact_UserCreatedBy]
GO



