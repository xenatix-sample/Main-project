-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[UserEmail]
-- Author:		Rajiv Ranjan
-- Date:		
--
-- Purpose:		Mapping b/w user and email
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/21/2015	Rajiv Ranjan	 Initial creation.
-- 09/02/2015   Justin Spalti - Added the IsPrimary column and defaulted it to not null
-- 09/30/2015   Justin Spalti - Added a FK relationship for EmailPermissionID
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-- 09/06/2016	Rahul Vats		Reviewed the Table 
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Core].[UserEmail](
	[UserEmailID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[EmailID] [bigint] NULL,
	[EmailPermissionID] [int] NULL,
	[IsPrimary] [bit] NOT NULL CONSTRAINT [DF_UserEmail_IsPrimary]  DEFAULT ((0)),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__UserEmail__IsActive]  DEFAULT ((1)),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK__UserEmailID] PRIMARY KEY CLUSTERED 
(
	[UserEmailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Index [IX_UserID_EmailID_IsActive]    Script Date: 9/30/2015 6:10:58 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_UserID_EmailID_IsActive] ON [Core].[UserEmail]
(
	[UserID] ASC,
	[EmailID] ASC,
	[IsActive] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [Core].[UserEmail]  WITH CHECK ADD  CONSTRAINT [FK_UserEmail_EmailID] FOREIGN KEY([EmailID])
REFERENCES [Core].[Email] ([EmailID])
GO

ALTER TABLE [Core].[UserEmail] CHECK CONSTRAINT [FK_UserEmail_EmailID]
GO

ALTER TABLE [Core].[UserEmail]  WITH CHECK ADD  CONSTRAINT [FK_UserEmail_EmailPermissionID] FOREIGN KEY([EmailPermissionID])
REFERENCES [Reference].[EmailPermission] ([EmailPermissionID])
GO

ALTER TABLE [Core].[UserEmail] CHECK CONSTRAINT [FK_UserEmail_EmailPermissionID]
GO

ALTER TABLE [Core].[UserEmail]  WITH CHECK ADD  CONSTRAINT [FK_UserEmail_UserID] FOREIGN KEY([UserID])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Core].[UserEmail] CHECK CONSTRAINT [FK_UserEmail_UserID]
GO

ALTER TABLE Core.UserEmail WITH CHECK ADD CONSTRAINT [FK_UserEmail_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.UserEmail CHECK CONSTRAINT [FK_UserEmail_UserModifedBy]
GO
ALTER TABLE Core.UserEmail WITH CHECK ADD CONSTRAINT [FK_UserEmail_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.UserEmail CHECK CONSTRAINT [FK_UserEmail_UserCreatedBy]
GO
