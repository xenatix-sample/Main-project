-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[CallCenter].[usp_AddCallCenterAssessmentResponse]
-- Author:		Rajiv Ranjan
-- Date:		06/14/2016
--
-- Purpose:		Add CallCenterAssessmentResponse data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/14/2016	Rajiv Ranjan	Initial creation.
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [CallCenter].[usp_AddCallCenterAssessmentResponse]
	@CallCenterHeaderID BIGINT,
	@AssessmentID BIGINT,
	@ResponseID BIGINT,
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

	DECLARE @ContactID BIGINT;

	SELECT @ContactID = ContactID FROM CallCenter.CallCenterHeader WHERE CallCenterHeaderID = @CallCenterHeaderID;

	INSERT INTO  [CallCenter].[CallCenterAssessmentResponse]
	(
		CallCenterHeaderID,
		[AssessmentID],
		[ResponseID],
		IsActive,
		ModifiedBy,
		ModifiedOn,
		[CreatedBy],
		[CreatedOn]
	)
	VALUES
	(
		@CallCenterHeaderID,
		@AssessmentID,
		@ResponseID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);
		
	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'CallCenter', 'CallCenterAssessmentResponse', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'CallCenter', 'CallCenterAssessmentResponse', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;			

 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE();
	END CATCH
END