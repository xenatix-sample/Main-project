-----------------------------------------------------------------------------------------------------------------------
-- Table:		Registration.[ContactPhone]
-- Author:		Saurabh Sahu
-- Date:		07/23/2015
--
-- Purpose:		Store Contact Phone relationship data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		Phone and Contact Table (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Saurabh Sahu		Modification .
-- 08/12/2015	Rajiv Ranjan		- Removed unique constraint on IsPreferred field
-- 08/13/2015	Sumana Sangapu		Task #: 1227 - Refactor schema
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 08/26/2016	Scott Martin	Added index
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[ContactPhone] (
	[ContactPhoneID] BIGINT IDENTITY(1,1) NOT NULL,
    [ContactID] BIGINT NOT NULL,
    [PhoneID] BIGINT NOT NULL,
	[PhonePermissionID] int NULL,
	[IsPrimary] BIT NOT NULL DEFAULT(0),
	[IsActive] BIT NOT NULL DEFAULT(1),
	[EffectiveDate] DATE NULL,
	[ExpirationDate] DATE NULL,
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_ContactPhone_ContactID]			PRIMARY KEY CLUSTERED ([ContactPhoneID] ASC),
    CONSTRAINT [FK_ContactPhone_ContactID]			FOREIGN KEY ([ContactID]) REFERENCES [Registration].[Contact] ([ContactID]),
    CONSTRAINT [FK_ContactPhone_PhoneID]			FOREIGN KEY ([PhoneID]) REFERENCES [Core].[Phone] ([PhoneID]) ,
	CONSTRAINT [FK_ContactPhone_PhonePermissionID]			FOREIGN KEY ([PhonePermissionID]) REFERENCES [Reference].[PhonePermission] ([PhonePermissionID])    
);

GO

CREATE NONCLUSTERED INDEX [IX_ContactPhone_ContactID] ON [Registration].[ContactPhone]
(
	[ContactID] ASC,
	[PhoneID] ASC,
	[IsPrimary] ASC
)
INCLUDE ( 	[SystemCreatedOn],
	[SystemModifiedOn]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE Registration.ContactPhone WITH CHECK ADD CONSTRAINT [FK_ContactPhone_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ContactPhone CHECK CONSTRAINT [FK_ContactPhone_UserModifedBy]
GO
ALTER TABLE Registration.ContactPhone WITH CHECK ADD CONSTRAINT [FK_ContactPhone_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ContactPhone CHECK CONSTRAINT [FK_ContactPhone_UserCreatedBy]
GO





