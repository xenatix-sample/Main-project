-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetClinicalAssessments]
-- Author:		Scott Martin
-- Date:		11/16/2015
--
-- Purpose:		Get Clinical Assessments
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/16/2015	Scott Martin	Initial creation.
-- 11/20/2015  Arun Choudhary   Added Alias for few columns
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Clinical].[usp_GetClinicalAssessments]
	@ClinicalAssessmentID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		ClinicalAssessmentID,
		ContactID,
		AssessmentDate As TakenTime,
		UserID AS TakenBy,
		AssessmentID,
		ResponseID ,
		AssessmentStatusID,
		IsActive,
		ModifiedBy,
		ModifiedOn
	FROM
		Clinical.ClinicalAssessments
	WHERE
		IsActive = 1
		AND ClinicalAssessmentID = @ClinicalAssessmentID
	  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO