-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[RoleModuleComponent]
-- Author:		Scott Martin
-- Date:		05/13/2016
--
-- Purpose:		Role Module Component Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/13/2016	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[RoleModuleComponent](
	[RoleModuleComponentID] BIGINT IDENTITY(1,1) NOT NULL,
	[RoleModuleID] BIGINT NOT NULL,
	[ModuleComponentID] BIGINT NOT NULL,
	[PermissionLevelID] INT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()), 
    CONSTRAINT [PK_RoleModuleComponent_RoleModuleComponentID] PRIMARY KEY CLUSTERED 
	( 
		[RoleModuleComponentID] ASC
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
ALTER TABLE Core.RoleModuleComponent WITH CHECK ADD CONSTRAINT [FK_RoleModuleComponent_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.RoleModuleComponent CHECK CONSTRAINT [FK_RoleModuleComponent_UserModifedBy]
GO
ALTER TABLE Core.RoleModuleComponent WITH CHECK ADD CONSTRAINT [FK_RoleModuleComponent_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.RoleModuleComponent CHECK CONSTRAINT [FK_RoleModuleComponent_UserCreatedBy]
GO
ALTER TABLE Core.RoleModuleComponent WITH CHECK ADD CONSTRAINT [FK_RoleModuleComponent_RoleModuleID] FOREIGN KEY ([RoleModuleID]) REFERENCES [Core].[RoleModule] ([RoleModuleID])
GO
ALTER TABLE Core.RoleModuleComponent CHECK CONSTRAINT [FK_RoleModuleComponent_RoleModuleID]
GO
ALTER TABLE Core.RoleModuleComponent WITH CHECK ADD CONSTRAINT [FK_RoleModuleComponent_ModuleComponentID] FOREIGN KEY ([ModuleComponentID]) REFERENCES [Core].[ModuleComponent] ([ModuleComponentID])
GO
ALTER TABLE Core.RoleModuleComponent CHECK CONSTRAINT [FK_RoleModuleComponent_ModuleComponentID]
GO
ALTER TABLE Core.RoleModuleComponent WITH CHECK ADD CONSTRAINT [FK_RoleModuleComponent_PermissionLevelID] FOREIGN KEY ([PermissionLevelID]) REFERENCES [Core].[PermissionLevel] ([PermissionLevelID])
GO
ALTER TABLE Core.RoleModuleComponent CHECK CONSTRAINT [FK_RoleModuleComponent_PermissionLevelID]
GO

CREATE NONCLUSTERED INDEX [IX_RoleModuleComponent_RoleModuleID_ModuleComponentID_PermissionLevelID] ON [Core].[RoleModuleComponent]
(
	[RoleModuleID] ASC,
	[ModuleComponentID] ASC,
	[ModifiedBy] ASC,
	[PermissionLevelID] ASC,
	[CreatedBy] ASC
)
INCLUDE ( 	[RoleModuleComponentID]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_RoleModuleComponent_RoleModuleID_IsActive] ON [Core].[RoleModuleComponent]
(
	[RoleModuleID] ASC,
	[IsActive] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_RoleModuleComponent_IsActive_RoleModuleID] ON [Core].[RoleModuleComponent]
(
	[IsActive] ASC,
	[RoleModuleID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO