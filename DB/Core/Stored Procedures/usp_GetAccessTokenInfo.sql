-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_GetAccessTokenInfo
-- Author:		Justin Spalti
-- Date:		07/23/2015
--
-- Purpose:		Gets the access token information based on thre token provided
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015 - Created the procedure header
-- 07/23/2015 - Added dbo to the table name
-- 07/30/2015   John Crossen  - Change schema from dbo to Core		
-- 08/03/2015 - John Crossen  - Change from dbo to Core schema
-- 08/03/2015 - Justin Spalti - Expanded the size of the Token parameter to match the column size
-- 10/05/2015 - Justin Spalti - Corrected the logic involving the EffectiveToDate so that a newly created user would be allowed to login to the system
-- 09/21/2016 - Deepak Kumar  - Updated NoLock 	
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetAccessTokenInfo]
@UserName NVARCHAR(100),
@Token VARCHAR(1000),
@ClientIP VARCHAR(15),
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'Data load executed successfully'

	BEGIN TRY	
		SELECT u.UserID, u.UserGUID, u.UserName, u.FirstName, u.LastName, u.[Password], 
			   u.EffectiveToDate, u.LoginAttempts, u.LoginCount, u.LastLogin, u.ModifiedOn, u.ModifiedBy, u.IsActive
		FROM Core.AccessToken a with(NoLock)
		JOIN Core.[Users] u
			ON u.UserID = a.UserId
		WHERE u.UserName = @UserName
			AND a.Token = @Token
			AND a.ClientIP = @ClientIP
			AND a.IsActive = 1
			AND u.IsActive = 1
			AND ISNULL(u.EffectiveToDate, DATEADD(MINUTE, 1, GETUTCDATE())) >= GETUTCDATE()			
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END