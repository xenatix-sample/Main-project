-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_AddReviewOfSystem]
-- Author:		Scott Martin
-- Date:		11/16/2015
--
-- Purpose:		Add Review of System data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/16/2015	Scott Martin	Initial creation.
-- 11/18/2015	Rajiv Ranjan	Removed ID parms.
-- 11/17/2015	Scott Martin	TFS 3610	Add audit logging. Re-added ID for audit logging
-- 11/19/2015	Rajiv Ranjan	Changed @DateEntered datatype to DATETIME
-- 11/21/2015	Rajiv Ranjan	Added IsReviewChanged
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Clinical].[usp_AddReviewOfSystem]
	@ContactID BIGINT,
	@DateEntered DATETIME,
	@ReviewdBy INT,
	@AssessmentID BIGINT,
	@ResponseID BIGINT,
	@IsReviewChanged BIT,
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
		   @ResultMessage = 'executed successfully';

	INSERT INTO  [Clinical].[ReviewOfSystems]
	(
		ContactID,
		DateEntered,
		ReviewdBy,
		AssessmentID,
		ResponseID,
		IsReviewChanged,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		[CreatedBy],
		[CreatedOn]
	)
	VALUES
	(
		@ContactID,
		@DateEntered,
		@ReviewdBy,
		@AssessmentID,
		@ResponseID,
		@IsReviewChanged,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);
		
	SELECT @ID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Clinical', 'ReviewOfSystems', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Clinical', 'ReviewOfSystems', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;			

 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE();
	END CATCH
END
GO