-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Clinical].[MedicalHistoryConditionDetail]
-- Author:		Scott Martin
-- Date:		12/2/2015
--
-- Purpose:		Medical History Condition Detail Table
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/2/2015	Scott Martin	Initial creation
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Clinical].[MedicalHistoryConditionDetail](
	[MedicalHistoryConditionDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[MedicalHistoryConditionID] [bigint] NOT NULL,
	[FamilyRelationshipID] [bigint] NULL,
	[IsSelf] [bit] NOT NULL,
	[Comments] [nvarchar](2000) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_MedicalHistoryConditionDetail] PRIMARY KEY CLUSTERED 
(
	[MedicalHistoryConditionDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Clinical].[MedicalHistoryConditionDetail] ADD  CONSTRAINT [DF_MedicalHistoryConditionDetail_IsSelf]  DEFAULT ((1)) FOR [IsSelf]
GO

ALTER TABLE [Clinical].[MedicalHistoryConditionDetail]  WITH CHECK ADD  CONSTRAINT [FK_MedicalHistoryConditionDetail_MedicalHistoryCondition] FOREIGN KEY([MedicalHistoryConditionID])
REFERENCES [Clinical].[MedicalHistoryCondition] ([MedicalHistoryConditionID])
GO

ALTER TABLE [Clinical].[MedicalHistoryConditionDetail] CHECK CONSTRAINT [FK_MedicalHistoryConditionDetail_MedicalHistoryCondition]
GO

ALTER TABLE [Clinical].[MedicalHistoryConditionDetail]  WITH CHECK ADD  CONSTRAINT [FK_MedicalHistoryConditionDetail_FamilyRelationship] FOREIGN KEY([FamilyRelationshipID])
REFERENCES [Clinical].[FamilyRelationship] ([FamilyRelationshipID])
GO

ALTER TABLE [Clinical].[MedicalHistoryConditionDetail] CHECK CONSTRAINT [FK_MedicalHistoryConditionDetail_FamilyRelationship]
GO

ALTER TABLE Clinical.MedicalHistoryConditionDetail WITH CHECK ADD CONSTRAINT [FK_MedicalHistoryConditionDetail_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.MedicalHistoryConditionDetail CHECK CONSTRAINT [FK_MedicalHistoryConditionDetail_UserModifedBy]
GO
ALTER TABLE Clinical.MedicalHistoryConditionDetail WITH CHECK ADD CONSTRAINT [FK_MedicalHistoryConditionDetail_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.MedicalHistoryConditionDetail CHECK CONSTRAINT [FK_MedicalHistoryConditionDetail_UserCreatedBy]
GO
