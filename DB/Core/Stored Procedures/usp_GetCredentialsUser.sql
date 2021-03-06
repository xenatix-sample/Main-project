-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetCredentialsUser]
-- Author:		John Crossen
-- Date:		10/16/2015
--
-- Purpose:		Gets the list of Users based on credential 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/16/2015	John Crossen	TFS# 2765 - Initial creation.
-----------------------------------------------------------------------------------------------------------------------

Create PROCEDURE [Core].[usp_GetCredentialsUser]
@CredentialID INT,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	

		SELECT uc.UserCredentialID, u.UserID, u.UserName,u.FirstName,u.LastName
		FROM Core.[Users] u
	    JOIN Core.UserCredential uc ON uc.UserID = u.UserID
		JOIN Reference.[Credentials] c ON c.CredentialID = uc.CredentialID
 		WHERE uc.CredentialID = @CredentialID
			AND uc.IsActive = 1
			AND c.IsActive = 1

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END