-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetAssessmentsList]
-- Author:		Vishal Yadav
-- Date:		06/10/2016
--
-- Purpose:		Gets the list of Assessments
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/17/2015	Vishal Yadav	Initial creation.
 -----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetAssessmentsList]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage =  'Executed successfully'

	BEGIN TRY
			SELECT  
				a.AssessmentID,
				a.Name as AssessmentName
			FROM	
				[Core].[Assessments] a 
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END