-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_DeleteBenefitsAssistance]
-- Author:		Scott Martin
-- Date:		05/19/2016
--
-- Purpose:		Delete Benefits Assistance
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/19/2016	Scott Martin	Initial creation.
-- 12/15/2016	Scott Martin	Updated auditing
-- 01/19/2017	Scott Martin	Added code to inactivate related service
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_DeleteBenefitsAssistance]
	@BenefitsAssistance BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ContactID BIGINT,
		@ServiceRecordingID BIGINT;

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	SELECT @ContactID = ContactID From Registration.BenefitsAssistance WHERE BenefitsAssistanceID = @BenefitsAssistance;

	SELECT @ServiceRecordingID = ServiceRecordingID FROM Registration.vw_GetBenefitsAssistanceServiceRecordingDetails WHERE SourceHeaderID = @BenefitsAssistance;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'BenefitsAssistance', @BenefitsAssistance, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Registration].[BenefitsAssistance]
	SET	IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		BenefitsAssistanceID = @BenefitsAssistance;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'BenefitsAssistance', @AuditDetailID, @BenefitsAssistance, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	IF @ServiceRecordingID IS NOT NULL
		BEGIN
		EXEC Core.usp_DeleteServiceRecording @ServiceRecordingID, @ModifiedBy, @ModifiedOn, @ResultCode OUTPUT, @ResultMessage OUTPUT;
		END 
 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO