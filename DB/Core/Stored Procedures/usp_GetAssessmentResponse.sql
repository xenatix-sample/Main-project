
----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Core.[usp_GetAssessmentResponse]
-- Author:		Demetrios C. Christopher
-- Date:		10/11/2015
--
-- Purpose:		Get a specific assessment response
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/11/2015	-	Demetrios C. Christopher - Initial Creation (cloned from [usp_GetAssessmentResponses]
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetAssessmentResponse]
	@ResponseID bigint,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT 

AS
BEGIN

		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully'
		
		BEGIN TRY

			SELECT  
				ResponseID,
				ContactID,
				AssessmentID,
				EnterDate
			FROM	
				Core.AssessmentResponses a
			WHERE
				a.ResponseID = @ResponseID 

		END TRY
		BEGIN CATCH
				SELECT  @ResultCode = ERROR_SEVERITY(),
						@ResultMessage = ERROR_MESSAGE()
		END CATCH
END
