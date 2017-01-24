-----------------------------------------------------------------------------------------------------------------------
-- Table:		Registration.[ContactEmail]
-- Author:		Saurabh Sahu
-- Date:		07/23/2015
--
-- Purpose:		Store Contact and Email relationship data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		Email Table, Address Table (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Saurabh Sahu		Modification .
-- 7/31/2015    John Crossen    Change schema from dbo to Registration
-- 08/13/2015	Sumana Sangapu		Task #: 1227 - Refactor schema
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[ContactEmail] (
	[ContactEmailID] BIGINT NOT NULL IDENTITY (1,1),
    [ContactID] BIGINT NOT NULL,
    [EmailID] BIGINT NOT NULL,
    [EmailPermissionID] INT NULL,
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
	CONSTRAINT [PK_ContactAddress_ContactEmailID]	PRIMARY KEY CLUSTERED ( ContactEmailID ASC),
    CONSTRAINT [FK_ContactEmail_ContactID]	FOREIGN KEY ([ContactID]) REFERENCES [Registration].[Contact] ([ContactID]),
    CONSTRAINT [FK_ContactEmail_EmailID]	FOREIGN KEY ([EmailID]) REFERENCES [Core].[Email] ([EmailID]),
	CONSTRAINT [FK_ContactEmail_EmailPermissionID]	FOREIGN KEY ([EmailPermissionID]) REFERENCES [Reference].[EmailPermission] ([EmailPermissionID])

);

GO

ALTER TABLE Registration.ContactEmail WITH CHECK ADD CONSTRAINT [FK_ContactEmail_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ContactEmail CHECK CONSTRAINT [FK_ContactEmail_UserModifedBy]
GO
ALTER TABLE Registration.ContactEmail WITH CHECK ADD CONSTRAINT [FK_ContactEmail_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ContactEmail CHECK CONSTRAINT [FK_ContactEmail_UserCreatedBy]
GO



