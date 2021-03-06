-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[CallCenter].[usp_GetCallCenterAssessmentResponse]
-- Author:		Rajiv Ranjan
-- Date:		06/14/2016
--
-- Purpose:		Get a list of CallCenterAssessmentResponse by CallCenterHeaderID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/14/2016	Rajiv Ranjan	Initial creation.
-- 12/12/2016	Gurpreet Singh	Modified sproc to return all records if AssessmentID is null
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [CallCenter].[usp_GetCallCenterAssessmentResponse]
    @CallCenterHeaderID BIGINT,
	@AssessmentID BIGINT,
    @ResultCode INT OUTPUT,
    @ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
    BEGIN TRY
		SELECT @ResultCode = 0,
			@ResultMessage = 'executed successfully'

		SELECT 
			CCAR.CallCenterAssessmentResponseID,
			CCAR.CallCenterHeaderID,
			CCAR.[AssessmentID],
			CCAR.[ResponseID],
			CCAR.ModifiedBy, 
			CCAR.ModifiedOn
		FROM 
			[CallCenter].[CallCenterAssessmentResponse]  CCAR
		WHERE 
			CCAR.IsActive = 1 
			AND CCAR.CallCenterHeaderID = @CallCenterHeaderID
			AND (ISNULL(@AssessmentID, 0) = 0 OR CCAR.AssessmentID=@AssessmentID)
		ORDER BY
			CCAR.ModifiedOn DESC

	END TRY
    BEGIN CATCH
            SELECT @ResultCode = ERROR_SEVERITY(),
                    @ResultMessage = ERROR_MESSAGE()
    END CATCH
END