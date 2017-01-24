-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Registration].[FinancialAssessmentDetails]
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
-- 08/05/2015	Sumana Sangapu	Task ID : 634 Initial Creation
-- 09/01/2015	Suresh Pandey	Changed FinancialAssessmentType to CategoryTypeID. [FinanceFrequencyID] and [EffectiveDate] become Nullable.
-- 09/18/2015	Sumana Sangapu	2370	Remove and Add fields to FA screen
-- 12/22/2015	Scott Martin	Added RelationshipTypeID
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[FinancialAssessmentDetails]
(
	[FinancialAssessmentDetailsID]			BIGINT      IDENTITY (1, 1) NOT NULL,
	[FinancialAssessmentID]					BIGINT		NOT NULL, 
	[CategoryTypeID]						INT NULL,
	[Amount]								DECIMAL(15, 2)	NOT NULL DEFAULT (0.00),
	[FinanceFrequencyID]					INT			NULL,
	[CategoryID]							INT		    NULL,
	[RelationshipTypeID]					INT			NOT NULL DEFAULT(39),
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [PK_FinancialAssessmentDetails_FinancialAssessmentDetailsID] PRIMARY KEY CLUSTERED 
	(
		[FinancialAssessmentDetailsID] ASC
	)
    WITH 
	(
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Registration].[FinancialAssessmentDetails]  WITH CHECK ADD  CONSTRAINT [FK_FinancialAssessmentDetails_FinancialAssessmentID] FOREIGN KEY([FinancialAssessmentID])
REFERENCES [Registration].[FinancialAssessment] ([FinancialAssessmentID])
GO

ALTER TABLE [Registration].[FinancialAssessmentDetails]  WITH CHECK ADD  CONSTRAINT [FK_FinancialAssessmentDetails_FinanceFrequencyID] FOREIGN KEY([FinanceFrequencyID])
REFERENCES [Reference].[FinanceFrequency] ([FinanceFrequencyID])
GO

ALTER TABLE [Registration].[FinancialAssessmentDetails]   WITH CHECK ADD  CONSTRAINT [FK_FinancialAssessmentDetails_CategoryID] FOREIGN KEY([CategoryID])
REFERENCES  [Reference].[Category]([CategoryID])
GO

ALTER TABLE  [Registration].[FinancialAssessmentDetails]  WITH CHECK ADD  CONSTRAINT [FK_FinancialAssessmentDetails_RelationshipType] FOREIGN KEY([RelationshipTypeID])
REFERENCES [Reference].[RelationshipType] ([RelationshipTypeID])
GO

ALTER TABLE  [Registration].[FinancialAssessmentDetails] CHECK CONSTRAINT [FK_FinancialAssessmentDetails_RelationshipType]
GO

ALTER TABLE Registration.FinancialAssessmentDetails WITH CHECK ADD CONSTRAINT [FK_FinancialAssessmentDetails_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.FinancialAssessmentDetails CHECK CONSTRAINT [FK_FinancialAssessmentDetails_UserModifedBy]
GO
ALTER TABLE Registration.FinancialAssessmentDetails WITH CHECK ADD CONSTRAINT [FK_FinancialAssessmentDetails_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.FinancialAssessmentDetails CHECK CONSTRAINT [FK_FinancialAssessmentDetails_UserCreatedBy]
GO
