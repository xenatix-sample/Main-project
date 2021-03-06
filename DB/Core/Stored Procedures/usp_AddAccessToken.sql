-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_LogAccessToken
-- Author:		Justin Spalti
-- Date:		07/23/2015
--
-- Purpose:		Saves the user's access token
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015 - Initial procedure creation
-- 07/23/2015 - Added dbo to the table name
-- 07/30/2015 - @UserID,@GeneratedOn and @ExpirationDate added in parameter set and use in select/insert statement
-- 07/30/2015 - John Crossen - Change schema from dbo to Core		
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-- 08/03/2015 - Justin Spalti -- Expanded the size of the Token parameter to match the column size
-- 01/14/2016 - Scott Martin -   Added ModifiedOn parameter, Added CreatedBy and CreatedOn fields
-- 02/25/2016 - Justin Spalti  - Removed the ModifiedBy parameter since it is not known at this point of execution. Renamed procedure.
-- 06/09/2016 - Sumana Sangapu - Ensure the user is Active - TFS #11254
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddAccessToken](
	@UserID INT,
	@UserName VARCHAR(100),
	@Token VARCHAR(1000),
	@ClientIP VARCHAR(15),
	@GeneratedOn DATETIME,
	@ExpirationDate DATETIME,
	@ModifiedOn DATETIME,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
)
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'Data saved successfully'

	BEGIN TRY	
		IF EXISTS
			(
				SELECT u.UserID
				FROM Core.[Users] u
				WHERE u.UserID = @UserID and 
					  u.UserName = @UserName and
					  u.IsActive = 1

			)
		BEGIN
			INSERT INTO Core.AccessToken (UserId, Token, ClientIP, GeneratedOn, ExpirationDate, IsActive, ModifiedOn, ModifiedBy, CreatedBy, CreatedOn)
			SELECT TOP 1 u.UserID, @Token, @ClientIP,@GeneratedOn, @ExpirationDate, 1, @ModifiedOn, @UserID, @UserID, @ModifiedOn
			FROM Core.[Users] u
			WHERE u.UserID = @UserID and
				  u.UserName = @UserName and
				  u.IsActive = 1	
		END
		ELSE 
		BEGIN
			RAISERROR('Token not found for user.', 16, 1)
		END
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END