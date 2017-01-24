-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
CREATE TABLE [Core].[UserTeam] (
    [UserID] INT NOT NULL,
    [TeamID] INT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_UserTeam] PRIMARY KEY CLUSTERED ([UserID] ASC, [TeamID] ASC)
);
GO

ALTER TABLE Core.UserTeam WITH CHECK ADD CONSTRAINT [FK_UserTeam_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.UserTeam CHECK CONSTRAINT [FK_UserTeam_UserModifedBy]
GO
ALTER TABLE Core.UserTeam WITH CHECK ADD CONSTRAINT [FK_UserTeam_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.UserTeam CHECK CONSTRAINT [FK_UserTeam_UserCreatedBy]
GO


