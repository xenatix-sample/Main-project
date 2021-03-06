-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[AssessmentQuestions]
-- Author:		Sumana Sangapu
-- Date:		08/15/2015
--
-- Purpose:		Holds the Questions of assessments  
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/15/2015   Sumana Sangapu  Task# 1601 - Assessment DB Design
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-- 03/23/2016	Kyle Campbell	TFS #8273 Added unique constraint on Datakey field, which now needs to be unique for new assessment logic mapping table
--								Changed Datakey field from varchar to BIGINT since we will be joining on these fields
-- 04/21/2016	Kyle Campbell	TFS# 10179	Added ClassName, Attributes, ValidationMessage columns to table
-- 04/25/2016	Kyle Campbell	Increase size for ClassName column
-- 06/29/2016	Atul Chauhan	Changed column "IsAnswerRequired" datatype bit to int
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[AssessmentQuestions](
	[QuestionID] [bigint] IDENTITY(1,1) NOT NULL,
	[AssessmentID] [bigint] NULL,
	[AssessmentSectionID] [bigint] NULL,
	[ParentQuestionID] [bigint] NOT NULL,
	[ParentOptionsID] [bigint] NULL,
	[Question] [nvarchar](2000) NULL,
	[QuestionTypeID] [int] NULL,
	[OptionsGroupID] [bigint] NULL,
	[IsAnswerRequired] INT NOT NULL,
	[InputTypeID] [int] NULL,
	[InputTypePositionID] [int] NULL,
	[IsNumberingRequired] [bit] NULL,
	[IsCalculated] [bit] NULL,
	[ImageID] [bigint] NULL,
	[SortOrder] [bigint] NULL,
	[DataKey] BIGINT NOT NULL,
	[ContainerAttributes] [NVARCHAR](MAX) NULL,
	[Attributes] [NVARCHAR](MAX) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_AssessmentQuestions] PRIMARY KEY CLUSTERED 
(
	[QuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Core].[AssessmentQuestions]  WITH CHECK ADD  CONSTRAINT [FK_AssessmentQuestions_Assessments] FOREIGN KEY([AssessmentID])
REFERENCES [Core].[Assessments] ([AssessmentID])
GO

ALTER TABLE [Core].[AssessmentQuestions] CHECK CONSTRAINT [FK_AssessmentQuestions_Assessments]
GO

ALTER TABLE [Core].[AssessmentQuestions]  WITH CHECK ADD  CONSTRAINT [FK_AssessmentQuestions_Images] FOREIGN KEY([ImageID])
REFERENCES [Core].[Images] ([ImageID])
GO

ALTER TABLE [Core].[AssessmentQuestions] CHECK CONSTRAINT [FK_AssessmentQuestions_Images]
GO

ALTER TABLE [Core].[AssessmentQuestions]  WITH CHECK ADD  CONSTRAINT [FK_AssessmentQuestions_InputType] FOREIGN KEY([InputTypeID])
REFERENCES [Reference].[AssessmentInputType] ([InputTypeID])
GO

ALTER TABLE [Core].[AssessmentQuestions] CHECK CONSTRAINT [FK_AssessmentQuestions_InputType]
GO

ALTER TABLE [Core].[AssessmentQuestions]  WITH CHECK ADD  CONSTRAINT [FK_AssessmentQuestions_OptionsGroup] FOREIGN KEY([OptionsGroupID])
REFERENCES [Core].[AssessmentOptionsGroup] ([OptionsGroupID])
GO

ALTER TABLE [Core].[AssessmentQuestions] CHECK CONSTRAINT [FK_AssessmentQuestions_OptionsGroup]
GO

ALTER TABLE [Core].[AssessmentQuestions]  WITH CHECK ADD  CONSTRAINT [FK_AssessmentQuestions_QuestionType] FOREIGN KEY([QuestionTypeID])
REFERENCES [Reference].[AssessmentQuestionType] ([QuestionTypeID])
GO

ALTER TABLE [Core].[AssessmentQuestions] CHECK CONSTRAINT [FK_AssessmentQuestions_QuestionType]
GO

ALTER TABLE [Core].[AssessmentQuestions]  WITH CHECK ADD  CONSTRAINT [FK_AssessmentQuestions_InputTypePositionID] FOREIGN KEY([InputTypePositionID])
REFERENCES [Reference].[AssessmentInputTypePosition] ([InputTypePositionID])
GO

ALTER TABLE [Core].[AssessmentQuestions] CHECK CONSTRAINT [FK_AssessmentQuestions_InputTypePositionID]
GO

ALTER TABLE [Core].[AssessmentQuestions]  WITH CHECK ADD  CONSTRAINT [FK_AssessmentQuestions_AssessmentSectionID] FOREIGN KEY([AssessmentSectionID])
REFERENCES [Core].[AssessmentSections] ([AssessmentSectionID])
GO

ALTER TABLE [Core].[AssessmentQuestions] CHECK CONSTRAINT [FK_AssessmentQuestions_AssessmentSectionID]
GO
	
ALTER TABLE [Core].[AssessmentQuestions]  WITH CHECK ADD  CONSTRAINT [FK_AssessmentQuestions_ParentOptionsID] FOREIGN KEY([ParentOptionsID])
REFERENCES [Core].[AssessmentOptions] ([OptionsID])
GO

ALTER TABLE [Core].[AssessmentOptions] CHECK CONSTRAINT [FK_AssessmentQuestions_ParentOptionsID]
GO

ALTER TABLE Core.AssessmentQuestions WITH CHECK ADD CONSTRAINT [FK_AssessmentQuestions_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.AssessmentQuestions CHECK CONSTRAINT [FK_AssessmentQuestions_UserModifedBy]
GO
ALTER TABLE Core.AssessmentQuestions WITH CHECK ADD CONSTRAINT [FK_AssessmentQuestions_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.AssessmentQuestions CHECK CONSTRAINT [FK_AssessmentQuestions_UserCreatedBy]
GO

ALTER TABLE [Core].[AssessmentQuestions] ADD CONSTRAINT IX_QuestionDataKey UNIQUE (DataKey)
GO