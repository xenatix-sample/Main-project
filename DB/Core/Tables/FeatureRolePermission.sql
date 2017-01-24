-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[FeatureRolePermission]
-- Author:		Rajiv Ranjan
-- Date:		07/29/2015
--
-- Purpose:		Feature Role Permission Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/29/2015	Rajiv Ranjan	 Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[FeatureRolePermission](
	[FeatureRolePermissionID] BIGINT IDENTITY(1,1) NOT NULL,
	[RoleID] BIGINT NOT NULL,
	[FeatureID] BIGINT NOT NULL,
	[PermissionID] [int] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [FK_FeatureRolePermission_FeatureID] FOREIGN KEY ([FeatureID]) REFERENCES [Core].[Feature] ([FeatureID]) ON DELETE CASCADE,
	CONSTRAINT [FK_FeatureRolePermission_RoleID] FOREIGN KEY ([RoleID]) REFERENCES [Core].[Role] ([RoleID]) ON DELETE CASCADE,
	CONSTRAINT [PK_FeatureRolePermission_FeatureRolePermissionID] PRIMARY KEY CLUSTERED ([FeatureRolePermissionID] ASC)
	WITH (
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
	
) ON [PRIMARY]

GO

CREATE INDEX [IX_FEatureRolePermission_RoleID_FeatureID_IsActive] ON [Core].[FeatureRolePermission] ([RoleID], [FeatureID], [IsActive]) INCLUDE ([PermissionID])
GO

ALTER TABLE Core.FeatureRolePermission WITH CHECK ADD CONSTRAINT [FK_FeatureRolePermission_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.FeatureRolePermission CHECK CONSTRAINT [FK_FeatureRolePermission_UserModifedBy]
GO
ALTER TABLE Core.FeatureRolePermission WITH CHECK ADD CONSTRAINT [FK_FeatureRolePermission_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.FeatureRolePermission CHECK CONSTRAINT [FK_FeatureRolePermission_UserCreatedBy]
GO

