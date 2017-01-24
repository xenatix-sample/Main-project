-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
CREATE TABLE [Core].[SecurityRole] (
    [SecurityRoleID] INT            NOT NULL,
    [Name]           NVARCHAR (150) NOT NULL,
    [Description]    NVARCHAR (250) NULL,
    [Active]         BIT            NOT NULL,
    [EffectiveDate]  DATETIME       NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_SecurityRoleID] PRIMARY KEY CLUSTERED 
	(
	[SecurityRoleID] ASC
	)
WITH (
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE Core.SecurityRole WITH CHECK ADD CONSTRAINT [FK_SecurityRole_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.SecurityRole CHECK CONSTRAINT [FK_SecurityRole_UserModifedBy]
GO
ALTER TABLE Core.SecurityRole WITH CHECK ADD CONSTRAINT [FK_SecurityRole_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.SecurityRole CHECK CONSTRAINT [FK_SecurityRole_UserCreatedBy]
GO
