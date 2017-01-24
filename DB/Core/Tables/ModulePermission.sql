﻿-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[ModulePermission]
-- Author:		Rajiv Ranjan
-- Date:		07/29/2015
--
-- Purpose:		Module Permission Details
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

CREATE TABLE [Core].[ModulePermission] (
    [ModulePermissionID] BIGINT      IDENTITY (1, 1) NOT NULL,
    [ModuleID]           BIGINT      NULL,
    [PermissionID]       BIGINT      NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [FK_ModulePermission_PermissionID] FOREIGN KEY ([PermissionID]) REFERENCES [Core].[Permission] ([PermissionID]) ON DELETE CASCADE,
	CONSTRAINT [FK_ModulePermission_ModuleID] FOREIGN KEY ([ModuleID]) REFERENCES [Core].[Module] ([ModuleID]) ON DELETE CASCADE,
    CONSTRAINT [PK_ModulePermission_ModulePermissionID] PRIMARY KEY CLUSTERED ([ModulePermissionID] ASC)
	WITH (
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE Core.ModulePermission WITH CHECK ADD CONSTRAINT [FK_ModulePermission_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ModulePermission CHECK CONSTRAINT [FK_ModulePermission_UserModifedBy]
GO
ALTER TABLE Core.ModulePermission WITH CHECK ADD CONSTRAINT [FK_ModulePermission_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ModulePermission CHECK CONSTRAINT [FK_ModulePermission_UserCreatedBy]
GO





