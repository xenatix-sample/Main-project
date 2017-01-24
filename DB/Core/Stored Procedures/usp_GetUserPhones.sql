-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetUserPhones]
-- Author:		Justin Spalti
-- Date:		09/30/2015
--
-- Purpose:		Get the phone Numbers associated with a user
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/30/2015	Justin Spalti - Initial procedure creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetUserPhones]
	@UserID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'Executed successfully'
	
	BEGIN TRY
		SELECT u.UserID, 
			   up.UserPhoneID, up.IsPrimary, 
			   p.PhoneID, p.PhoneTypeID, p.Number, p.Extension,
			   pm.PhonePermissionID,
			   p.IsActive, p.ModifiedBy, p.ModifiedOn
		FROM Core.Users u
		INNER JOIN Core.UserPhone up
			ON up.UserID = u.UserID
		LEFT OUTER JOIN Reference.PhonePermission pm
			ON pm.PhonePermissionID = up.PhonePermissionID
		LEFT OUTER JOIN Core.Phone p
			ON p.PhoneID = up.PhoneID
		WHERE u.UserID = @UserID
			AND u.IsActive = 1
			AND up.IsActive = 1
			AND p.IsActive = 1		
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
