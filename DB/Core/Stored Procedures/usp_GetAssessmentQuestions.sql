/****** Object:  StoredProcedure [Core].[usp_GetAssessmentQuestions]    Script Date: 10/6/2015 9:40:43 AM ******/

----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Core.[usp_GetAssessmentQuestions]
-- Author:		Sumana Sangapu
-- Date:		09/17/2015
--
-- Purpose:		Get the questions for the assessments
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/18/2015	-	Sumana Sangapu	- Task 1601	- Initial Creation
-- 10/07/2015	-	Rajiv Ranjan	- Refactored questions & response into seperate proc
-- 10/11/2015	-	Demetrios C. Christopher - Eliminated unnecessary parameters
-- 01/15/2016	-	Rajiv Ranjan	- Task 5307	- Refactor proc, allowed null into @AssessmentSectionID for getting all data
-- 04/21/2016	Kyle Campbell	TFS #10179	Added ClassName, Attributes, ValidationMessage columns
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetAssessmentQuestions]
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
					s.AssessmentSectionID as AssessmentSectionID,
					s.SectionName as SectionName,
					q1.ParentQuestionID AS ParentQuestionID, 
					q1.Question AS Question, 
					qt.QuestionTypeID,
					qt.QuestionType AS QuestionType, 
					StyleClassName,
					it.InputTypeID,
					it.InputType AS InputType, 
					q1.OptionsGroupID,
					itp.InputTypePositionID,
					itp.InputTypePosition, 
					q1.IsNumberingRequired AS IsNumberingRequired, 
					q1.IsAnswerRequired AS IsAnswerRequired, 
					i.ImageID AS ImageID,
					--ROW_NUMBER() OVER (ORDER BY q1.QuestionID) AS SortOrder
					q1.SortOrder,
					q1.DataKey,
					q1.[ParentOptionsID],
					q1.ContainerAttributes,
					q1.Attributes
				FROM	
					[Core].[AssessmentQuestions] q
					LEFT JOIN [Core].[AssessmentQuestions] q1
					ON q.QuestionID = q1.ParentQuestionID
					LEFT JOIN [Core].[AssessmentSections] s
					ON	q1.AssessmentID = s.AssessmentID
					AND s.AssessmentSectionID = q1.AssessmentSectionID					
					LEFT JOIN [Reference].[AssessmentQuestionType] qt
					ON qt.QuestionTypeID = q1.QuestionTypeID
					LEFT JOIN [Reference].[AssessmentInputType] it
					ON q1.InputTypeID = it.InputTypeID
					LEFT JOIN [Reference].[AssessmentInputTypePosition] itp
					ON itp.InputTypePositionID = q1.InputTypePositionID 
					LEFT JOIN [Core].[Images] i
					ON i.ImageID = q1.ImageID	 
				WHERE	
					(ISNULL(@AssessmentSectionID,0) = 0 OR s.AssessmentSectionID = @AssessmentSectionID)
					AND q1.ParentQuestionID IS NOT NULL 
					AND q1.IsActive = 1					
				ORDER BY q1.SortOrder asc
		END TRY
		BEGIN CATCH
				SELECT  @ResultCode = ERROR_SEVERITY(),
						@ResultMessage = ERROR_MESSAGE()
		END CATCH

END
