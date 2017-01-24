-----------------------------------------------------------------------------------------------------------------------
-- Table:		Registration.[ContactAddress]
-- Author:		Saurabh Sahu
-- Date:		07/23/2015
--
-- Purpose:		Store Contact and Address relationship data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		Contact Table, Address Table (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Saurabh Sahu		Modification .
-- 7/31/2015    John Crossen    Change schema from dbo to Registration
-- 08/13/2015	Sumana Sangapu		Task #: 1227 - Refactor schema
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-- 03/02/2016	Kyle Campbell	Added EffectiveDate and ExpirationDate fields
-- 08/26/2016	Scott Martin	Added index
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[ContactAddress] (
	[ContactAddressID] BIGINT NOT NULL IDENTITY(1,1),
    [AddressID] BIGINT NOT NULL,
    [ContactID] BIGINT NOT NULL,
    [MailPermissionID] INT NULL,
	[IsPrimary] BIT NOT NULL DEFAULT(0),
	[EffectiveDate] DATE NULL,
	[ExpirationDate] DATE NULL,
    [IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_ContactAddress_ContactAddressID]	PRIMARY KEY CLUSTERED ( ContactAddressID ASC),
    CONSTRAINT [FK_ContactAddress_AddressID]	FOREIGN KEY ([AddressID]) REFERENCES [Core].[Addresses] ([AddressID]) ON DELETE CASCADE,
    CONSTRAINT [FK_ContactAddress_ContactID]	FOREIGN KEY ([ContactID]) REFERENCES [Registration].[Contact] ([ContactID]) ON DELETE CASCADE,
	CONSTRAINT [FK_ContactAddress_MailPermissionID]	FOREIGN KEY ([MailPermissionID]) REFERENCES [Reference].[MailPermission] ([MailPermissionID]) ON DELETE CASCADE,
);

GO
CREATE NONCLUSTERED INDEX [IX_ContactID]
    ON [Registration].[ContactAddress]([ContactID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_AddressID]
    ON [Registration].[ContactAddress]([AddressID] ASC);

	GO

CREATE NONCLUSTERED INDEX [IX_ContactAddress_SystemCreatedOn] ON [Registration].[ContactAddress]
(
	[SystemCreatedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [IX_ContactAddress_SystemModifiedOn] ON [Registration].[ContactAddress]
(
	[SystemModifiedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE Registration.ContactAddress WITH CHECK ADD CONSTRAINT [FK_ContactAddress_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ContactAddress CHECK CONSTRAINT [FK_ContactAddress_UserModifedBy]
GO
ALTER TABLE Registration.ContactAddress WITH CHECK ADD CONSTRAINT [FK_ContactAddress_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ContactAddress CHECK CONSTRAINT [FK_ContactAddress_UserCreatedBy]
GO


