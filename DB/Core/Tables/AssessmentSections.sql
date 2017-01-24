-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[AssessmentSections]
-- Author:		Sumana Sangapu
-- Date:		10/05/2015
--
-- Purpose:		Holds the Assessments Sections   
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/05/2015   Sumana Sangapu  Task# 1601 - Assessment DB Design
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[AssessmentSections](
	[AssessmentSectionID] [bigint] IDENTITY(1,1) NOT NULL,
	[SectionName] [nvarchar](100) NULL,
	[AssessmentID] [bigint] NULL,
	[AssessmentTemplateID] INT NULL,
	[SortOrder] [bigint] NULL,
	[PermissionKey] [nvarchar](100) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [PK_AssessmentSections_AssessmentSectionID] PRIMARY KEY CLUSTERED 
(
	[AssessmentSectionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE Core.AssessmentSections WITH CHECK ADD CONSTRAINT [FK_AssessmentSections_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.AssessmentSections CHECK CONSTRAINT [FK_AssessmentSections_UserModifedBy]
GO
ALTER TABLE Core.AssessmentSections WITH CHECK ADD CONSTRAINT [FK_AssessmentSections_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.AssessmentSections CHECK CONSTRAINT [FK_AssessmentSections_UserCreatedBy]
GO

ALTER TABLE Core.AssessmentSections WITH CHECK ADD CONSTRAINT [FK_AssessmentSections_AssessmentTemplateID] FOREIGN KEY ([AssessmentTemplateID]) REFERENCES [Reference].[AssessmentTemplate] ([AssessmentTemplateID])
GO
ALTER TABLE Core.AssessmentSections CHECK CONSTRAINT [FK_AssessmentSections_AssessmentTemplateID]
GO