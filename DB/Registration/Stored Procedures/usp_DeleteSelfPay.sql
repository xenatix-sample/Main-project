-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_DeleteSelfPay]
-- Author:		Kyle Campbell
-- Date:		03/21/2016
--
-- Purpose:		Get Contact SelfPay Records
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/21/2016	Kyle Campbell	TFS #7798	Initial Creation
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_DeleteSelfPay]
	@SelfPayID BIGINT,
    @ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
			@ContactID BIGINT;

	BEGIN TRY
		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully';

		SELECT @ContactID = ContactID FROM Registration.SelfPay WHERE SelfPayID = @SelfPayID;

		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'SelfPay', @SelfPayID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Registration.SelfPay
		SET	IsActive = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE 
			SelfPayID = @SelfPayID;

		EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'SelfPay', @AuditDetailID, @SelfPayID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	END TRY

	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO