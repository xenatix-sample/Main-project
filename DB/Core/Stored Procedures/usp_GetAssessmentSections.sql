
----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Core.[usp_GetAssessmentSections]
-- Author:		Sumana Sangapu
-- Date:		10/05/2015
--
-- Purpose:		Get the assessments sections
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/05/2015	-	Sumana Sangapu	- Task 1601	- Initial Creation
-- 01/15/2016	-	Rajiv Ranjan	- Task 5307	- Refactor proc, allowed null into @AssessmentID for getting all data
-- 06/17/2016	-	Kyle Campbell	Modified to sort by SortOrder
-- 06/26/2016	-	Gurpreet Singh	Added SortOrder to output
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetAssessmentSections]
	@AssessmentID BIGINT = NULL,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT 

AS
BEGIN

		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully'
		
		BEGIN TRY

				SELECT  
					a.AssessmentID,
					s.AssessmentSectionID,
					s.SectionName,
					s.PermissionKey,
					s.SortOrder
				FROM	
					[Core].[Assessments] a 
					LEFT JOIN [Core].[AssessmentSections] s
					on a.AssessmentID = s.AssessmentID
				WHERE	
					(ISNULL(@AssessmentID,0) = 0 OR a.AssessmentID = @AssessmentID )
					AND s.IsActive = 1 
				ORDER BY SortOrder

		END TRY
		BEGIN CATCH
				SELECT  @ResultCode = ERROR_SEVERITY(),
						@ResultMessage = ERROR_MESSAGE()
		END CATCH
END