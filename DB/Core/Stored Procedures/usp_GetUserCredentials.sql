-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Core.[usp_GetUserCredentials]
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
-- 08/12/2015	John Crossen TFS#1182 		Modify to pull per user.
-- 03/29/2016 - Justin Spalti - Added StateIssuedByID to the results set
-- 06/09/2016 - Sumana Sangapu - Ensure the user is Active - TFS #11254
-- 09/30/2016 - Rajiv Ranjan - Can get credentials for an inactive user as well. - TFS #15334
-------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetUserCredentials]
@UserID INT,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	

		SELECT uc.UserCredentialID, u.UserID, c.CredentialID, c.CredentialAbbreviation, c.CredentialName, uc.StateIssuedByID, 
			   uc.LicenseNbr, uc.LicenseIssueDate, uc.LicenseExpirationDate, uc.IsActive
		FROM Core.[Users] u
	    JOIN Core.UserCredential uc ON uc.UserID = u.UserID
		JOIN Reference.[Credentials] c ON c.CredentialID = uc.CredentialID
 		WHERE u.UserID = @UserID
			AND uc.IsActive = 1
			AND c.IsActive = 1

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
