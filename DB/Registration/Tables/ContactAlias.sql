CREATE TABLE [Registration].[ContactAlias]
(
	[ContactAliasID] BIGINT NOT NULL IDENTITY(1,1),
	[ContactID] BIGINT NOT NULL,	
	[AliasFirstName] NVARCHAR(200) NULL,
	[AliasMiddle] NVARCHAR(200) NULL,
	[AliasLastName] NVARCHAR(200) NULL,
	[SuffixID] INT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [PK_ContactAlias_ContactAliasID] PRIMARY KEY CLUSTERED ([ContactAliasID] ASC),
	CONSTRAINT [FK_ContactAlias_ContactID] FOREIGN KEY (ContactID) REFERENCES [Registration].[Contact] (ContactID),
	CONSTRAINT [FK_ContactAlias_SuffixID] FOREIGN KEY (SuffixID) REFERENCES [Reference].[Suffix] (SuffixID),
	CONSTRAINT [FK_ContactAlias_ModifiedBy] FOREIGN KEY (ModifiedBy) REFERENCES Core.Users (UserID),
	CONSTRAINT [FK_ContactAlias_CreatedBy] FOREIGN KEY (CreatedBy) REFERENCES Core.Users (UserID)
);
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Alias First Name', 
@level0type = N'SCHEMA', @level0name = Registration, 
@level1type = N'TABLE',  @level1name = ContactAlias,
@level2type = N'COLUMN', @level2name = AliasFirstName;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Alias Middle Name', 
@level0type = N'SCHEMA', @level0name = Registration, 
@level1type = N'TABLE',  @level1name = ContactAlias,
@level2type = N'COLUMN', @level2name = AliasMiddle;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Alias Last Name', 
@level0type = N'SCHEMA', @level0name = Registration, 
@level1type = N'TABLE',  @level1name = ContactAlias,
@level2type = N'COLUMN', @level2name = AliasLastName;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'References the ID in the Reference.Suffix table', 
@level0type = N'SCHEMA', @level0name = Registration, 
@level1type = N'TABLE',  @level1name = ContactAlias,
@level2type = N'COLUMN', @level2name = SuffixID;
GO