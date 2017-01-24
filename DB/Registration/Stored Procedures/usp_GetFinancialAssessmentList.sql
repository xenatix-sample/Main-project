-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetFinancialAssessmentList]
-- Author:		Suresh Pandey
-- Date:		08/21/2015
--
-- Purpose:		Get Financial Assessment List for contact id
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/21/2015	Suresh Pandey Task - 634	Initial creation
-- exec [Registration].[usp_GetFinancialAssessmentList] 1,'',''
----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetFinancialAssessmentList]
@ContactID BIGINT,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY

	SELECT  FinancialAssessmentID,
			AssessmentDate			
	FROM	Registration.FinancialAssessment
	WHERE   ContactID = @ContactID
	AND		IsActive =1 
	ORDER BY AssessmentDate DESC
		
	END TRY
	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
