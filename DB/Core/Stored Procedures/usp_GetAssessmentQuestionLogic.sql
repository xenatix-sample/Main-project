----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Core.[usp_GetAssessmentQuestionLogic]
-- Author:		Kyle Campbell
-- Date:		03/23/2016
--
-- Purpose:		Get the logic code for assessment questions
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/23/2016	Kyle Campbell	TFS #8273	Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Core].[usp_GetAssessmentQuestionLogic]
	@AssessmentSectionID BIGINT = NULL,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT 
AS
BEGIN
		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully'
		
		BEGIN TRY
			SELECT	AQLM.QuestionDataKey, 
					LT.LogicType, 
					AL.LogicCode, 
					AQLM.LogicOrder
			FROM Core.AssessmentQuestions AQ 
				INNER JOIN Core.AssessmentQuestionLogicMapping AQLM ON AQ.DataKey = AQLM.QuestionDataKey
				INNER JOIN Core.AssessmentLogic AL ON AQLM.LogicID = AL.LogicID
				LEFT JOIN Reference.LogicType LT ON AL.LogicTypeID = LT.LogicTypeID
			WHERE 
				(ISNULL(@AssessmentSectionID,0) = 0 OR AssessmentSectionID = @AssessmentSectionID)
				AND AQLM.IsActive = 1
				AND AL.IsActive = 1
				AND AQ.IsActive = 1
		END TRY

		BEGIN CATCH
				SELECT  @ResultCode = ERROR_SEVERITY(),
						@ResultMessage = ERROR_MESSAGE()
		END CATCH
END
