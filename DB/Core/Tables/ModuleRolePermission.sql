-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[ModuleRolePermission]
-- Author:		Rajiv Ranjan
-- Date:		07/29/2015
--
-- Purpose:		Module Role Permission Details
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
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[ModuleRolePermission]
(
	[ModuleRolePermissionID] BIGINT IDENTITY(1,1) NOT NULL , 
    [RoleID] BIGINT NOT NULL , 
    [ModuleID] BIGINT NOT NULL, 
    [PermissionID] BIGINT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [FK_ModuleRolePermission_RoleID] FOREIGN KEY ([RoleID]) REFERENCES [Core].[Role] ([RoleID]) ON DELETE CASCADE,
	CONSTRAINT [FK_ModuleRolePermission_ModuleID] FOREIGN KEY ([ModuleID]) REFERENCES [Core].[Module] ([ModuleID]) ON DELETE CASCADE,
	CONSTRAINT [FK_ModuleRolePermission_PermissionID] FOREIGN KEY ([PermissionID]) REFERENCES [Core].[Permission] ([PermissionID]) ON DELETE CASCADE,
    CONSTRAINT [PK_ModuleRolePermission_ModuleRolePermissionID] PRIMARY KEY CLUSTERED 
    (
		[ModuleRolePermissionID] ASC
    )WITH 
	(
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE Core.ModuleRolePermission WITH CHECK ADD CONSTRAINT [FK_ModuleRolePermission_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ModuleRolePermission CHECK CONSTRAINT [FK_ModuleRolePermission_UserModifedBy]
GO
ALTER TABLE Core.ModuleRolePermission WITH CHECK ADD CONSTRAINT [FK_ModuleRolePermission_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ModuleRolePermission CHECK CONSTRAINT [FK_ModuleRolePermission_UserCreatedBy]
GO
