-----------------------------------------------------------------------------------------------------------------------
-- Table:		Registration.[ContactClientIdentifier]
-- Author:		Scott Martin
-- Date:		12/22/2015
--
-- Purpose:		Store Contact Alternate ID data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/22/2015	Scott Martin		Initial Creation
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 03/09/2016	Kyle Campbell	TFS #6339 Added EffectiveDate and ExpirationDate fields
-- 08/26/2016	Scott Martin	Added index
-- 09/22/2016	Gurpreet Singh	Allow Null for ClientIdentifierTypeID
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[ContactClientIdentifier] (
	[ContactClientIdentifierID]	BIGINT IDENTITY(1,1) NOT NULL,
    [ContactID]   BIGINT   NOT NULL,
    [ClientIdentifierTypeID]     INT NULL,
	[AlternateID] NVARCHAR(50)  NULL,
	[ExpirationReasonID] INT NULL,
	[EffectiveDate] DATE NULL,
	[ExpirationDate] DATE NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_ContactClientIdentifier_ContactClientIdentifierID]			PRIMARY KEY CLUSTERED ([ContactClientIdentifierID] ASC),
    CONSTRAINT [FK_ContactClientIdentifier_ContactID]			FOREIGN KEY ([ContactID]) REFERENCES [Registration].[Contact] ([ContactID]),
    CONSTRAINT [FK_ContactClientIdentifier_ClientIdentifierTypeID]			FOREIGN KEY ([ClientIdentifierTypeID]) REFERENCES [Reference].[ClientIdentifierType] ([ClientIdentifierTypeID])
);

GO

CREATE NONCLUSTERED INDEX [IX_ContactClientIdentifier_ContactID] ON [Registration].[ContactClientIdentifier]
(
	ContactID ASC 
)
INCLUDE (SystemCreatedOn, SystemModifiedOn) 
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE Registration.ContactClientIdentifier WITH CHECK ADD CONSTRAINT [FK_ContactClientIdentifier_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ContactClientIdentifier CHECK CONSTRAINT [FK_ContactClientIdentifier_UserModifedBy]
GO
ALTER TABLE Registration.ContactClientIdentifier WITH CHECK ADD CONSTRAINT [FK_ContactClientIdentifier_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ContactClientIdentifier CHECK CONSTRAINT [FK_ContactClientIdentifier_UserCreatedBy]
GO
ALTER TABLE Registration.ContactClientIdentifier WITH CHECK ADD CONSTRAINT [FK_ContactClientIdentifier_ExpirationReasonID] FOREIGN KEY ([ExpirationReasonID]) REFERENCES [Reference].[OtherIDExpirationReasons] ([ExpirationReasonID])
GO
ALTER TABLE Registration.ContactClientIdentifier CHECK CONSTRAINT [FK_ContactClientIdentifier_ExpirationReasonID]
GO
