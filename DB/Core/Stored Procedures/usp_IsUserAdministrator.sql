-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_IsUserAdministrator
-- Author:		Rahul Vats
-- Date:		08/17/2016
--
-- Purpose:		To Find If User Is Administrator
--
-- Notes:		The procedure will return true or false depending on if a user has the admine role
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/17/2016 - RAV -- Created the Procedure
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_IsUserAdministrator]
@UserID INT,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'User roles loaded successfully'

	BEGIN TRY	
		SELECT CASE WHEN EXISTS (
			SELECT u.UserID FROM Core.Users u 
				JOIN Core.UserRole ur ON ur.UserID = u.UserID AND u.UserID = @UserID
				JOIN Core.Role r on r.RoleID = ur.RoleID
			where
				r.Name = 'Administrator'
		)
		THEN CAST (1 AS BIT)
		ELSE CAST (0 AS BIT) END AS IsAdmin
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
