-----------------------------------------------------------------------------------------------------------------------
-- Table:	[Reference].[RelationshipType]
-- Author:		John Crossen
-- Date:		08/19/2015
--
-- Purpose:		Reference Table for RelationshipTypes.
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/19/2015	John Crossen		TFS# 1298 - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[RelationshipType](
	[RelationshipTypeID] [INT] IDENTITY(1,1) NOT NULL,
	[RelationshipType] [NVARCHAR](50) NOT NULL,
	[IsActive] [bit] NOT NULL DEFAULT(1),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL DEFAULT (GETUTCDATE()),
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[SystemCreatedOn] [datetime] NOT NULL,
	[SystemModifiedOn] [datetime] NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_RelationshipType] PRIMARY KEY CLUSTERED 
(
	[RelationshipTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[RelationshipType]  WITH CHECK ADD  CONSTRAINT [FK_RelationshipType_Users] FOREIGN KEY([ModifiedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Reference].[RelationshipType] CHECK CONSTRAINT [FK_RelationshipType_Users]
GO



CREATE NONCLUSTERED INDEX [NonClusteredIndex-20150819-080624] ON [Reference].[RelationshipType]
(
	[RelationshipType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

