-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_DeleteContactPhone]
-- Author:		Avikal Gupta
-- Date:		9/16/2015
--
-- Purpose:		To remove contact phone
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 9/16/2015	Avikal Task	Initial creation
-- 9/23/2015    John Crossen                Add Audit
-- 10/16/2015   Avikal modification made to set another phone as the primary phone if the one getting deleted is primary
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn field
-- 02/25/2016	Gurpreet Singh	#5716	Removing code that sets next record IsPrimary after delete
---------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_DeleteContactPhone]
	@ContactPhoneID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@IsPrimary BIT=0,
		@NewContactPhoneID BIGINT,
		@ContactID BIGINT;

	SET NOCOUNT ON;
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
		SELECT
			@IsPrimary = IsPrimary,
			@ContactID = CP.ContactID
		FROM
			Registration.ContactPhone CP
		WHERE
			CP.ContactPhoneID = @ContactPhoneID;

		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'ContactPhone', @ContactPhoneID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Registration.ContactPhone
		SET IsPrimary = 0,
			IsActive = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE 
			ContactPhoneID = @ContactPhoneID;

		EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'ContactPhone', @AuditDetailID, @ContactPhoneID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END