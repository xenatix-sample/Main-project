-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[RoleModulePermission]
-- Author:		Scott Martin
-- Date:		05/13/2016
--
-- Purpose:		Role Module Permission Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/13/2016	Scott Martin	Initial Creation
-- 05/20/2016	Scott Martin	Changed PermissionLevelID and PermissionID to nullable
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[RoleModulePermission](
	[RoleModulePermissionID] BIGINT IDENTITY(1,1) NOT NULL,
	[RoleModuleID] BIGINT NOT NULL,
	[PermissionLevelID] INT NULL,
	[PermissionID] BIGINT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()), 
    CONSTRAINT [PK_RoleModulePermission_RoleModulePermissionID] PRIMARY KEY CLUSTERED 
	( 
		[RoleModulePermissionID] ASC
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
ALTER TABLE Core.RoleModulePermission WITH CHECK ADD CONSTRAINT [FK_RoleModulePermission_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.RoleModulePermission CHECK CONSTRAINT [FK_RoleModulePermission_UserModifedBy]
GO
ALTER TABLE Core.RoleModulePermission WITH CHECK ADD CONSTRAINT [FK_RoleModulePermission_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.RoleModulePermission CHECK CONSTRAINT [FK_RoleModulePermission_UserCreatedBy]
GO
ALTER TABLE Core.RoleModulePermission WITH CHECK ADD CONSTRAINT [FK_RoleModulePermission_RoleModuleID] FOREIGN KEY ([RoleModuleID]) REFERENCES [Core].[RoleModule] ([RoleModuleID])
GO
ALTER TABLE Core.RoleModulePermission CHECK CONSTRAINT [FK_RoleModulePermission_RoleModuleID]
GO
ALTER TABLE Core.RoleModulePermission WITH CHECK ADD CONSTRAINT [FK_RoleModulePermission_PermissionLevelID] FOREIGN KEY ([PermissionLevelID]) REFERENCES [Core].[PermissionLevel] ([PermissionLevelID])
GO
ALTER TABLE Core.RoleModulePermission CHECK CONSTRAINT [FK_RoleModulePermission_PermissionLevelID]
GO
ALTER TABLE Core.RoleModulePermission WITH CHECK ADD CONSTRAINT [FK_RoleModulePermission_PermissionID] FOREIGN KEY ([PermissionID]) REFERENCES [Core].[Permission] ([PermissionID])
GO
ALTER TABLE Core.RoleModulePermission CHECK CONSTRAINT [FK_RoleModulePermission_PermissionID]
GO

CREATE NONCLUSTERED INDEX [IX_ModuleRolePermission_ModuleID_PermissionID_RoleID] ON [Core].[ModuleRolePermission]
(
	[ModuleID] ASC,
	[PermissionID] ASC,
	[RoleID] ASC,
	[CreatedBy] ASC,
	[ModifiedBy] ASC
)
INCLUDE ( 	[ModuleRolePermissionID]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_RoleModulePermission_RoleModuleID_IsActive] ON [Core].[RoleModulePermission]
(
	[RoleModuleID] ASC,
	[IsActive] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO