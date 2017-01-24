-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetFinancialAssessment]
-- Author:		Suresh Pandey
-- Date:		08/17/2015
--
-- Purpose:		Get Financial Assessment data List 
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/17/2015	Suresh Pandey Task	Initial creation
-- 08/21/2015	Suresh Pandey change where clause to handle if FinancialAssessmentID is comming as 0/null then select on the base of contact id else on both.
--								also added TOP 1 in select statement to return only one Financial Assessment for the contact.	
-- 09/18/2015	Sumana Sangapu	2370	Remove and Add fields to FA screen
-- 09/21/2015	Suresh Pandey	2370	Added ExpirationDate and ExpirationReasonID in select 
-- 10/31/2015 - Arun Choudhary - Added modifiedon in select. Needed in tiles flyout.
-- 09/16/2016 - Atul Chauhan -  Will return entire data for contact in case of FinancialAssessmentID=0 as per PBI 14212
-- 10/18/2016	Scott Martin	Added CreatedBy column to results
-- exec [Registration].[usp_GetFinancialAssessment] 1,3,'',''

----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetFinancialAssessment]
@ContactID BIGINT,
@FinancialAssessmentID	BIGINT =null,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY

	;WITH CTE_FinancialSummary AS
	(
		SELECT DISTINCT FinancialAssessmentXML.value('(/FinancialSummary/FinancialAssessment/FinancialAssessmentID/node())[1]','NVARCHAR(MAX)') AS FinancialAssessmentID
		FROM  Registration.FinancialSummary 
	)
	SELECT  FA.FinancialAssessmentID,
			ContactID,
			AssessmentDate,
			ExpirationDate,
			ExpirationReasonID,
			ISNULL(TotalIncome,0)As TotalIncome,
			ISNULL(TotalExpenses,0)As TotalExpenses,
			ISNULL(TotalExtraOrdinaryExpenses,0)As TotalExtraOrdinaryExpenses,
			ISNULL(TotalOther,0)As TotalOther,
			ISNULL(AdjustedGrossIncome,0)AdjustedGrossIncome,
			Cast(FamilySize AS INT) AS FamilySize,
			ModifiedOn,
			ModifiedBy,
			CreatedBy,
			Case when FAS.FinancialAssessmentID is null then  CAST(0 AS BIT) else CAST(1 AS BIT) end IsViewFinanicalAssessment
	FROM	Registration.FinancialAssessment FA
			Left Join CTE_FinancialSummary FAS on FA.FinancialAssessmentID=FAS.FinancialAssessmentID 
	WHERE   (FA.FinancialAssessmentID = @FinancialAssessmentID OR ISNULL(@FinancialAssessmentID,0) = 0)
	AND		ContactID = @ContactID
	AND		IsActive =1 
	ORDER BY AssessmentDate DESC
		
	END TRY
	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END