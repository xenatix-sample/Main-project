-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_AddClinicalAssessments]
-- Author:		Scott Martin
-- Date:		11/16/2015
--
-- Purpose:		Add Clinnical Assessments
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/16/2015	Scott Martin	Initial creation.
-- 11/18/2015	Scott Martin	TFS 3610	Add audit logging
-- 11/20/2015	Arun Choudhary  Changed AssessmetDate data type to datetime
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Clinical].[usp_AddClinicalAssessments]
	@ContactID BIGINT,
	@AssessmentDate DATETIME,
	@UserID INT,
	@AssessmentID BIGINT,
	@ResponseID BIGINT,
	@AssessmentStatusID SMALLINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'Executed successfully';

	INSERT INTO [Clinical].[ClinicalAssessments]
	(
		ContactID,
		AssessmentDate,
		UserID,
		AssessmentID,
		ResponseID,
		AssessmentStatusID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		[CreatedBy],
		[CreatedOn]
	)
	VALUES
	(
		@ContactID,
		@AssessmentDate,
		@UserID,
		@AssessmentID,
		@ResponseID,
		@AssessmentStatusID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);
		
	SELECT @ID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Clinical', 'ClinicalAssessments', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Clinical', 'ClinicalAssessments', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE();
	END CATCH
END
GO


