-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddFinancialAssessmentDetails]
-- Author:		Sumana Sangapu
-- Date:		08/06/2015
--
-- Purpose:		Add Financial Assessment Details in Finance Screen
--
-- Notes:		Check if any assessments are present for that ContactID and AssessmentDate. IF yes get the corresponding AssessmentID else insert a record in 
--				FinancialAssessment table and retrieve the AssessmentID
--
-- Depends:		Registration.Contact
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/06/2015   Sumana Sangapu	634 - Initial Creation
-- 08/20/2015	Suresh Pandey	Added familysize parameter and use in insert statement for [FinancialAssessment] table 
-- 08/21/2015	Suresh Pandey	calling [usp_CalculateFinancialAssessmentTotals] sp at the end to calculate total.
-- 08/30/2015	Suresh Pandey	Adding @TotalIncome ,@TotalExpenses,@AdjustedGrossIncome as parameter to accept value from UI.
-- 09/01/2015	Suresh Pandey	Changed column FinancialAssessmentType to CategoryTypeID.
-- 09/02/2015	Suresh Pandey	Changed logic to add new record in FinancialAssessmentDetails table. If any of the column value is not nullable then only it will insert record.
--								Add logic to update the family size.
-- 09/04/2015	Sumana Sangapu	Modified the logic to update the assessment date for existing FinancialAssessmentID
-- 09/04/2015   Jason Smith     Fixed an issue with the MERGE statement that prevented new Financial Assessments from being created
-- 09/18/2015	Sumana Sangapu	2370	Remove and Add fields to FA screen
-- 09/21/2015	Suresh Pandey	2370	Remove parameters
-- 09/25/2015	D. Christopher	Separated CRUD between Assessment and Assessment Details
-- 12/16/2015	Scott Martin	Added audit logging
-- 12/23/2015	Scott Martin	Added RelationshipTypeID
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddFinancialAssessmentDetails]
	@FinancialAssessmentID BIGINT,
	@CategoryTypeID INT,
	@Amount DECIMAL (15, 2) = 0.00,
	@FinanceFrequencyID INT,
	@CategoryID INT,
	@RelationshipTypeID INT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID INT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT  @ID = 0,
			@ResultCode = 0,
			@ResultMessage = 'executed successfully';

	BEGIN TRY
	DECLARE @ContactID BIGINT;
	
	SELECT @ContactID = ContactID FROM Registration.FinancialAssessment WHERE FinancialAssessmentID = @FinancialAssessmentID;
		
	INSERT INTO	[Registration].[FinancialAssessmentDetails]
	(
		FinancialAssessmentID,
		CategoryTypeID,
		Amount,
		FinanceFrequencyID,
		CategoryID,
		RelationshipTypeID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@FinancialAssessmentID,
		@CategoryTypeID,
		@Amount,
		@FinanceFrequencyID,
		@CategoryID,
		@RelationshipTypeID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'FinancialAssessmentDetails', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'FinancialAssessmentDetails', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT 
				@ID = 0,
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END

