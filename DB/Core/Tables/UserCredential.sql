-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Core.UserCredential
-- Author:		John Crossen
-- Date:		08/12/2014
--
-- Purpose:		Provide a brief description of what your function does.
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		Reference.Credentials and Core.Users
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/12/2014	John Crossen		TFS# 885 - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-- 03/29/2016 - Justin Spalti - Added StateIssuedByID to the table and Referencesd it as a FK
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[UserCredential] (
    [UserCredentialID]      BIGINT         IDENTITY (1, 1) NOT NULL,
    [UserID]                INT            NOT NULL,
    [CredentialID]          BIGINT         NOT NULL,
    [LicenseNbr]            NVARCHAR (100) NULL,
	[StateIssuedByID]       INT            NULL,
    [LicenseIssueDate]      DATE           NULL,
    [LicenseExpirationDate] DATE           NULL,
    [IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_UserCredential] PRIMARY KEY CLUSTERED ([UserCredentialID] ASC),
    CONSTRAINT [FK_UserCredential_Users] FOREIGN KEY ([UserID]) REFERENCES [Core].[Users] ([UserID]),
	CONSTRAINT [FK_UserCredential_StateProvince] FOREIGN KEY ([StateIssuedByID]) REFERENCES [Reference].[StateProvince] ([StateProvinceID])
);
GO

ALTER TABLE Core.UserCredential WITH CHECK ADD CONSTRAINT [FK_UserCredential_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.UserCredential CHECK CONSTRAINT [FK_UserCredential_UserModifedBy]
GO
ALTER TABLE Core.UserCredential WITH CHECK ADD CONSTRAINT [FK_UserCredential_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.UserCredential CHECK CONSTRAINT [FK_UserCredential_UserCreatedBy]
GO
