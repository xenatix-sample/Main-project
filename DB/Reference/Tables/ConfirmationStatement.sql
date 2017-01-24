-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[ConfirmationStatement]
-- Author:		Scott Martin
-- Date:		04/09/2016
--
-- Purpose:		Look up table for Financial Assessment confirmation statements
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/09/2016	Scott Martin	Initial Creation
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[ConfirmationStatement]
(
	[ConfirmationStatementID] INT IDENTITY (1,1) NOT NULL,
	[ConfirmationStatement] NVARCHAR(MAX) NOT NULL,
	[ConfirmationStatementGroupID] INT NOT NULL,
	[DocumentTypeID] INT NOT NULL,
	[IsSignatureRequired] BIT NOT NULL DEFAULT(0),
    [IsActive] BIT NOT NULL DEFAULT (1), 
    [ModifiedBy] INT NOT NULL, 
    [ModifiedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()), 
    [CreatedBy] INT NOT NULL DEFAULT (1), 
    [CreatedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [PK_ConfirmationStatement_ConfirmationStatementID] PRIMARY KEY CLUSTERED ([ConfirmationStatementID] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	CONSTRAINT [FK_ConfirmationStatement_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID]),
	CONSTRAINT [FK_ConfirmationStatement_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID]),
	CONSTRAINT [FK_ConfirmationStatement_ConfirmationStatementGroupID] FOREIGN KEY ([ConfirmationStatementGroupID]) REFERENCES [Reference].[ConfirmationStatementGroup] ([ConfirmationStatementGroupID]),
	CONSTRAINT [FK_ConfirmationStatement_DocumentTypeID] FOREIGN KEY ([DocumentTypeID]) REFERENCES [Reference].[DocumentType] ([DocumentTypeID])
) ON [PRIMARY]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Confirmation Statement', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ConfirmationStatement;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating the confirmation verbiage for letters/forms/etc', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ConfirmationStatement;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ConfirmationStatement;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'0', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ConfirmationStatement;
GO;
