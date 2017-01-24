-----------------------------------------------------------------------------------------------------------------------
-- Table:		Registration.[ContactPresentingProblem]
-- Author:		Scott Martin
-- Date:		03/28/2016
--
-- Purpose:		Store Contact Presenting Problem
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/28/2016	Scott Martin	Initial Creation
-- 03/31/2016	Scott Martin	Removed OrganizationID and replaced it with PresentingProblemTypeID
-- 08/26/2016	Scott Martin	Added index
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[ContactPresentingProblem]
(
	[ContactPresentingProblemID] BIGINT NOT NULL IDENTITY(1,1), 
    [ContactID] BIGINT NOT NULL, 
    [PresentingProblemTypeID] SMALLINT NULL,
	[EffectiveDate] DATE NULL,
	[ExpirationDate] DATE NULL,
    [IsActive] BIT NOT NULL DEFAULT (1), 
    [ModifiedBy] INT NOT NULL, 
    [ModifiedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()), 
    [CreatedBy] INT NOT NULL DEFAULT (1), 
    [CreatedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()), 
    [SystemCreatedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()), 
    [SystemModifiedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()),	
    CONSTRAINT [PK_ContactPresentingProblem_ContactPresentingProblemID]	PRIMARY KEY CLUSTERED (ContactPresentingProblemID ASC),
	CONSTRAINT [FK_ContactPresentingProblem_ContactID]	FOREIGN KEY ([ContactID]) REFERENCES [Registration].[Contact] ([ContactID]),
	CONSTRAINT [FK_ContactPresentingProblem_PresentingProblemTypeID]	FOREIGN KEY ([PresentingProblemTypeID]) REFERENCES [Reference].[PresentingProblemType] ([PresentingProblemTypeID]),
	CONSTRAINT [FK_ContactPresentingProblem_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID]),
	CONSTRAINT [FK_ContactPresentingProblem_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
)
GO

CREATE NONCLUSTERED INDEX [IX_ContactPresentingProblem_ContactID] ON [Registration].[ContactPresentingProblem]
(
	ContactID ASC 
)
INCLUDE (SystemCreatedOn, SystemModifiedOn) 
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Stores the unique identifiers for this table', 
@level0type = N'SCHEMA', @level0name = Registration, 
@level1type = N'TABLE',  @level1name = ContactPresentingProblem,
@level2type = N'COLUMN', @level2name = ContactPresentingProblemID;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'References ContactID from Registration.Contact table', 
@level0type = N'SCHEMA', @level0name = Registration, 
@level1type = N'TABLE',  @level1name = ContactPresentingProblem,
@level2type = N'COLUMN', @level2name = ContactID;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'References PresentingProblemTypeID from Reference.PresentingProblemType table', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = PresentingProblemType,
@level2type = N'COLUMN', @level2name = PresentingProblemTypeID;
GO