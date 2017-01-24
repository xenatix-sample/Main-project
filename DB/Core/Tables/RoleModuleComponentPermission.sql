-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[RoleModuleComponentPermission]
-- Author:		Scott Martin
-- Date:		05/13/2016
--
-- Purpose:		Role Module Component Permission Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/13/2016	Scott Martin	Initial Creation
-- 05/20/2016	Scott Martin	Changed PermissionLevelID and PermissionID to nullable
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[RoleModuleComponentPermission](
	[RoleModuleComponentPermissionID] BIGINT IDENTITY(1,1) NOT NULL,
	[RoleModuleComponentID] BIGINT NOT NULL,
	[PermissionLevelID] INT NULL,
	[PermissionID] BIGINT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()), 
    CONSTRAINT [PK_RoleModuleComponentPermission_RoleModuleComponentPermissionID] PRIMARY KEY CLUSTERED 
	( 
		[RoleModuleComponentPermissionID] ASC
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
ALTER TABLE Core.RoleModuleComponentPermission WITH CHECK ADD CONSTRAINT [FK_RoleModuleComponentPermission_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.RoleModuleComponentPermission CHECK CONSTRAINT [FK_RoleModuleComponentPermission_UserModifedBy]
GO
ALTER TABLE Core.RoleModuleComponentPermission WITH CHECK ADD CONSTRAINT [FK_RoleModuleComponentPermission_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.RoleModuleComponentPermission CHECK CONSTRAINT [FK_RoleModuleComponentPermission_UserCreatedBy]
GO
ALTER TABLE Core.RoleModuleComponentPermission WITH CHECK ADD CONSTRAINT [FK_RoleModuleComponentPermission_RoleModuleComponentID] FOREIGN KEY ([RoleModuleComponentID]) REFERENCES [Core].[RoleModuleComponent] ([RoleModuleComponentID])
GO
ALTER TABLE Core.RoleModuleComponentPermission CHECK CONSTRAINT [FK_RoleModuleComponentPermission_RoleModuleComponentID]
GO
ALTER TABLE Core.RoleModuleComponentPermission WITH CHECK ADD CONSTRAINT [FK_RoleModuleComponentPermission_PermissionLevelID] FOREIGN KEY ([PermissionLevelID]) REFERENCES [Core].[PermissionLevel] ([PermissionLevelID])
GO
ALTER TABLE Core.RoleModuleComponentPermission CHECK CONSTRAINT [FK_RoleModuleComponentPermission_PermissionLevelID]
GO
ALTER TABLE Core.RoleModuleComponentPermission WITH CHECK ADD CONSTRAINT [FK_RoleModuleComponentPermission_PermissionID] FOREIGN KEY ([PermissionID]) REFERENCES [Core].[Permission] ([PermissionID])
GO
ALTER TABLE Core.RoleModuleComponentPermission CHECK CONSTRAINT [FK_RoleModuleComponentPermission_PermissionID]
GO

CREATE NONCLUSTERED INDEX [IX_RoleModuleComponentPermission_RoleModuleComponentID_PermissionID_PermissionLevelID] ON [Core].[RoleModuleComponentPermission]
(
	[RoleModuleComponentID] ASC,
	[PermissionID] ASC,
	[PermissionLevelID] ASC,
	[CreatedBy] ASC,
	[ModifiedBy] ASC
)
INCLUDE ( 	[RoleModuleComponentPermissionID]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_RoleModuleComponentPermission_IsActive_PermissionLevelID_PermissionID] ON [Core].[RoleModuleComponentPermission]
(
	[IsActive] ASC,
	[PermissionLevelID] ASC,
	[PermissionID] ASC,
	[RoleModuleComponentID] ASC
)
INCLUDE ( 	[RoleModuleComponentPermissionID]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO