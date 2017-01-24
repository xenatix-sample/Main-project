-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[ManifestVersion]
-- Author:		Justin Spalti
-- Date:		08/06/2015
--
-- Purpose:		Lookup for the latest versions of the manifest file used for the app cache
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/03/2015   Justin Spalti  TFS# 875 - Initial creation for use with the global configuration settings
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns

--To Do:
-- 09/27/2016 Rahul Vats	Why is there a version field in Manifest and ManifestVersion Table separately? 
--							Why is the version in the ManifestVersion Table int? What is the approach?
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[ManifestVersion](
	[ManifestVersionID] [int] IDENTITY(1,1) NOT NULL,
	[Version] [int] NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_ManifestVersion_IsActive]  DEFAULT ((1)),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ManifestVersion] PRIMARY KEY CLUSTERED 
(
	[ManifestVersionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Core.ManifestVersion WITH CHECK ADD CONSTRAINT [FK_ManifestVersion_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ManifestVersion CHECK CONSTRAINT [FK_ManifestVersion_UserModifedBy]
GO
ALTER TABLE Core.ManifestVersion WITH CHECK ADD CONSTRAINT [FK_ManifestVersion_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ManifestVersion CHECK CONSTRAINT [FK_ManifestVersion_UserCreatedBy]
GO