-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_DeleteContactForms]
-- Author:		Scott Martin
-- Date:		06/10/2016
--
-- Purpose:		Delete ContactForms
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/10/2016	Scott Martin	Initial creation.
-- 12/15/2016	Scott Martin	Updated auditing
-- 01/19/2017	Scott Martin	Added code to inactivate related service
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_DeleteContactForms]
	@ContactFormsID BIGINT,
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

	SELECT @ContactID = ContactID FROM Registration.ContactForms WHERE ContactFormsID = @ContactFormsID;

	SELECT @ServiceRecordingID = ServiceRecordingID FROM Registration.vw_GetContactFormsServiceRecordingDetails WHERE SourceHeaderID = @ContactFormsID;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'ContactForms', @ContactFormsID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Registration].[ContactForms]
	SET	IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ContactFormsID = @ContactFormsID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'ContactForms', @AuditDetailID, @ContactFormsID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
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