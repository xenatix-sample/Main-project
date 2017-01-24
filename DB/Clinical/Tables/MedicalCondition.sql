-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Clinical].[MedicalCondition]
-- Author:		John Crossen
-- Date:		10/27/2015
--
-- Purpose:		MedicalHistoryCondition Lookuptable
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/19/2015	John Crossen	TFS# 3664 - Initial creation.
-- 12/2/2015	Scott Martin	Renamed table to MedicalCondition. Changed columns to match new table name
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Clinical].[MedicalCondition](
	[MedicalConditionID] [INT] IDENTITY(1,1) NOT NULL,
	[MedicalCondition] [NVARCHAR](255) NOT NULL,
	[IsSystem] [BIT] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_MedicalCondition] PRIMARY KEY CLUSTERED 
(
	[MedicalConditionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Clinical].[MedicalCondition] ADD  CONSTRAINT [DF_MedicalCondition_IsSystem]  DEFAULT ((0)) FOR [IsSystem]
GO

CREATE NONCLUSTERED INDEX [IX__MedicalCondition__MedicalConditionID] ON [Clinical].[MedicalCondition]
(
	[MedicalCondition] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE Clinical.MedicalCondition WITH CHECK ADD CONSTRAINT [FK_MedicalCondition_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.MedicalCondition CHECK CONSTRAINT [FK_MedicalCondition_UserModifedBy]
GO
ALTER TABLE Clinical.MedicalCondition WITH CHECK ADD CONSTRAINT [FK_MedicalCondition_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.MedicalCondition CHECK CONSTRAINT [FK_MedicalCondition_UserCreatedBy]
GO
