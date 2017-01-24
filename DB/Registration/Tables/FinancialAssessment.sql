-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Registration].[FinancialAssessment]
-- Author:		Sumana Sangapu
-- Date:		08/06/2015
--
-- Purpose:		Income Details for Finance Screen
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/06/2015	Sumana Sangapu	634		Initial creation.
-- 09/18/2015	Sumana Sangapu	2370	Remove and Add fields to FA screen
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 08/26/2016	Scott Martin	Added index
-- 09/07/2016	Scott Martin	Removed unique constraint
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[FinancialAssessment](
	[FinancialAssessmentID] [bigint] IDENTITY(1,1) NOT NULL,
	[ContactID] [bigint] NOT NULL,
	[AssessmentDate] [DATE] NOT NULL,
	[TotalIncome] [decimal](15, 2) NULL DEFAULT (0.00),
	[TotalExpenses] [decimal](15, 2)  NULL DEFAULT(0.00),
	[TotalExtraOrdinaryExpenses] [decimal](15, 2)  NULL DEFAULT(0.00),
	[TotalOther] [decimal](15, 2)  NULL DEFAULT(0.00),
	[AdjustedGrossIncome] [decimal](15, 2) NULL DEFAULT(0.00),
	[FamilySize] [tinyint] NULL,
	[ExpirationDate]						DATE		NULL,
	[ExpirationReasonID]					INT			NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_FinancialAssessment_FinancialAssessmentID] PRIMARY KEY CLUSTERED 
(
	[FinancialAssessmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE NONCLUSTERED INDEX [IX_FinancialAssessment_ContactID] ON [Registration].[FinancialAssessment]
(
	[ContactID] ASC
)
INCLUDE ( 	[FinancialAssessmentID],
	[TotalIncome],
	[FamilySize],
	[SystemCreatedOn],
	[SystemModifiedOn]
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE [Registration].[FinancialAssessment]  WITH CHECK ADD  CONSTRAINT [FK_FinancialAssessment_ContactID] FOREIGN KEY([ContactID]) REFERENCES [Registration].[Contact] ([ContactID])
GO
ALTER TABLE [Registration].[FinancialAssessment] CHECK CONSTRAINT [FK_FinancialAssessment_ContactID]
GO
ALTER TABLE Registration.FinancialAssessment WITH CHECK ADD CONSTRAINT [FK_FinancialAssessment_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.FinancialAssessment CHECK CONSTRAINT [FK_FinancialAssessment_UserModifedBy]
GO
ALTER TABLE Registration.FinancialAssessment WITH CHECK ADD CONSTRAINT [FK_FinancialAssessment_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.FinancialAssessment CHECK CONSTRAINT [FK_FinancialAssessment_UserCreatedBy]
GO
