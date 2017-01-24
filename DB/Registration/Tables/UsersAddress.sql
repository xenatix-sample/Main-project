-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
CREATE TABLE [Registration].[UsersAddress] (
    [ContactID]   INT      NOT NULL,
    [AddressID]   INT      NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_UsersAddress] PRIMARY KEY CLUSTERED ([ContactID] ASC, [AddressID] ASC)
);

GO

ALTER TABLE Registration.UsersAddress WITH CHECK ADD CONSTRAINT [FK_UsersAddress_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.UsersAddress CHECK CONSTRAINT [FK_UsersAddress_UserModifedBy]
GO
ALTER TABLE Registration.UsersAddress WITH CHECK ADD CONSTRAINT [FK_UsersAddress_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.UsersAddress CHECK CONSTRAINT [FK_UsersAddress_UserCreatedBy]
GO
