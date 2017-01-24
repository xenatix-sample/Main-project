
----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Core.[usp_GetAssessmentDocumentType]
-- Author:		Atul Chauhan
-- Date:		11/04/2016
--
-- Purpose:		Get the assessments Document Type ID
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/04/2016	-	Atul Chauhan-Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetAssessmentDocumentType]
	@AssessmentID BIGINT = NULL,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT 

AS
BEGIN

		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully'
		
		BEGIN TRY

				SELECT  
					DM.DocumentTypeID as Result
				FROM	
					Core.DocumentMapping DM
				WHERE	
					AssessmentID=@AssessmentID

		END TRY
		BEGIN CATCH
				SELECT  @ResultCode = ERROR_SEVERITY(),
						@ResultMessage = ERROR_MESSAGE()
		END CATCH
END