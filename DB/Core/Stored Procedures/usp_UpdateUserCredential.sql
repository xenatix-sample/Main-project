-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Core.[usp_UpdateUserCredentials]
-- Author:		John Crossen
-- Date:		08/12/2015
--
-- Purpose:		Select Credential details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/12/2015	John Crossen TFS#1182 		Initial creation.
-- 9/23/2015    John Crossen                Add Audit
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
-- 03/23/2016	Sumana Sangapu		Added StateProvinceID for Issued by State
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateUserCredential]
	@UserCredentialID BIGINT,
	@UserID INT,
	@CredentialID BIGINT,
	@LicenseNbr NVARCHAR(100) = NULL,
	@LicenseIssueDate DATE = NULL,
	@LicenseExpirationDate DATE = NULL,
	@StateProvinceID INT = NULL,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY	

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'UserCredential', @UserCredentialID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Core.[UserCredential]
	SET UserID = @UserID,
		CredentialID = @CredentialID,
		LicenseNbr = @LicenseNbr,
		LicenseIssueDate = @LicenseIssueDate,
		LicenseExpirationDate = @LicenseExpirationDate,
		StateIssuedByID	= @StateProvinceID,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE 
		UserCredentialID = @UserCredentialID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'UserCredential', @AuditDetailID, @UserCredentialID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END