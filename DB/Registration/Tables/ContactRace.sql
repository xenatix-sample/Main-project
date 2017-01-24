-----------------------------------------------------------------------------------------------------------------------
-- Table:		Registration.[ContactRace]
-- Author:		Kyle Campbell
-- Date:		03/07/2016
--
-- Purpose:		Store Contact and Race relationship data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		Contact Table, Race Table (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/07/2016	Kyle Campbell	Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Registration].[ContactRace]
(
	[ContactRaceID] BIGINT NOT NULL IDENTITY(1,1), 
    [ContactID] BIGINT NOT NULL, 
    [RaceID] INT NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT (1), 
    [ModifiedBy] INT NOT NULL, 
    [ModifiedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()), 
    [CreatedBy] INT NOT NULL DEFAULT (1), 
    [CreatedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()), 
    [SystemCreatedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()), 
    [SystemModifiedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()),	
    CONSTRAINT [PK_ContactRace_ContactRaceID]	PRIMARY KEY CLUSTERED (ContactRaceID ASC),
	CONSTRAINT [FK_ContactRace_ContactID]	FOREIGN KEY ([ContactID]) REFERENCES [Registration].[Contact] ([ContactID]),
	CONSTRAINT [FK_ContactRace_RaceID]	FOREIGN KEY ([RaceID]) REFERENCES [Reference].[Race] ([RaceID]),    	
	CONSTRAINT [FK_ContactRace_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID]),
	CONSTRAINT [FK_ContactRace_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
)
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Stores the unique identifiers for this table', 
@level0type = N'SCHEMA', @level0name = Registration, 
@level1type = N'TABLE',  @level1name = ContactRace,
@level2type = N'COLUMN', @level2name = ContactRaceID;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'References ContactID from Registration.Contact table', 
@level0type = N'SCHEMA', @level0name = Registration, 
@level1type = N'TABLE',  @level1name = ContactRace,
@level2type = N'COLUMN', @level2name = ContactID;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'References RaceID from Reference.Race table', 
@level0type = N'SCHEMA', @level0name = Registration, 
@level1type = N'TABLE',  @level1name = ContactRace,
@level2type = N'COLUMN', @level2name = RaceID;
GO

CREATE NONCLUSTERED INDEX [IX_ContactRace_ContactID]
ON [Registration].[ContactRace] ([ContactID])
