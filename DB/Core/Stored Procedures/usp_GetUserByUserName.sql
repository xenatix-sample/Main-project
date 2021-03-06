-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_GetUserByUserName
-- Author:		Justin Spalti
-- Date:		07/23/2015
--
-- Purpose:		Gets the user model using the provided user name
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015 - Created the procedure header
-- 07/23/2015 - Added dbo to the table name
-- 07/30/2015   John Crossen     Change schema from dbo to Core		
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-- 06/09/2016 - Sumana Sangapu - Ensure the user is Active - TFS #11254
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetUserByUserName]
@UserName VARCHAR(100),
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'Data retrieved successfully'

	BEGIN TRY	
		SELECT u.UserID, u.UserGUID, u.UserName, u.FirstName, u.LastName, u.[Password], 
			   u.EffectiveToDate, u.LoginAttempts, u.LoginCount, u.LastLogin, u.ModifiedOn, u.ModifiedBy, u.IsActive
		FROM Core.[Users] u
		WHERE u.UserName = @UserName
		AND	  u.IsActive = 1
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
