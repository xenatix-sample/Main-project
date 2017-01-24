  -----------------------------------------------------------------------------------------------------------------------
-- Table:		dbo.[FinancialAssessment_Staging_ErrorDetails]
-- Author:		Sumana Sangapu
-- Date:		05/19/2016
--
-- Purpose:		Table to hold the error details from the validation of lookup data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/19/2016	Sumana Sangapu		Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Synch].[FinancialAssessment_Staging_ErrorDetails](
	[FinancialAssessmentID] [bigint] NULL,
	[ContactID] [bigint] NULL,
	[AssessmentDate] [varchar](50) NULL,
	[TotalIncome] [varchar](50) NULL,
	[TotalExpenses] [varchar](50) NULL,
	[TotalExtraOrdinaryExpenses] [varchar](50) NULL,
	[TotalOther] [varchar](50) NULL,
	[AdjustedGrossIncome] [varchar](50) NULL,
	[FamilySize] [varchar](50) NULL,
	[ExpirationDate] [varchar](50) NULL,
	[ExpirationReasonID] [varchar](50) NULL,
	[IsActive] [varchar](50) NULL,
	[ModifiedBy] [varchar](50) NULL,
	[ModifiedOn] [varchar](50) NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedOn] [varchar](50) NULL,
	[SystemCreatedOn] [varchar](50) NULL,
	[SystemModifiedOn] [varchar](50) NULL,
	ErrorSource varchar(50) NULL
) ON [PRIMARY]

GO


