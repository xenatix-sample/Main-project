-----------------------------------------------------------------------------------------------------------------------
-- Table:		Registration.[FinancialSummary]
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
-- 07/06/2016	Scott Martin	Added AssessmentEndDate
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[FinancialSummary]
(
	[FinancialSummaryID] BIGINT NOT NULL IDENTITY(1,1),
	[ContactID] BIGINT NOT NULL,
	[OrganizationID] BIGINT NOT NULL,
	[FinancialAssessmentXML] XML NULL,
	[DateSigned] DATE NULL,
	[EffectiveDate] DATE NULL,
	[AssessmentEndDate] DATE NULL,
	[ExpirationDate] DATE NULL,
	[SignatureStatusID] INT NOT NULL,
	[UserID] INT NOT NULL,
	[UserPhoneID] BIGINT NULL,
	[CredentialID] BIGINT NULL,
    [IsActive] BIT NOT NULL DEFAULT (1), 
    [ModifiedBy] INT NOT NULL, 
    [ModifiedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()), 
    [CreatedBy] INT NOT NULL DEFAULT (1), 
    [CreatedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()), 
    [SystemCreatedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()), 
    [SystemModifiedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()),
	CONSTRAINT [PK_FinancialSummary_FinancialSummaryID] PRIMARY KEY CLUSTERED ([FinancialSummaryID] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	CONSTRAINT [FK_FinancialSummary_ContactID]	FOREIGN KEY ([ContactID]) REFERENCES [Registration].[Contact] ([ContactID]),   	
	CONSTRAINT [FK_FinancialSummary_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID]),
	CONSTRAINT [FK_FinancialSummary_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID]),
	CONSTRAINT [FK_FinancialSummary_OrganizationID] FOREIGN KEY ([OrganizationID]) REFERENCES [Core].[OrganizationDetailsMapping] ([MappingID]),
	CONSTRAINT [FK_FinancialSummary_SignatureStatusID] FOREIGN KEY ([SignatureStatusID]) REFERENCES [Reference].[SignatureStatus] ([SignatureStatusID]),
	CONSTRAINT [FK_FinancialSummary_UserID] FOREIGN KEY ([UserID]) REFERENCES [Core].[Users] ([UserID]),
	CONSTRAINT [FK_FinancialSummary_UserPhoneID] FOREIGN KEY ([UserPhoneID]) REFERENCES [Core].[UserPhone] ([UserPhoneID]),
	CONSTRAINT [FK_FinancialSummary_CredentialID] FOREIGN KEY ([CredentialID]) REFERENCES [Reference].[Credentials] ([CredentialID])
)
GO
