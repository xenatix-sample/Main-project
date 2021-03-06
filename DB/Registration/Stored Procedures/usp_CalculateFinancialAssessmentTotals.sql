 	
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_CalculateFinancialAssessmentTotals]
-- Author:		Sumana Sangapu
-- Date:		08/11/2015
--
-- Purpose:		Calculate the Total Income and Total Expenses for Financial Assessment Screen for the assessment year next 12 months from the Assessment Date. 
--				FinanceFrequency table holds common units of measure by week.
		
--
-- Notes:		Calculate the Total Income and Total Expenses for the past year 
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/11/2015   Sumana Sangapu	Task # 634 Initial Creation
-- 08/27/2015	Sumana Sangapu	1515	Refactor Finance Screen
-- 09/01/2015	Suresh Pandey	In case no record is active, set 0 in TotalIncome,TotalExpenses and AdjustedGrossIncome
-- 09/01/2015	Suresh Pandey	Check Income is null in AdjustedGrossIncome calcualtion 
-- 09/18/2015	Sumana Sangapu	2370 Add/Remove fields to FA screen
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_CalculateFinancialAssessmentTotals]
	@AssessmentDate date,
	@FinancialAssessmentID bigint,
	@ContactID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'
			   		
	BEGIN TRY
	DECLARE @Totals TABLE
	(
		[FinancialAssessmentID] BIGINT,
		[CategoryType] NVARCHAR(100),
		Totals DECIMAL (38,2)
	);

	-- Calculate the total Income and Expenses for the FinancialAssessmentID
	INSERT INTO @Totals
	SELECT
		fad.FinancialAssessmentID,
		CategoryType,				
		SUM(Amount * CONVERT(DECIMAL(15,2), FrequencyFactor)) AS Totals  
	FROM
		[Registration].[FinancialAssessmentDetails] fad
		INNER JOIN [Reference].[FinanceFrequency] f
			ON fad.FinanceFrequencyID = f.FinanceFrequencyID 
		INNER JOIN [Reference].[Category] c
			ON c.CategoryID = fad.CategoryID
		INNER JOIN [Reference].[CategoryType] ct
			ON ct.CategoryTypeID = c.CategoryTypeID
	WHERE
		FinancialAssessmentID = @FinancialAssessmentID
		AND fad.IsActive = 1
	GROUP BY
		fad.FinancialAssessmentID,
		CategoryType

	IF  EXISTS (SELECT 1 FROM @Totals)
		BEGIN
		-- Update the TotalIncome, TotalExpenses, TotalExtraOrdinaryExpenses, TotalOther and AdjustedGrossIncome after pivoting the Totals from @Totals	
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'FinancialAssessment', @FinancialAssessmentID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
											
		UPDATE f   
		SET TotalIncome = ISNULL(Income,0),
			TotalExpenses = ISNULL(Expense,0),
			TotalExtraOrdinaryExpenses = ISNULL([Extraordinary Expenses],0),
			TotalOther = ISNULL([Other],0),
			AdjustedGrossIncome = ISNULL(Income,0) - (ISNULL(Expense,0) + ISNULL([Extraordinary Expenses],0))
		FROM
			[Registration].[FinancialAssessment] f
			INNER JOIN @Totals SRC PIVOT ( Sum(Totals) FOR CategoryType in ([Income],[Expense],[Extraordinary Expenses],[Other]) ) pvt
				ON f.FinancialAssessmentID = pvt.FinancialAssessmentID
		WHERE
			f.FinancialAssessmentID = @FinancialAssessmentID  
			AND	f.ContactID = @ContactID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'FinancialAssessment', @AuditDetailID, @FinancialAssessmentID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END
	ELSE
		BEGIN
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'FinancialAssessment', @FinancialAssessmentID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE [Registration].[FinancialAssessment]    
		SET TotalIncome = 0.00,
			TotalExpenses = 0.00,
			TotalExtraOrdinaryExpenses = 0.00,
			TotalOther = 0.00,
			AdjustedGrossIncome = 0.00
		WHERE
			FinancialAssessmentID = @FinancialAssessmentID  
			AND ContactID = @ContactID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'FinancialAssessment', @AuditDetailID, @FinancialAssessmentID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END
			
	END TRY

	BEGIN CATCH
			SELECT  @ResultCode = ERROR_SEVERITY(),
					@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
