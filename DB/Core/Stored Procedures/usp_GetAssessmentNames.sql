-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetAssessmentNames]
-- Author:		Scott Martin
-- Date:		11/17/2015
--
-- Purpose:		Gets the list of Assessment Names
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/17/2015	Scott Martin	Initial creation.
 -----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetAssessmentNames]
	@DocumentTypeID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage =  'Executed successfully'

	BEGIN TRY	

				SELECT  
					a.AssessmentID,
					a.Name as AssessmentName, 
					dm.ScreeningTypeID as ScreeningTypeID
				FROM	
					[Core].[Assessments] a 
					LEFT JOIN [Core].[DocumentMapping] dm
					ON a.AssessmentID = dm.AssessmentID
				WHERE dm.DocumentTypeID = @DocumentTypeID

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END