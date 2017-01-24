-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Core.CredentialPermission
-- Author:		John Crossen
-- Date:		08/12/2014
--
-- Purpose:		Provide a brief description of what your function does.
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		Reference.Credentials, Core.Permission
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/12/2014	John Crossen		TFS# 885 - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[CredentialPermission](
	[CredentialPermissionID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[CredentialID] [BIGINT] NOT NULL,
	[PermissionID] [BIGINT] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_CredentialPermission_FeaturePermissionID] PRIMARY KEY CLUSTERED 
(
	[CredentialPermissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Core].[CredentialPermission]  WITH CHECK ADD  CONSTRAINT [FK_CredentialPermission_CredentialID] FOREIGN KEY([CredentialID])
REFERENCES [Reference].[Credentials] ([CredentialID])
ON DELETE CASCADE
GO

ALTER TABLE [Core].[CredentialPermission] CHECK CONSTRAINT [FK_CredentialPermission_CredentialID]
GO

ALTER TABLE [Core].[CredentialPermission]  WITH CHECK ADD  CONSTRAINT [FK_CredentialPermission_PermissionID] FOREIGN KEY([PermissionID])
REFERENCES [Core].[Permission] ([PermissionID])
ON DELETE CASCADE
GO

ALTER TABLE [Core].[CredentialPermission] CHECK CONSTRAINT [FK_CredentialPermission_PermissionID]
GO

ALTER TABLE Core.CredentialPermission WITH CHECK ADD CONSTRAINT [FK_CredentialPermission_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.CredentialPermission CHECK CONSTRAINT [FK_CredentialPermission_UserModifedBy]
GO
ALTER TABLE Core.CredentialPermission WITH CHECK ADD CONSTRAINT [FK_CredentialPermission_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.CredentialPermission CHECK CONSTRAINT [FK_CredentialPermission_UserCreatedBy]
GO

