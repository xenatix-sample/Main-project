-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Clinical].[MedicalHistoryDetail]
-- Author:		John Crossen
-- Date:		10/27/2015
--
-- Purpose:		MedicalHistory Detail Table
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/19/2015	John Crossen	TFS# 3664 - Initial creation.
-- 11/23/2015	Scott Martin	Corrected ModifiedBy and ModifiedOn types. Changed casing on FamilyRelationship
-- 12/2/2015	Scott Martin	Renamed table to MedicalHistoryCondition. Removed IsSelf, FamilyRelationshipID and Comments columns
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Clinical].[MedicalHistoryCondition](
	[MedicalHistoryConditionID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[MedicalHistoryID] [BIGINT] NOT NULL,
	[MedicalConditionID] [INT] NOT NULL,
    [HasCondition]              BIT      NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_MedicalHistoryCondition] PRIMARY KEY CLUSTERED 
(
	[MedicalHistoryConditionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Clinical].[MedicalHistoryCondition]  WITH CHECK ADD  CONSTRAINT [FK_MedicalHistoryCondition_MedicalHistory] FOREIGN KEY([MedicalHistoryID])
REFERENCES [Clinical].[MedicalHistory] ([MedicalHistoryID])
GO

ALTER TABLE [Clinical].[MedicalHistoryCondition] CHECK CONSTRAINT [FK_MedicalHistoryCondition_MedicalHistory]
GO

ALTER TABLE [Clinical].[MedicalHistoryCondition]  WITH CHECK ADD  CONSTRAINT [FK_MedicalHistoryCondition_MedicalCondition] FOREIGN KEY([MedicalConditionID])
REFERENCES [Clinical].[MedicalCondition] ([MedicalConditionID])
GO

ALTER TABLE [Clinical].[MedicalHistoryCondition] CHECK CONSTRAINT [FK_MedicalHistoryCondition_MedicalCondition]
GO

ALTER TABLE Clinical.MedicalHistoryCondition WITH CHECK ADD CONSTRAINT [FK_MedicalHistoryCondition_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.MedicalHistoryCondition CHECK CONSTRAINT [FK_MedicalHistoryCondition_UserModifedBy]
GO
ALTER TABLE Clinical.MedicalHistoryCondition WITH CHECK ADD CONSTRAINT [FK_MedicalHistoryCondition_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.MedicalHistoryCondition CHECK CONSTRAINT [FK_MedicalHistoryCondition_UserCreatedBy]
GO
