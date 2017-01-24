CREATE TABLE [Core].[AssessmentLogic]
(
	[LogicID] INT IDENTITY(1,1) NOT NULL,
	[LogicTypeID] INT NULL,
	[LogicCode] NVARCHAR(MAX) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [PK_AssessmentLogicID] PRIMARY KEY CLUSTERED 
	(
		[LogicID] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
) ON [PRIMARY]
GO

ALTER TABLE Core.AssessmentLogic WITH CHECK ADD CONSTRAINT [FK_AssessmentLogic_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.AssessmentLogic CHECK CONSTRAINT [FK_AssessmentLogic_UserModifedBy]
GO
ALTER TABLE Core.AssessmentLogic WITH CHECK ADD CONSTRAINT [FK_AssessmentLogic_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.AssessmentLogic CHECK CONSTRAINT [FK_AssessmentLogic_UserCreatedBy]
GO

ALTER TABLE Core.AssessmentLogic WITH CHECK ADD CONSTRAINT [FK_AssessmentLogic_LogicTypeID] FOREIGN KEY ([LogicTypeID]) REFERENCES Reference.LogicType ([LogicTypeID])
GO
ALTER TABLE Core.AssessmentLogic CHECK CONSTRAINT [FK_AssessmentLogic_LogicTypeID]
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'References LogicTypeID in Reference.LogicType table', 
@level0type = N'SCHEMA', @level0name = Core, 
@level1type = N'TABLE',  @level1name = AssessmentLogic,
@level2type = N'COLUMN', @level2name = LogicTypeID;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Stores the functions/code to perform necessary operations on certain Assessment Questions and Options', 
@level0type = N'SCHEMA', @level0name = Core, 
@level1type = N'TABLE',  @level1name = AssessmentLogic,
@level2type = N'COLUMN', @level2name = LogicCode;
GO