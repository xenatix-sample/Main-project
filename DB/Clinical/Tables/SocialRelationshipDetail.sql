
-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Clinical].[[SocialRelationShipDetail]]
-- Author:		John Crossen
-- Date:		10/27/2015
--
-- Purpose:		Social Relationship Detail for Social Relationship Bundle.
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/19/2015	John Crossen	TFS# 3362 - Initial creation.
-- 12/10/2015	Scott Martin	Moved ChildhoodHistory, RelationshipHistory, and FamilyHistory to SocialRelationship
--								and added FamilyRelationshipID
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Clinical].[SocialRelationshipDetail](
	[SocialRelationshipDetailID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[SocialRelationshipID] [BIGINT] NOT NULL,
	[FamilyRelationshipID] [bigint] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_SocialRelationshipDetail] PRIMARY KEY CLUSTERED 
(
	[SocialRelationshipDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Clinical].[SocialRelationshipDetail]  WITH CHECK ADD  CONSTRAINT [FK_SocialRelationshipDetail_SocialRelationship] FOREIGN KEY([SocialRelationshipID])
REFERENCES [Clinical].[SocialRelationship] ([SocialRelationshipID])
GO

ALTER TABLE [Clinical].[SocialRelationshipDetail] CHECK CONSTRAINT [FK_SocialRelationshipDetail_SocialRelationship]
GO

ALTER TABLE [Clinical].[SocialRelationshipDetail]  WITH CHECK ADD  CONSTRAINT [FK_SocialRelationshipDetail_FamilyRelationship] FOREIGN KEY([FamilyRelationshipID])
REFERENCES [Clinical].[FamilyRelationship] ([FamilyRelationshipID])
GO

ALTER TABLE [Clinical].[SocialRelationshipDetail] CHECK CONSTRAINT [FK_SocialRelationshipDetail_FamilyRelationship]
GO

ALTER TABLE Clinical.SocialRelationshipDetail WITH CHECK ADD CONSTRAINT [FK_SocialRelationshipDetail_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.SocialRelationshipDetail CHECK CONSTRAINT [FK_SocialRelationshipDetail_UserModifedBy]
GO
ALTER TABLE Clinical.SocialRelationshipDetail WITH CHECK ADD CONSTRAINT [FK_SocialRelationshipDetail_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.SocialRelationshipDetail CHECK CONSTRAINT [FK_SocialRelationshipDetail_UserCreatedBy]
GO

