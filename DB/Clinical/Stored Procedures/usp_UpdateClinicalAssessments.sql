-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_AddClinicalAssessment]
-- Author:		Scott Martin
-- Date:		11/16/2015
--
-- Purpose:		Update Clinical Assessment
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/16/2015	Scott Martin	Initial creation.
-- 11/18/2015	Scott Martin	Added audit logging
-- 11/20/2015	Arun Choudhary  Changed AssessmetDate data type to datetime
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn to Update statement
-- 02/17/2016	Scott Martin	Refactored audit logging
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Clinical].[usp_UpdateClinicalAssessments]
	@ClinicalAssessmentID BIGINT,
	@ContactID BIGINT,
	@AssessmentDate DATETIME,
	@UserID INT,
	@AssessmentID BIGINT,
	@ResponseID BIGINT,
	@AssessmentStatusID SMALLINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Clinical', 'ClinicalAssessments', @ClinicalAssessmentID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Clinical.ClinicalAssessments
	SET ContactID = @ContactID,
		AssessmentDate = @AssessmentDate,
		UserID = @UserID,
		AssessmentID = @AssessmentID,
		ResponseID = @ResponseID,
		AssessmentStatusID = @AssessmentStatusID,
		ModifiedBy = @ModifiedBy,
		ModifiedOn  = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()	   
	WHERE
		ClinicalAssessmentID = @ClinicalAssessmentID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Clinical', 'ClinicalAssessments', @AuditDetailID, @ClinicalAssessmentID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	  
 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO