-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetAssessmentNamesByGroup]
-- Author:		Scott Martin
-- Date:		06/08/2016
--
-- Purpose:		Gets the list of Assessment Names By grouping
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/08/2016	Scott Martin	Initial creation.
-- 08/29/2016	Rahul Vats		Fix the error for Unresolved Reference.
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Core].[usp_GetAssessmentNamesByGroup]
	@AssessmentGroupID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage =  'Executed successfully'

	BEGIN TRY	

	SELECT  
		A.AssessmentID,
		A.Name as AssessmentName, 
		DM.ScreeningTypeID,
		AG.AssessmentGroupID,
		AG.AssessmentGroup
	FROM	
		[Core].[Assessments] A 
		LEFT OUTER JOIN [Core].[DocumentMapping] DM
			ON A.AssessmentID = DM.AssessmentID
		LEFT OUTER JOIN Core.AssessmentGroupDetails AGD			
			ON A.AssessmentID = AGD.AssessmentID
		LEFT OUTER JOIN Core.AssessmentGroup AG
			ON AGD.AssessmentGroupID = AG.AssessmentGroupID
	WHERE
		AG.AssessmentGroupID = @AssessmentGroupID

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END