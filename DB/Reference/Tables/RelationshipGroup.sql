-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[RelationshipGroup]
-- Author:		Scott Martin
-- Date:		12/1/2015
--
-- Purpose:		Header table for separating out groups of relationships
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/1/2015	Scott Martin	Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[RelationshipGroup](
	[RelationshipGroupID] [bigint] IDENTITY(1,1) NOT NULL,
	[RelationshipGroup] [nvarchar](255) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_RelationshipGroup] PRIMARY KEY CLUSTERED 
(
	[RelationshipGroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Reference.RelationshipGroup WITH CHECK ADD CONSTRAINT [FK_RelationshipGroup_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.RelationshipGroup CHECK CONSTRAINT [FK_RelationshipGroup_UserModifedBy]
GO
ALTER TABLE Reference.RelationshipGroup WITH CHECK ADD CONSTRAINT [FK_RelationshipGroup_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.RelationshipGroup CHECK CONSTRAINT [FK_RelationshipGroup_UserCreatedBy]
GO

