-----------------------------------------------------------------------------------------------------------------------
-- Procedure:  usp_SaveUserProfile
-- Author:		Justin Spalti
-- Date:		09/28/2015
--
-- Purpose:		Gets all user profile related data
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/28/2015 - Initial proedure creation for saving on the User Profile form
-- 10/02/2015 - Justin Spalti - Added logic to verify the current password and then to update the user's password if needed
-- 01/14/2016	Scott Martin		Added SystemModifiedOn field
-- 03/01/2016 - Justin Spalti - Removed the name parameters as they cannot be updated on the my profile screen 
-- 06/07/2016	Scott Martin		Added code for the minimum number of unique passwords a user can have before reusing one
-- 06/10/2016 - Vamshi Kammari		Added code for the minimum a day to change the password.
-- 07/06/2016 - Rahul Vats			Added check for AD Users
-- 07/07/2016 - Rahul Vats  		Update IsTemporary for AD Users Formatted Using http://www.sql-format.com/  
-- 07/20/2016 - Rahul Vats			Check for AD User First
-- 07/21/2016	RAV - Reviewed The Query http://sqlmag.com/sql-server-2000/designing-performance-null-or-not-null
-----------------------------------------------------------------------------------------------------------------------
Create PROCEDURE [Core].[usp_SaveUserProfile] 
	@UserID int,
	@UpdatePassword bit,
	@NewPassword nvarchar(15),
	@CurrentPassword nvarchar(15),
	@ModifiedOn datetime,
	@ModifiedBy int,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS
BEGIN
	SELECT
		@ResultCode = 0,
		@ResultMessage = 'Executed successfully'

	-- To-Do: Add auditing calls

	BEGIN TRY
		DECLARE @PasswordLogID bigint,
			@MinUniquePassword int = 12,
			@ADUser bit = 0;

	--We need to make sure that ADUser do not get validated for the password checks because they get authenticated against AD
	SELECT
		@ADUser = ADFlag
	FROM Core.Users
	WHERE UserID = @UserID
	
	IF @ADUser = 1 -- Reversing the If Else Logic - Check for AD User First. It still has better performance because we are checking condition or moving out.
	BEGIN
		-- AD User needs to have the IsTemporaryPassword Flag set to false so that they don't get directed to the User Profile on each login
		UPDATE Core.Users
		SET IsTemporaryPassword = 0,
			--When they save their application profile we should keep a track
			ModifiedBy = @ModifiedBy, 
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE UserID = @UserID
    END
	ELSE
	BEGIN
		--If Password needs to be updated then only we should do these calculations
		IF @UpdatePassword = 1
		BEGIN
			
			--If the password doesn't match the current password
			IF NOT EXISTS (	SELECT 1 FROM Core.Users u WHERE u.UserID = @UserID AND ( PWDCOMPARE(@CurrentPassword, u.[Password]) = 1) )
			BEGIN
				RAISERROR ('The current password could not be verified.', 16, 1)
			END
			
			--Password should not be updated more than once in 24 hours
			IF ( ( SELECT COUNT(*) FROM Core.PasswordLog WHERE UserID = @UserID ) > 1) --AND @UpdatePassword = 1 This was a redundant check
			BEGIN
				IF ( ( SELECT DATEDIFF(hh, MAX(AuditTimestamp), GETUTCDATE() ) FROM Core.PasswordLog WHERE UserID = @UserID ) < 24 ) --AND @UpdatePassword = 1
				BEGIN
					RAISERROR (
						'Password cannot be updated with in 24 hours ', -- Message text.
						16, -- Severity.
						1 -- State.
					);
				END
			END

			--Find out from Settings What is the minimum number of previous password that you cannnot keep
			SELECT 
				@MinUniquePassword = SV.Value 
			FROM Core.SettingValues SV
				INNER JOIN Core.Settings S ON SV.SettingsID = S.SettingsID
			WHERE S.Settings = 'MinUniquePassword';
			--Select the password log which matches current password. It should be the latest in the password log
			SELECT 
				@PasswordLogID = MAX(PasswordLogID)
			FROM Core.PasswordLog
			WHERE UserID = @UserID
				AND PWDCOMPARE(@NewPassword, [Password]) = 1;

			-- As per your settings you cannot change your password from amonst your previous @MinUniquePassword 
			IF ( ( SELECT COUNT(*) FROM Core.PasswordLog WHERE UserID = @UserID AND PasswordLogID >= @PasswordLogID ) <= @MinUniquePassword ) AND @PasswordLogID IS NOT NULL
			BEGIN
				RAISERROR (
					'You cannot reuse that password yet. Please choose a different password.', -- Message text.
					16, -- Severity.
					1 -- State.
				);
			END
			
			-- Change The IsTemporary Password for Temporary User. This can be better organized.
			UPDATE Core.Users 
			SET IsTemporaryPassword = 0,
			--Password should only be saved when needed
				[Password] = Core.fn_GenerateHash(@NewPassword),
				ModifiedBy = @ModifiedBy,
				ModifiedOn = @ModifiedOn,
				SystemModifiedOn = GETUTCDATE()
			WHERE UserID = @UserID
		END
		--Every time user save the User Profile This needs to be updated
		UPDATE Core.Users 
		SET 
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE UserID = @UserID
	END
	END TRY
	BEGIN CATCH
		SELECT
			@ResultCode = ERROR_SEVERITY(),
			@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO