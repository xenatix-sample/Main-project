-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetClinicalAssessmentsByContact]
-- Author:		Arun Choudhary
-- Date:		11/21/2015
--
-- Purpose:		Get Clinical Assessments By Contact ID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/21/2015  Arun Choudhary   Initial check-in
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Clinical].[usp_GetClinicalAssessmentsByContact]
	@ContactID BIGINT,
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
		ResponseID,
		(select top 1 AssessmentSectionID from Core.AssessmentSections where AssessmentID = ca.AssessmentID and IsActive = 1 order by SortOrder asc) as SectionID,
		AssessmentStatusID,
		IsActive,
		ModifiedBy,
		ModifiedOn
	FROM
		Clinical.ClinicalAssessments ca
	WHERE
		IsActive = 1
		AND ContactID = @ContactID
	  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO