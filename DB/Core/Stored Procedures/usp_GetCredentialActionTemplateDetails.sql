
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetCredentialActionTemplateDetails]
-- Author:		Sumana Sangapu
-- Date:		04/14/2016
--
-- Purpose:		Gets users Credential Action Template Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/14/2016	Sumana Sangapu		Initial creation.
-- 06/24/2016	Scott Martin		Added ServiceID to associate credentials with specific services
-- 08/30/2016	Gurpreet Singh		Added LicenseIssueDate, LicenseExpirationDate validation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetCredentialActionTemplateDetails]
	@UserID INT,	
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		DECLARE @minDate DATETIME
		SELECT @minDate = cast(-53690 AS DATETIME)
		
		SELECT	DISTINCT cr.CredentialID , cr.CredentialName, cr.CredentialAbbreviation, ca.CredentialActionID, ca.CredentialAction, af.CredentialActionFormID , af.CredentialActionForm, c.ServicesID,
				u.LicenseIssueDate, u.LicenseExpirationDate
		FROM	[Core].[CredentialActionTemplate] c
		INNER JOIN Core.UserCredential u
		ON		c.CredentialID= u.CredentialID 
		INNER JOIN Reference.[Credentials] cr
		ON		u.CredentialID = cr.CredentialID
		INNER JOIN Reference.CredentialAction ca
		ON		c.CredentialActionID = ca.CredentialActionID 
		INNER JOIN Reference.CredentialActionForms af
		ON		c.CredentialActionFormID = af.CredentialActionFormID
		WHERE	UserID = @UserID
		AND		u.IsActive = 1 
		AND (ISNULL(u.LicenseIssueDate,@minDate) = @minDate OR u.LicenseIssueDate <= convert(date, getdate()))
		AND (ISNULL(u.LicenseExpirationDate,@minDate) = @minDate OR  u.LicenseExpirationDate >= convert(date, getdate()))

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO
