-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_UpdateReviewOfSystem]
-- Author:		Scott Martin
-- Date:		11/16/2015
--
-- Purpose:		Update Review of System
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/16/2015	Scott Martin	Initial creation.
-- 11/18/2015	Rajiv Ranjan	Removed ID parms.
-- 11/18/2015	Scott Martin	TFS 3610	Add audit logging. Re-Added ID for audit logging
-- 11/19/2015	Rajiv Ranjan	Changed @DateEntered datatype to DATETIME
-- 11/21/2015	Rajiv Ranjan	Added IsReviewChanged
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn to Update statement
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_UpdateReviewOfSystem]
	@RoSID BIGINT,
	@ContactID BIGINT,
	@DateEntered DATETIME,
	@ReviewdBy INT,
	@AssessmentID BIGINT,
	@ResponseID BIGINT,
	@IsReviewChanged BIT,
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

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Clinical', 'ReviewOfSystems', @RoSID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Clinical].[ReviewOfSystems]
	SET ContactID = @ContactID,
		DateEntered = @DateEntered,
		ReviewdBy = @ReviewdBy,
		AssessmentID = @AssessmentID,
		ResponseID = @ResponseID,
		IsReviewChanged=@IsReviewChanged,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		RoSID = @RoSID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Clinical', 'ReviewOfSystems', @AuditDetailID, @RoSID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO