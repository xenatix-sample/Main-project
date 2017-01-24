-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_DeleteContactEmail]
-- Author:		Satish Kumar Singh
-- Date:		10/07/2015
--
-- Purpose:		To remove email
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/07/2015   Created by Satish Singh
-- 10/16/2015   Row affected required for Test cases. commented SET NOCOUNT ON
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn field
-- 12/15/2016	Scott Martin	Updated auditing
---------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_DeleteContactEmail]
	@ContactEmailID INT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@IsPrimary BIT = 0,
		@NewContactEmailID BIGINT,
		@ContactID BIGINT,
		@AuditDetailID BIGINT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
	SELECT
		@IsPrimary = IsPrimary,
		@ContactID = CE.ContactID
	FROM
		Registration.ContactEmail CE
	WHERE
		CE.ContactEmailID = @ContactEmailID;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'ContactEmail', @ContactEmailID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Registration].[ContactEmail]
	SET [IsPrimary] = 0,
		[IsActive] = 0,
		[ModifiedBy] = @ModifiedBy,
		[ModifiedOn] = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE [ContactEmailID] = @ContactEmailID

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'ContactEmail', @AuditDetailID, @ContactEmailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 
	
	SELECT TOP 1
		@NewContactEmailID = CE.ContactEmailID 
	FROM 
		Registration.ContactEmail CE 
	WHERE
		CE.IsActive = 1
		AND CE.ContactEmailID <> @ContactEmailID
		AND CE.ContactID = @ContactID
	ORDER BY
		ContactEmailID DESC;
			
	IF (@IsPrimary = 1 AND ISNULL(@NewContactEmailID, 0) > 0)-- if we are deactivating primary address then make other one primary
	BEGIN
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'ContactEmail', @NewContactEmailID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE CE
	SET IsPrimary = 1,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM
		Registration.ContactEmail CE 
	WHERE 
		CE.ContactEmailID = @NewContactEmailID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'ContactEmail', @AuditDetailID, @NewContactEmailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 
	END
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
