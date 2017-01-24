
----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Core.[usp_GetAssessmentResponses]
-- Author:		Rajiv Ranjan
-- Date:		09/17/2015
--
-- Purpose:		Get the assessment responses
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/18/2015	-	Rajiv Ranjan - Initial Creation
-- 02/01/2016	-	Rajiv Ranjan - Parameter order was incorrect so re-arranged.

-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetAssessmentResponses]
	@ContactID bigint,
	@AssessmentID bigint,
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
				a.AssessmentID = @AssessmentID 
				AND a.ContactID= @ContactID

		END TRY
		BEGIN CATCH
				SELECT  @ResultCode = ERROR_SEVERITY(),
						@ResultMessage = ERROR_MESSAGE()
		END CATCH
END
