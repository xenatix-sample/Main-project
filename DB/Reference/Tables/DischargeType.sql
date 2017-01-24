CREATE TABLE Reference.DischargeType (
	DischargeTypeID			INT IDENTITY(1,1) NOT NULL,
	DischargeType			NVARCHAR(50) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_DischargeType_DischargeTypeID] PRIMARY KEY CLUSTERED 
(
	[DischargeTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Reference.DischargeType WITH CHECK ADD CONSTRAINT [FK_DischargeType_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.DischargeType CHECK CONSTRAINT [FK_DischargeType_UserModifedBy]
GO
ALTER TABLE Reference.DischargeType WITH CHECK ADD CONSTRAINT [FK_DischargeType_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.DischargeType CHECK CONSTRAINT [FK_DischargeType_UserCreatedBy]
GO
