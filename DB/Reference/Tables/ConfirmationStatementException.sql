-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[ConfirmationStatementException]
-- Author:		Scott Martin
-- Date:		04/09/2016
--
-- Purpose:		Maps the organization levels that should not get a specific confirmation statement
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/09/2016	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[ConfirmationStatementException]
(
	[ConfirmationStatementExceptionID] INT IDENTITY (1,1) NOT NULL,
	[ConfirmationStatementID] INT NOT NULL,
	[OrganizationDetailID] BIGINT NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT (1), 
    [ModifiedBy] INT NOT NULL, 
    [ModifiedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()), 
    [CreatedBy] INT NOT NULL DEFAULT (1), 
    [CreatedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [PK_ConfirmationStatementException_ConfirmationStatementExceptionID] PRIMARY KEY CLUSTERED ([ConfirmationStatementExceptionID] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	CONSTRAINT [FK_ConfirmationStatementException_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID]),
	CONSTRAINT [FK_ConfirmationStatementException_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID]),
	CONSTRAINT [FK_ConfirmationStatementException_ConfirmationStatementID] FOREIGN KEY ([ConfirmationStatementID]) REFERENCES [Reference].[ConfirmationStatement] ([ConfirmationStatementID]),
	CONSTRAINT [FK_ConfirmationStatementException_OrganizationDetailID] FOREIGN KEY ([OrganizationDetailID]) REFERENCES [Core].[OrganizationDetails] ([DetailID])
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'References Reference.ConfirmationStatment', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ConfirmationStatementException,
@level2type = N'COLUMN', @level2name = ConfirmationStatementID;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'References Core.OrganizationDetails', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ConfirmationStatementException,
@level2type = N'COLUMN', @level2name = OrganizationDetailID;
GO
