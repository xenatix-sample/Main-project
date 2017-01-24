-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Core.[usp_AssignCredentialPermission]
-- Author:		John Crossen
-- Date:		08/12/2015
--
-- Purpose:		Assign Credential Permissions
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/12/2015	John Crossen TFS#1182 		Initial creation.
--------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE usp_AssignCredentialPermission
	@CredentialID BIGINT,
	@PermissionID BIGINT,
	@IsActive BIT,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS

BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY

	 IF EXISTS(SELECT 1 FROM Core.CredentialPermission WHERE CredentialID = @CredentialID AND PermissionID=@PermissionID  AND [IsActive]=1)
		 BEGIN
			RAISERROR ('CredentialPermission Pairing can not be duplicate.', -- Message text.
               16, -- Severity.
               1 -- State.
               );
		 END	


	INSERT INTO Core.CredentialPermission
	        ( CredentialID ,
	          PermissionID ,
	          ModifiedOn ,
	          ModifiedBy ,
	          IsActive
	        )
	VALUES  ( @CredentialID , 
	          @PermissionID ,
			  GETUTCDATE(),
			  @ModifiedBy,
			  @IsActive
	        )
	       
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END