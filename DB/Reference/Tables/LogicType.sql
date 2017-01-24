CREATE TABLE [Reference].[LogicType]
(
	[LogicTypeID] INT IDENTITY (1,1) NOT NULL,
	[LogicType] NVARCHAR(50) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
	[SortOrder] INT NULL,
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [PK_LogicType] PRIMARY KEY CLUSTERED 
	(
		[LogicTypeID] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
) ON [PRIMARY]
GO

ALTER TABLE Reference.LogicType WITH CHECK ADD CONSTRAINT [FK_LogicType_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.LogicType CHECK CONSTRAINT [FK_LogicType_UserModifedBy]
GO
ALTER TABLE Reference.LogicType WITH CHECK ADD CONSTRAINT [FK_LogicType_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.LogicType CHECK CONSTRAINT [FK_LogicType_UserCreatedBy]
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Stores the logic types for Assessment Questions/Options', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = LogicType,
@level2type = N'COLUMN', @level2name = LogicType;
GO
