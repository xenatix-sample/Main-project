
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_VerifyUserCredential]
-- Author:		Sumana Sangapu
-- Date:		04/09/2016
--
-- Purpose:		Verify whether the user has the credentials required to perform an action
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/09/2016	Sumana Sangapu	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_VerifyUserCredential]
	@UserID INT,
	@CredentialActionFormID int,
	@CredentialActionID  int,
	@CredentialID int,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY		

		SELECT DISTINCT CAST(1 AS BIT) Result 
		FROM	[Core].[CredentialActionTemplate] c
		INNER JOIN Core.UserCredential u
		ON		c.CredentialID= u.CredentialID 
		WHERE	CredentialActionFormID =  @CredentialActionFormID
		AND		CredentialActionID = @CredentialActionID
		AND		c.CredentialID = @CredentialID
		AND		UserID = @UserID
		AND		c.IsActive = 1 
		AND		u.IsActive = 1 
	
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END