-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[ModuleComponentPermission]
-- Author:		Scott Martin
-- Date:		07/18/2016
--
-- Purpose:		Module Component Permission Details
--
-- Notes:		Used to indicate the default permission values for components
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/18/2016	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[ModuleComponentPermission](
	[ModuleComponentPermissionID] BIGINT IDENTITY(1,1) NOT NULL,
	[ModuleComponentID] BIGINT NOT NULL,
	[PermissionLevelID] INT NULL,
	[PermissionID] BIGINT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()), 
    CONSTRAINT [PK_ModuleComponentPermission_ModuleComponentPermissionID] PRIMARY KEY CLUSTERED 
	( 
		[ModuleComponentPermissionID] ASC
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
ALTER TABLE Core.ModuleComponentPermission WITH CHECK ADD CONSTRAINT [FK_ModuleComponentPermission_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ModuleComponentPermission CHECK CONSTRAINT [FK_ModuleComponentPermission_UserModifedBy]
GO
ALTER TABLE Core.ModuleComponentPermission WITH CHECK ADD CONSTRAINT [FK_ModuleComponentPermission_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ModuleComponentPermission CHECK CONSTRAINT [FK_ModuleComponentPermission_UserCreatedBy]
GO
ALTER TABLE Core.ModuleComponentPermission WITH CHECK ADD CONSTRAINT [FK_ModuleComponentPermission_ModuleComponentID] FOREIGN KEY ([ModuleComponentID]) REFERENCES [Core].[ModuleComponent] ([ModuleComponentID])
GO
ALTER TABLE Core.ModuleComponentPermission CHECK CONSTRAINT [FK_ModuleComponentPermission_ModuleComponentID]
GO
ALTER TABLE Core.ModuleComponentPermission WITH CHECK ADD CONSTRAINT [FK_ModuleComponentPermission_PermissionLevelID] FOREIGN KEY ([PermissionLevelID]) REFERENCES [Core].[PermissionLevel] ([PermissionLevelID])
GO
ALTER TABLE Core.ModuleComponentPermission CHECK CONSTRAINT [FK_ModuleComponentPermission_PermissionLevelID]
GO
ALTER TABLE Core.ModuleComponentPermission WITH CHECK ADD CONSTRAINT [FK_ModuleComponentPermission_PermissionID] FOREIGN KEY ([PermissionID]) REFERENCES [Core].[Permission] ([PermissionID])
GO
ALTER TABLE Core.ModuleComponentPermission CHECK CONSTRAINT [FK_ModuleComponentPermission_PermissionID]
GO