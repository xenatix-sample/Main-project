-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[Manifest]
-- Author:		Rajiv Ranjan
-- Date:		07/30/2015
--
-- Purpose:		Common for activity details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/30/2015	Rajiv Ranjan	TFS# - Initial creation.
-- 07/30/2015	Suresh Pandey	TFS# - Add IsActive column, modified by no null,
-- 07/30/2015   Sumana Sangapu	1016 - Changed schema from Core to Registration/Core/Reference
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
--To Do:
-- 09/27/2016 Rahul Vats	Why is there a version field in Manifest and ManifestVersion Table separately? 
--							Why is the version in the ManifestVersion Table int? What is the approach?
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[Manifest](
	[ManifestID] [int] IDENTITY(1,1) NOT NULL,
	[FilePath] [nvarchar](250) NOT NULL,
	[Version] [nvarchar](10) NOT NULL,
	[SecurityRoleID] [int] NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_Manifest_ManifestID] PRIMARY KEY CLUSTERED 
(
	[ManifestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Core.Manifest WITH CHECK ADD CONSTRAINT [FK_Manifest_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Manifest CHECK CONSTRAINT [FK_Manifest_UserModifedBy]
GO
ALTER TABLE Core.Manifest WITH CHECK ADD CONSTRAINT [FK_Manifest_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Manifest CHECK CONSTRAINT [FK_Manifest_UserCreatedBy]
GO
