-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_DeleteCallCenter]
-- Author:		Arun Choudhary
-- Date:		02/08/2016
--
-- Purpose:		Delete Call Center Header information 
--
-- Notes:		
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/08/2016	Arun Choudhary	Initial Creation
-- 02/17/2016	Kyle Campbell	Added audit proc calls
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [CallCenter].[usp_DeleteCallCenter]
	@CallCenterHeaderID	BIGINT,
 	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);
				
	SELECT @ResultCode = 0,
			@ResultMessage = 'executed successfully'

	BEGIN TRY
			DECLARE @ContactID BIGINT;

			SELECT @ContactID = ContactID FROM CallCenter.CallCenterHeader WHERE CallCenterHeaderID = @CallCenterHeaderID;
		
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'CallCenter', 'CallCenterHeader', @CallCenterHeaderID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE 
			[CallCenter].[CallCenterHeader]
		SET 
			IsActive = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE 
			CallCenterHeaderID = @CallCenterHeaderID;

		EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'CallCenter', 'CallCenterHeader', @AuditDetailID, @CallCenterHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	END TRY
	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH

END