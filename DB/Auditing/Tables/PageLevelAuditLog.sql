
-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Auditing].[PageLevelAuditLog]
-- Author:		Sumana Sangapu
-- Date:		12/12/2016
--
-- Purpose:		Holds the PageLevel Audit Details of the user
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/12/2016	Sumana Sangapu  Initial creation.
-- 12/15/2016	Sumana Sangapu	Added ModifiedBy as part of the DB table definition prototype
-- 12/20/2016	Sumana Sangapu	Add SearchText
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Auditing].[PageLevelAuditLog](
	[PageLevelAuditLogID] [bigint] IDENTITY(1,1) NOT NULL,
	[TransactionLogID] [bigint] NULL,
	[UserID] [int] NOT NULL,
	[ContactID] [bigint] NULL,
	[ModuleComponentID] [bigint] NOT NULL,
	[UserCredentials] [nvarchar](4000) NULL,
	[UserRoles] [nvarchar](4000) NOT NULL,
	[SearchText] nvarchar(4000) NULL,
	[IsCareMember] [bit] NULL,
	[IsBreaktheGlassEnabled] [bit] NULL,
	[ActionTypeID] [int] NOT NULL,
    [ModifiedBy] INT NOT NULL,
	[CreatedOn] [datetime] NOT NULL
 CONSTRAINT [PK_PageLevelAuditLogID] PRIMARY KEY CLUSTERED 
(
	[PageLevelAuditLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
) ON [PRIMARY]

GO

ALTER TABLE [Auditing].[PageLevelAuditLog] WITH CHECK ADD  CONSTRAINT [FK_PageLevelAuditLog_UserID] FOREIGN KEY([UserID])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Auditing].[PageLevelAuditLog] CHECK CONSTRAINT [FK_PageLevelAuditLog_UserID]
GO

ALTER TABLE [Auditing].[PageLevelAuditLog]  WITH CHECK ADD  CONSTRAINT [FK_PageLevelAuditLog_ContactID] FOREIGN KEY([ContactID])
REFERENCES Registration.Contact ([ContactID])
GO

ALTER TABLE [Auditing].[PageLevelAuditLog] CHECK CONSTRAINT [FK_PageLevelAuditLog_ContactID]
GO

ALTER TABLE [Auditing].[PageLevelAuditLog]  WITH CHECK ADD  CONSTRAINT [FK_PageLevelAuditLog_ComponentID] FOREIGN KEY([ModuleComponentID])
REFERENCES [Core].[ModuleComponent] ([ModuleComponentID])
GO

ALTER TABLE [Auditing].[PageLevelAuditLog] CHECK CONSTRAINT [FK_PageLevelAuditLog_ComponentID]
GO

ALTER TABLE [Auditing].[PageLevelAuditLog]  WITH CHECK ADD  CONSTRAINT [FK_PageLevelAuditLog_ActionTypeID] FOREIGN KEY([ActionTypeID])
REFERENCES Reference.ActionType ([ActionTypeID])
GO

ALTER TABLE [Auditing].[PageLevelAuditLog] CHECK CONSTRAINT [FK_PageLevelAuditLog_ActionTypeID]
GO

ALTER TABLE [Auditing].[PageLevelAuditLog]  WITH CHECK ADD  CONSTRAINT [FK_PageLevelAuditLog_ModifiedBy] FOREIGN KEY([ModifiedBy])
REFERENCES Core.Users ([UserID])
GO

ALTER TABLE [Auditing].[PageLevelAuditLog] CHECK CONSTRAINT [FK_PageLevelAuditLog_ModifiedBy]
GO
EXEC sys.sp_addextendedproperty @name=N'Caption', @value=N'Action Type' , @level0type=N'SCHEMA',@level0name=N'Auditing', @level1type=N'TABLE',@level1name=N'PageLevelAuditLog'
GO

EXEC sys.sp_addextendedproperty @name=N'IsOptionSet', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'Auditing', @level1type=N'TABLE',@level1name=N'PageLevelAuditLog'
GO

EXEC sys.sp_addextendedproperty @name=N'IsUserOptionSet', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'Auditing', @level1type=N'TABLE',@level1name=N'PageLevelAuditLog'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Holds the data for PageLevel Audit by the user' , @level0type=N'SCHEMA',@level0name=N'Auditing', @level1type=N'TABLE',@level1name=N'PageLevelAuditLog'
GO