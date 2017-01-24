CREATE TABLE [Core].[AssessmentQuestionLogicMapping]
(
	[LogicMappingID] INT IDENTITY(1,1) NOT NULL,
	[LogicID] INT NOT NULL,
	[QuestionDataKey] BIGINT NOT NULL,
	[LogicLocationID] INT NOT NULL,
	[LogicOrder] INT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [PK_AssessmentQuestionLogicMappingID] PRIMARY KEY CLUSTERED 
	(
		[LogicMappingID] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
) ON [PRIMARY]
GO

ALTER TABLE Core.AssessmentQuestionLogicMapping WITH CHECK ADD CONSTRAINT [FK_AssessmentQuestionLogicMapping_LogicID] FOREIGN KEY ([LogicID]) REFERENCES Core.AssessmentLogic ([LogicID])
GO
ALTER TABLE Core.AssessmentQuestionLogicMapping CHECK CONSTRAINT [FK_AssessmentQuestionLogicMapping_LogicID]
GO

ALTER TABLE Core.AssessmentQuestionLogicMapping WITH CHECK ADD CONSTRAINT [FK_AssessmentQuestionLogicMapping_QuestionDataKey] FOREIGN KEY ([QuestionDataKey]) REFERENCES Core.AssessmentQuestions ([DataKey])
GO
ALTER TABLE Core.AssessmentQuestionLogicMapping CHECK CONSTRAINT [FK_AssessmentQuestionLogicMapping_QuestionDataKey]
GO

ALTER TABLE Core.AssessmentQuestionLogicMapping WITH CHECK ADD CONSTRAINT [FK_AssessmentQuestionLogicMapping_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.AssessmentQuestionLogicMapping CHECK CONSTRAINT [FK_AssessmentQuestionLogicMapping_UserModifedBy]
GO
ALTER TABLE Core.AssessmentQuestionLogicMapping WITH CHECK ADD CONSTRAINT [FK_AssessmentQuestionLogicMapping_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.AssessmentQuestionLogicMapping CHECK CONSTRAINT [FK_AssessmentQuestionLogicMapping_UserCreatedBy]
GO

ALTER TABLE Core.AssessmentQuestionLogicMapping WITH CHECK ADD CONSTRAINT [FK_AssessmentQuestionLogicMapping_LogicLocationID] FOREIGN KEY([LogicLocationID]) REFERENCES [Reference].[AssessmentLogicLocation] ([LogicLocationID])
GO
ALTER TABLE Core.AssessmentQuestionLogicMapping CHECK CONSTRAINT [FK_AssessmentQuestionLogicMapping_LogicLocationID]
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'References LogicID in Core.AssessmentLogic table', 
@level0type = N'SCHEMA', @level0name = Core, 
@level1type = N'TABLE',  @level1name = AssessmentQuestionLogicMapping,
@level2type = N'COLUMN', @level2name = LogicID;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'References DataKey in Core.AssessmentQuestions table', 
@level0type = N'SCHEMA', @level0name = Core, 
@level1type = N'TABLE',  @level1name = AssessmentQuestionLogicMapping,
@level2type = N'COLUMN', @level2name = QuestionDataKey;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Specifies in which order to process logic operations if multiple operations are required for one question', 
@level0type = N'SCHEMA', @level0name = Core, 
@level1type = N'TABLE',  @level1name = AssessmentQuestionLogicMapping,
@level2type = N'COLUMN', @level2name = LogicOrder;
GO