-----------------------------------------------------------------------------------------------------------------------
-- Table:		[dbo].[UserRole]
-- Author:		Rajiv Ranjan
-- Date:		
--
-- Purpose:		Store the token generated for user
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- MM/DD/YYYY	Rajiv Ranjan	 Initial creation.
-- 07/30/2015	Suresh Pandey	TFS# - changed modified by and modified on not null
-- 07/30/2015   John Crossen     Change schema from dbo to Core
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-- 09/06/2016	Rahul Vats		Reviewed the Table 
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Core].[UserRole](
	[UserRoleID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[RoleID] [bigint] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_dbo.UserRole] PRIMARY KEY CLUSTERED 
(
	[UserRoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Core].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserRole_Role_RoleId] FOREIGN KEY([RoleID])
REFERENCES [Core].[Role] ([RoleID])
ON DELETE CASCADE
GO

ALTER TABLE [Core].[UserRole] CHECK CONSTRAINT [FK_dbo.UserRole_Role_RoleId]
GO

ALTER TABLE [Core].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserRole_User_UserID] FOREIGN KEY([UserID])
REFERENCES [Core].[Users] ([UserID])
ON DELETE CASCADE
GO

ALTER TABLE [Core].[UserRole] CHECK CONSTRAINT [FK_dbo.UserRole_User_UserID]
GO

ALTER TABLE Core.UserRole WITH CHECK ADD CONSTRAINT [FK_UserRole_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.UserRole CHECK CONSTRAINT [FK_UserRole_UserModifedBy]
GO
ALTER TABLE Core.UserRole WITH CHECK ADD CONSTRAINT [FK_UserRole_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.UserRole CHECK CONSTRAINT [FK_UserRole_UserCreatedBy]
GO

CREATE NONCLUSTERED INDEX [IX_UserRole_UserID_IsActive_RoleId] ON [Core].[UserRole]
(
	[UserID] ASC,
	[IsActive] ASC,
	[RoleID] ASC
)
INCLUDE ( 	[UserRoleID]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO