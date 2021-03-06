-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Core.[usp_GetCredentialPermissions]
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
-------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetCredentialPermissions]
	@CredentialID NVARCHAR(100),
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		
		IF OBJECT_ID('tempdb..#tmpCredentialPermissions') IS NOT NULL
			DROP TABLE #tmpCredentialPermissions
		
		
		CREATE TABLE #tmpCredentialPermissions(tmpID INT PRIMARY KEY IDENTITY (1,1), CredentialID BIGINT, PermissionID BIGINT,PermissionName NVARCHAR(250), 
		[PermissionDescription] NVARCHAR(1000),	CredentialName NVARCHAR(20), CredentialDescription NVARCHAR(255))
	

		INSERT INTO #tmpCredentialPermissions(CredentialID, PermissionID, PermissionName,PermissionDescription, CredentialName, CredentialDescription)
		SELECT CP.CredentialID, CP.PermissionID, P.Name, P.[Description], CR.CredentialAbbreviation, CR.CredentialName 
		FROM Core.Permission P JOIN Core.CredentialPermission CP ON CP.PermissionID = P.PermissionID
		JOIN Reference.[Credentials] CR ON CR.CredentialID = CP.CredentialID
		WHERE CR.CredentialID = @CredentialID
			AND CR.IsActive = 1
			AND P.IsActive = 1



		INSERT INTO #tmpCredentialPermissions(CredentialID, PermissionID, PermissionName,PermissionDescription, CredentialName, CredentialDescription) 
		SELECT 0, 0, NULL, NULL, r.CredentialAbbreviation,  r.CredentialName
		FROM  Reference.[Credentials] r
		LEFT JOIN #tmpCredentialPermissions t
			ON t.CredentialID = r.CredentialID
		WHERE t.CredentialID IS NULL
			AND r.IsActive = 1

		SELECT  CredentialID, PermissionID, PermissionName,PermissionDescription, CredentialName, CredentialDescription
		FROM #tmpCredentialPermissions 
		ORDER BY CASE WHEN CredentialID = 0 THEN 1 ELSE 0 END, CredentialDescription

		DROP TABLE #tmpCredentialPermissions	
	
	
	
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END