-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_AddServiceRecordingAdditionalUser]
-- Author:		Scott Martin
-- Date:		03/23/2016
--
-- Purpose:		Add Service Recording Additional Users
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/23/2016	Scott Martin		Initial creation
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddServiceRecordingAdditionalUser]
	@ServiceRecordingID bigint,
	@UserID int,
	@ModifiedOn datetime,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ContactID BIGINT;

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	SELECT @ContactID = ContactID FROM Core.vw_GetServiceRecordingDetails SRD WHERE SRD.ServiceRecordingID = @ServiceRecordingID;

	INSERT INTO  Core.ServiceRecordingAdditionalUser
	(
		ServiceRecordingID,
		UserID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@ServiceRecordingID,
		@UserID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);
		
	SELECT @ID = SCOPE_IDENTITY();

	DECLARE @AuditDetailID BIGINT;
			
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'ServiceRecordingAdditionalUser', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'ServiceRecordingAdditionalUser', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE();
	END CATCH
END