-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[UserIdentifierDetails]
-- Author:		Sumana Sangapu
-- Date:		04/06/2016
--
-- Purpose:		Stores the UserIdentifierDetails. Additional details like StaffIDNumber, NPI Number, DEA Number etc.
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/06/2016	Sumana Sangapu		Initial Creation
-- 09/06/2016	Rahul Vats		Reviewed the Table 
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Core].[UserIdentifierDetails] (
	[UserIdentifierDetailsID]	BIGINT IDENTITY(1,1) NOT NULL,
    [UserID]   INT   NOT NULL,
    [UserIdentifierTypeID]     INT      NOT NULL,
	[IDNumber] NVARCHAR(100)  NULL,
	[EffectiveDate] DATE NULL,
	[ExpirationDate] DATE NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_UserIdentifierTypeDetails_UserIdentifierTypeDetailsID] PRIMARY KEY CLUSTERED ([UserIdentifierDetailsID] ASC),
    CONSTRAINT [FK_UserIdentifierTypeDetails_UserID] FOREIGN KEY ([UserID]) REFERENCES [Core].[Users] ([UserID]),
    CONSTRAINT [FK_UserIdentifierTypeDetails_UserIdentifierTypeID]	FOREIGN KEY ([UserIdentifierTypeID]) REFERENCES [Reference].[UserIdentifierType] ([UserIdentifierTypeID])
)
GO

ALTER TABLE Core.UserIdentifierDetails WITH CHECK ADD CONSTRAINT [FK_UserIdentifierDetails_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.UserIdentifierDetails CHECK CONSTRAINT [FK_UserIdentifierDetails_UserModifedBy]
GO
ALTER TABLE Core.UserIdentifierDetails WITH CHECK ADD CONSTRAINT [FK_UserIdentifierDetails_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.UserIdentifierDetails CHECK CONSTRAINT [FK_UserIdentifierDetails_UserCreatedBy]
GO
