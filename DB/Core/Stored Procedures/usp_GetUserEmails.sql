-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetUserEmails]
-- Author:		Justin Spalti
-- Date:		09/30/2015
--
-- Purpose:		Get User Email Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/30/2015	Justin Spalti - Initial procedure creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetUserEmails]
	@UserID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'Executed successfully'

	BEGIN TRY
		SELECT u.UserID,
			   ue.UserEmailID, ue.EmailPermissionID, ue.IsPrimary,
			   e.EmailID, e.Email
		FROM Core.Users u
		INNER JOIN Core.UserEmail ue
			ON ue.UserID = u.UserID
		INNER JOIN Core.Email e
			ON e.EmailID = ue.EmailID
		WHERE u.UserID = @UserID
			AND u.IsActive = 1
			AND ue.IsActive = 1
			AND e.IsActive = 1		
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END