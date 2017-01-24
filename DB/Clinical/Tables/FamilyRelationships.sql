-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Clinical].[[FamilyRelationShip]]
-- Author:		John Crossen
-- Date:		10/27/2015
--
-- Purpose:		FamilyRelationship table, while similar to collateral, this is for quick relationship entry for clinical screens
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/19/2015	John Crossen	TFS# 3362 - Initial creation.
-- 11/23/2015	Scott Martin	Changed casing on FamilyRelationship. Renamed RelationID to RelationshipTypeID
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Clinical].[FamilyRelationship](
	[FamilyRelationshipID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[ContactID] [BIGINT] NOT NULL,
	[RelationshipTypeID] [INT] NOT NULL,
	[FirstName] [NVARCHAR](200) NULL,
	[LastName] [NVARCHAR](200) NULL,
	[IsDeceased] [BIT] NOT NULL,
	[IsInvolved] [BIT] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_FamilyRelationShip] PRIMARY KEY CLUSTERED 
(
	[FamilyRelationshipID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Clinical].[FamilyRelationship] ADD  CONSTRAINT [DF_FamilyRelationship_IsDeceased]  DEFAULT ((0)) FOR [IsDeceased]
GO

ALTER TABLE [Clinical].[FamilyRelationship] ADD  CONSTRAINT [DF_FamilyRelationship_IsInvolved]  DEFAULT ((0)) FOR [IsInvolved]
GO

ALTER TABLE [Clinical].[FamilyRelationship]  WITH CHECK ADD  CONSTRAINT [FK_FamilyRelationship_Contact] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [Clinical].[FamilyRelationship] CHECK CONSTRAINT [FK_FamilyRelationship_Contact]
GO

ALTER TABLE [Clinical].[FamilyRelationship]  WITH CHECK ADD  CONSTRAINT [FK_FamilyRelationship_RelationshipType] FOREIGN KEY([RelationshipTypeID])
REFERENCES [Reference].[RelationshipType] ([RelationshipTypeID])
GO

ALTER TABLE [Clinical].[FamilyRelationship] CHECK CONSTRAINT [FK_FamilyRelationship_RelationshipType]
GO

ALTER TABLE Clinical.FamilyRelationship WITH CHECK ADD CONSTRAINT [FK_FamilyRelationship_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.FamilyRelationship CHECK CONSTRAINT [FK_FamilyRelationship_UserModifedBy]
GO
ALTER TABLE Clinical.FamilyRelationship WITH CHECK ADD CONSTRAINT [FK_FamilyRelationship_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.FamilyRelationship CHECK CONSTRAINT [FK_FamilyRelationship_UserCreatedBy]
GO

