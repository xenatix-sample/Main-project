

----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Core.[usp_GetAssessmentQuestionOptions]
-- Author:		Sumana Sangapu
-- Date:		09/17/2015
--
-- Purpose:		Get the options for assessment questions  
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/26/2015	-	Sumana Sangapu	- Task 1601	- Initial Creation
-- 10/11/2015	-	Demetrios C. Christopher - Eliminated unnecessary parameters
-- 01/18/2016	-	Demetrios C. Christopher - Task 5307 - Allowed null into @AssessmentSectionID for getting all data
-- 01/27/2016	-   Sumana Sangapu	- 5717 - Sort Order is partitioned over OptionsID 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetAssessmentQuestionOptions]
	@AssessmentSectionID BIGINT = NULL,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT 
AS
BEGIN

		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully'
		
		BEGIN TRY

				SELECT	
					q1.QuestionID AS QuestionID, 
					q1.ParentQuestionID AS ParentQuestionID, 
					q1.OptionsGroupID AS OptionsGroupID, 
					OptionsGroupName, 
					o.OptionsID, 
					o.Options,
					o.Attributes,
					ROW_NUMBER() OVER (ORDER BY q1.QuestionID, o.SortOrder, OptionsID) AS SortOrder 
				FROM	
					[Core].[AssessmentQuestions] q
					LEFT JOIN [Core].[AssessmentQuestions] q1
					ON q.QuestionID = q1.ParentQuestionID
					LEFT JOIN [Core].[AssessmentSections] s
					ON	q1.AssessmentID = s.AssessmentID
					AND s.AssessmentSectionID = q1.AssessmentSectionID					
					LEFT JOIN [Core].[AssessmentOptionsGroup] og
					ON og.OptionsGroupID = q1.OptionsGroupID
					LEFT JOIN [Core].[AssessmentOptions] o
					ON o.OptionsGroupID = og.OptionsGroupID 
				WHERE	
					s.AssessmentSectionID = ISNULL(@AssessmentSectionID, s.AssessmentSectionID)
					AND	q1.ParentQuestionID IS NOT NULL 

		END TRY
		BEGIN CATCH
				SELECT  @ResultCode = ERROR_SEVERITY(),
						@ResultMessage = ERROR_MESSAGE()
		END CATCH

END
