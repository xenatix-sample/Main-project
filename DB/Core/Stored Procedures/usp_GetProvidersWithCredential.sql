-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetProvidersWithCredential]
-- Author:		Satish Singh
-- Date:		01/13/2016
--
-- Purpose:		Gets the list of Users based on credential 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/13/2016	Satish Singh	TFS# 5163 - Initial creation.
-- 03/04/2016   Lokesh Singhal  TFS# 7025 - Filter out disabled user.
-----------------------------------------------------------------------------------------------------------------------

Create PROCEDURE [Core].[usp_GetProvidersWithCredential]
@CredentialID BIGINT=NULL,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	

		SELECT CAST(u.UserID as BIGINT) as ProviderId, u.FirstName+' '+u.LastName as ProviderName, CAST(uc.CredentialID as BIGINT) as CredentialId
		FROM Core.[Users] u
	    JOIN Core.UserCredential uc ON uc.UserID = u.UserID
		JOIN Reference.[Credentials] c ON c.CredentialID = uc.CredentialID
 		WHERE (ISNULL(@CredentialID,0) = 0 OR c.CredentialID= @CredentialID ) 
		AND uc.IsActive = 1
		AND c.IsActive = 1
		AND u.IsActive = 1

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END