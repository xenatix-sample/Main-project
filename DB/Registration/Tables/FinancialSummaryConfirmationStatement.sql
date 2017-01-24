-----------------------------------------------------------------------------------------------------------------------
-- Table:		Registration.[FinancialSummaryConfirmationStatement]
-- Author:		Scott Martin
-- Date:		04/09/2016
--
-- Purpose:		Stores Financial Assessment historical data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/09/2016	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[FinancialSummaryConfirmationStatement]
(
	[FinancialSummaryConfirmationStatementID] BIGINT NOT NULL IDENTITY(1,1),
	[FinancialSummaryID] BIGINT NOT NULL,
	[ConfirmationStatementID] INT NOT NULL,
	[DateSigned] DATE NULL,
	[SignatureStatusID] INT NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT (1), 
    [ModifiedBy] INT NOT NULL, 
    [ModifiedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()), 
    [CreatedBy] INT NOT NULL DEFAULT (1), 
    [CreatedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()), 
    [SystemCreatedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()), 
    [SystemModifiedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()),
	CONSTRAINT [PK_FinancialSummaryConfirmationStatement_FinancialSummaryConfirmationStatementID] PRIMARY KEY CLUSTERED ([FinancialSummaryConfirmationStatementID] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	CONSTRAINT [FK_FinancialSummaryConfirmationStatement_FinancialSummaryID]	FOREIGN KEY ([FinancialSummaryID]) REFERENCES [Registration].[FinancialSummary] ([FinancialSummaryID]),   	
	CONSTRAINT [FK_FinancialSummaryConfirmationStatement_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID]),
	CONSTRAINT [FK_FinancialSummaryConfirmationStatement_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID]),
	CONSTRAINT [FK_FinancialSummaryConfirmationStatement_ConfirmationStatementID] FOREIGN KEY ([ConfirmationStatementID]) REFERENCES [Reference].[ConfirmationStatement] ([ConfirmationStatementID]),
	CONSTRAINT [FK_FinancialSummaryConfirmationStatement_SignatureStatusID] FOREIGN KEY ([SignatureStatusID]) REFERENCES [Reference].[SignatureStatus] ([SignatureStatusID])
)
GO
