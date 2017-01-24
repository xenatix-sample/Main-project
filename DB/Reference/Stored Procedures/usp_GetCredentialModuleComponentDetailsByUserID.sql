-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetCredentialModuleComponentDetails]
-- Author:		Scott Martin
-- Date:		08/10/2016
--
-- Purpose:		Gets the list of Credential Module Component lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/10/2016	Scott Martin	- Initial creation.
-- 09/08/2016 - Arun Choudhary - Added LicenseExpirationDate to the result set
-- 09/08/2016 - Vishal Yadav - ModuleComponentID added 
-- 09/20/2016 - Gaurav Gupta - Added LicenseIssueDate
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetCredentialModuleComponentDetailsByUserID]
	 @UserID INT
	,@ModuleComponentID BIGINT
	,@ResultCode INT OUTPUT
	,@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0
		,@ResultMessage = 'executed successfully'

	BEGIN TRY
		SELECT uc.UserCredentialID
			,u.UserID
			,c.CredentialID
			,c.CredentialAbbreviation
			,c.CredentialName
			,uc.LicenseExpirationDate
			,uc.LicenseIssueDate
			,uc.IsActive
			,cmc.ServicesID
		FROM Core.[Users] u
		INNER JOIN Core.UserCredential uc ON uc.UserID = u.UserID
		INNER JOIN Reference.[Credentials] c ON c.CredentialID = uc.CredentialID
		LEFT JOIN Reference.CredentialModuleComponent cmc ON uc.CredentialID = cmc.CredentialID
		WHERE u.UserID = @UserID
			AND cmc.ModuleComponentID=@ModuleComponentID
			AND u.IsActive = 1
			AND uc.IsActive = 1
			AND c.IsActive = 1
			AND cmc.IsActive = 1
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY()
			,@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
