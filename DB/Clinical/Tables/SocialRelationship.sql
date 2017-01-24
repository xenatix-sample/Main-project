-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Clinical].[[SocialRelationShip]]
-- Author:		John Crossen
-- Date:		10/27/2015
--
-- Purpose:		Social Relationship Header for Social Relationship Bundle.
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/19/2015	John Crossen	TFS# 3362 - Initial creation.
-- 11/20/2015	Scott Martin	EncounterID was mispelled and changed TakenTime to TakenOn
-- 11/30/2015	Scott Martin	Changed column TakenOn to TakenTime
-- 12/10/2015	Scott Martin	Moved ChildhoodHistory, RelationshipHistory, and FamilyHistory from SocialRelationshipDetail
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Clinical].[SocialRelationship](
	[SocialRelationshipID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[EncounterID] [BIGINT] NULL,
	[ContactID] [BIGINT] NOT NULL,
	[ReviewedNoChanges] BIT NOT NULL DEFAULT(0),
	[TakenBy] [INT] NOT NULL,
	[TakenTime] [DATETIME] NOT NULL,
	[ChildhoodHistory] [NVARCHAR](1000) NULL,
	[RelationshipHistory] [NVARCHAR](1000) NULL,
	[FamilyHistory] [NVARCHAR](1000) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_SocialRelationship] PRIMARY KEY CLUSTERED 
(
	[SocialRelationshipID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Clinical].[SocialRelationship] ADD  CONSTRAINT [DF_SocialRelationship_TakenTime]  DEFAULT (GETUTCDATE()) FOR [TakenTime]
GO

ALTER TABLE [Clinical].[SocialRelationship]  WITH CHECK ADD  CONSTRAINT [FK_SocialRelationship_Contact] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [Clinical].[SocialRelationship] CHECK CONSTRAINT [FK_SocialRelationship_Contact]
GO

ALTER TABLE Clinical.SocialRelationship WITH CHECK ADD CONSTRAINT [FK_SocialRelationship_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.SocialRelationship CHECK CONSTRAINT [FK_SocialRelationship_UserModifedBy]
GO
ALTER TABLE Clinical.SocialRelationship WITH CHECK ADD CONSTRAINT [FK_SocialRelationship_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.SocialRelationship CHECK CONSTRAINT [FK_SocialRelationship_UserCreatedBy]
GO



