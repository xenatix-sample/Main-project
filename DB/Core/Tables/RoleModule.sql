-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[RoleModule]
-- Author:		Rajiv Ranjan
-- Date:		07/29/2015
--
-- Purpose:		Role Module Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/29/2015	Rajiv Ranjan	 Initial creation.
-- 07/30/2015   John Crossen     Change schema from dbo to Core
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn	
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-- 05/13/2016	Scott Martin	Added PermissionLevelID and constraints for RoleID and ModuleID
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[RoleModule](
	[RoleModuleID] BIGINT IDENTITY(1,1) NOT NULL,
	[RoleID] BIGINT NOT NULL,
	[ModuleID] BIGINT NOT NULL,
	[PermissionLevelID] INT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()), 
    CONSTRAINT [PK_RoleModule_RoleModuleID] PRIMARY KEY CLUSTERED 
	( 
		[RoleModuleID] ASC
    )
	WITH 
	(
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON,
		 ALLOW_PAGE_LOCKS = ON
	 ) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE Core.RoleModule WITH CHECK ADD CONSTRAINT [FK_RoleModule_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.RoleModule CHECK CONSTRAINT [FK_RoleModule_UserModifedBy]
GO
ALTER TABLE Core.RoleModule WITH CHECK ADD CONSTRAINT [FK_RoleModule_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.RoleModule CHECK CONSTRAINT [FK_RoleModule_UserCreatedBy]
GO
ALTER TABLE Core.RoleModule WITH CHECK ADD CONSTRAINT [FK_RoleModule_RoleID] FOREIGN KEY ([RoleID]) REFERENCES [Core].[Role] ([RoleID])
GO
ALTER TABLE Core.RoleModule CHECK CONSTRAINT [FK_RoleModule_RoleID]
GO
ALTER TABLE Core.RoleModule WITH CHECK ADD CONSTRAINT [FK_RoleModule_ModuleID] FOREIGN KEY ([ModuleID]) REFERENCES [Core].[Module] ([ModuleID])
GO
ALTER TABLE Core.RoleModule CHECK CONSTRAINT [FK_RoleModule_ModuleID]
GO
ALTER TABLE Core.RoleModule WITH CHECK ADD CONSTRAINT [FK_RoleModule_PermissionLevelID] FOREIGN KEY ([PermissionLevelID]) REFERENCES [Core].[PermissionLevel] ([PermissionLevelID])
GO
ALTER TABLE Core.RoleModule CHECK CONSTRAINT [FK_RoleModule_PermissionLevelID]
GO

CREATE NONCLUSTERED INDEX [IX_RoleModule_IsActive_RoleID] ON [Core].[RoleModule]
(
	[IsActive] ASC,
	[RoleID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO