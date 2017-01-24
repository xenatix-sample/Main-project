-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateContactBenefit]
-- Author:		Avikal Gupta
-- Date:		08/28/2015
--
-- Purpose:		To remove association between payor and contact
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/28/2015	Avikal Task	Initial creation
-- 9/23/2015    John Crossen                Add Audit
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn field
-- 12/15/2016	Scott Martin	Updated auditing
---------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_DeleteContactBenefit]
	@ContactPayorID INT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ContactID BIGINT;

	SET NOCOUNT ON;
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	SELECT @ContactID = ContactID FROM Registration.ContactPayor WHERE ContactPayorID = @ContactPayorID;

	BEGIN TRY
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'ContactPayor', @ContactPayorID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Registration].[ContactPayor]
	SET [IsActive] = 0,
		[ModifiedBy] = @ModifiedBy,
		[ModifiedOn] = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		[ContactPayorID] = @ContactPayorID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'ContactPayor', @AuditDetailID, @ContactPayorID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 			

	END TRY

	BEGIN CATCH
	SELECT @ResultCode = ERROR_SEVERITY(),
			@ResultMessage = ERROR_MESSAGE()
	END CATCH
END

