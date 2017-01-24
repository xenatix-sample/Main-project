-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[FeaturePermission]
-- Author:		Rajiv Ranjan
-- Date:		07/29/2015
--
-- Purpose:		Feature Permission Details
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

CREATE TABLE [Core].[FeaturePermission](
	[FeaturePermissionID] BIGINT IDENTITY(1,1) NOT NULL,
	[FeatureID] BIGINT NOT NULL,
	[PermissionID] BIGINT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [FK_FeaturePermission_FeatureID] FOREIGN KEY ([FeatureID]) REFERENCES [Core].[Feature] ([FeatureID]) ON DELETE CASCADE,
	CONSTRAINT [FK_FeaturePermission_PermissionID] FOREIGN KEY ([PermissionID]) REFERENCES [Core].[Permission] ([PermissionID]) ON DELETE CASCADE,
	CONSTRAINT [PK_FeaturePermission_FeaturePermissionID] PRIMARY KEY CLUSTERED 
	(
		[FeaturePermissionID] ASC
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

ALTER TABLE Core.FeaturePermission WITH CHECK ADD CONSTRAINT [FK_FeaturePermission_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.FeaturePermission CHECK CONSTRAINT [FK_FeaturePermission_UserModifedBy]
GO
ALTER TABLE Core.FeaturePermission WITH CHECK ADD CONSTRAINT [FK_FeaturePermission_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.FeaturePermission CHECK CONSTRAINT [FK_FeaturePermission_UserCreatedBy]
GO



