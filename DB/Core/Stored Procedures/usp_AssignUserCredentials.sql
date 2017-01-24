-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Core.[usp_AssignUserCredentials]
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
-- 09/22/2015   John Crossen        Add Audit
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedOn and CreatedBy fields
-- 03/23/2016	Sumana Sangapu		Added StateProvinceID for Issued by State
-------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AssignUserCredentials]
	@UserID INT,
	@CredentialID BIGINT,
	@LicenseNbr NVARCHAR(100) NULL,
	@LicenseIssueDate DATE NULL,
	@LicenseExpirationDate DATE NULL,
	@StateProvinceID INT NULL,
	@IsActive BIT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	DECLARE @ID BIGINT

	IF EXISTS(SELECT 1 FROM Core.UserCredential WHERE CredentialID = @CredentialID AND UserID=@UserID AND LicenseIssueDate=@LicenseIssueDate AND [IsActive]=1)
		 BEGIN
			RAISERROR ('Credential Pairing can not be duplicate.', -- Message text.
               16, -- Severity.
               1 -- State.
               );
		 END
		 	
	INSERT INTO Core.UserCredential
	(
		UserID ,
		CredentialID ,
		LicenseNbr ,
		LicenseIssueDate ,
		LicenseExpirationDate ,
		StateIssuedByID,
		IsActive ,
		ModifiedBy ,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@UserID,
		@CredentialID,
		@LicenseNbr,
		@LicenseIssueDate,
		@LicenseExpirationDate,
		@StateProvinceID,
		@IsActive,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn 
	);

    SELECT @ID = SCOPE_IDENTITY();

	DECLARE @AuditDetailID BIGINT;

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'UserCredential', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'UserCredential', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


