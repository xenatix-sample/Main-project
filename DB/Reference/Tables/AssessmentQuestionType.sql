-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[AssessmentQuestionType]
-- Author:		Sumana Sangapu
-- Date:		08/15/2015
--
-- Purpose:		Holds the Question type lookup details of assessments  
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/15/2015   Sumana Sangapu  Task# 1601 - Assessment DB Design
-- 09/16/2015	Sumana Sangapu	Task# 1601 - Added Styling Columns 
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[AssessmentQuestionType](
	[QuestionTypeID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionType] [nvarchar](50) NULL,
	[StyleClassName] [nvarchar] (255) NULL,
	[SortOrder] [int] NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_QuestionType] PRIMARY KEY CLUSTERED 
(
	[QuestionTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Reference.AssessmentQuestionType WITH CHECK ADD CONSTRAINT [FK_AssessmentQuestionType_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.AssessmentQuestionType CHECK CONSTRAINT [FK_AssessmentQuestionType_UserModifedBy]
GO
ALTER TABLE Reference.AssessmentQuestionType WITH CHECK ADD CONSTRAINT [FK_AssessmentQuestionType_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.AssessmentQuestionType CHECK CONSTRAINT [FK_AssessmentQuestionType_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Assessment Question Type', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AssessmentQuestionType;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating kind of question', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AssessmentQuestionType;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AssessmentQuestionType;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'0', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AssessmentQuestionType;
GO;