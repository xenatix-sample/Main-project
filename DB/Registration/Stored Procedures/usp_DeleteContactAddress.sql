-----------------------------------------------------------------------------------------------------------------------
-- Procedure: [usp_DeleteContactAddress]
-- Author:  Arun Choudhary
-- Date:  10/08/2015
--
-- Purpose:  To remove address
--
-- Notes:  N/A (or any additional notes)
--
-- Depends:  N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/07/2015   Created by Arun Choudhary
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn field
-- 12/15/2016	Scott Martin	Updated auditing
---------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_DeleteContactAddress]
	 @ContactAddressID INT,
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
	DECLARE @IsPrimary BIT = 0,
			@NewContactAddressID BIGINT,
			@ContactID BIGINT;

	SELECT 
		@IsPrimary = IsPrimary,
		@ContactID = CA.ContactID
	FROM 
		Registration.ContactAddress CA
	WHERE
		CA.ContactAddressID = @ContactAddressID;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'ContactAddress', @ContactAddressID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Registration].[ContactAddress]
	SET [IsPrimary] = 0,
		[IsActive] = 0,
		[ModifiedBy] = @ModifiedBy,
		[ModifiedOn] = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		[ContactAddressID] = @ContactAddressID;
  
	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'ContactAddress', @AuditDetailID, @ContactAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 

	SELECT TOP 1
		@NewContactAddressID = CA.ContactAddressID 
	FROM 
		Registration.ContactAddress CA 
	WHERE
		CA.IsActive = 1
		AND CA.ContactAddressID <> @ContactAddressID
		AND CA.ContactID = @ContactID
	ORDER BY
		ContactAddressID DESC;
			
IF (@IsPrimary = 1 AND ISNULL(@NewContactAddressID, 0) > 0 )-- if de-activating primary address then make other one primary
	BEGIN
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'ContactAddress', @NewContactAddressID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE 
		CA
		SET IsPrimary=1,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
	FROM
		Registration.ContactAddress CA 
	WHERE 
		CA.ContactAddressID = @NewContactAddressID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'ContactAddress', @AuditDetailID, @NewContactAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 
	END

	END TRY

	BEGIN CATCH
	SELECT @ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END