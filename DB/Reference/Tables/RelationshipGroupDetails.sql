-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[RelationshipGroupDetails]
-- Author:		Scott Martin
-- Date:		12/1/2015
--
-- Purpose:		Mapping table for associating a relationship type to a relationship group
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/1/2015	Scott Martin	Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[RelationshipGroupDetails](
	[RelationshipGroupDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[RelationshipGroupID] [bigint] NOT NULL,
	[RelationshipTypeID] [int] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_RelationshipGroupDetails] PRIMARY KEY CLUSTERED 
(
	[RelationshipGroupDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[RelationshipGroupDetails]  WITH CHECK ADD  CONSTRAINT [FK_RelationshipGroupDetails_RelationshipGroup] FOREIGN KEY([RelationshipGroupID])
REFERENCES [Reference].[RelationshipGroup] ([RelationshipGroupID])
GO

ALTER TABLE [Reference].[RelationshipGroupDetails] CHECK CONSTRAINT [FK_RelationshipGroupDetails_RelationshipGroup]
GO

ALTER TABLE [Reference].[RelationshipGroupDetails]  WITH CHECK ADD  CONSTRAINT [FK_RelationshipGroupDetails_RelationshipType] FOREIGN KEY([RelationshipTypeID])
REFERENCES [Reference].[RelationshipType] ([RelationshipTypeID])
GO

ALTER TABLE [Reference].[RelationshipGroupDetails] CHECK CONSTRAINT [FK_RelationshipGroupDetails_RelationshipType]
GO

ALTER TABLE Reference.RelationshipGroupDetails WITH CHECK ADD CONSTRAINT [FK_RelationshipGroupDetails_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.RelationshipGroupDetails CHECK CONSTRAINT [FK_RelationshipGroupDetails_UserModifedBy]
GO
ALTER TABLE Reference.RelationshipGroupDetails WITH CHECK ADD CONSTRAINT [FK_RelationshipGroupDetails_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.RelationshipGroupDetails CHECK CONSTRAINT [FK_RelationshipGroupDetails_UserCreatedBy]
GO

