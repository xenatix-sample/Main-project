-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_AuthenticateUser
-- Author:		Justin Spalti
-- Date:		07/23/2015
--
-- Purpose:		Adds a user into the User table
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015 - Added the procedure header and added dbo to the table
-- 07/23/2015 - demetrios.christopher@xenatix.com - Added password hashing
-- 07/30/2015   John Crossen     Change schema from dbo to Core		
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-- 09/08/2015 - Justin Spalti - Updated the procedure to only validate against the effectivetodate if it's non-null
-- 01/28/2015 - Lokesh Singhal  Do not allow to login if 3 attempts with wrong password have been made
-- 01/29/2015 - Lokesh Singhal Show appropriate message if 3 attempts with wrong password have been made
-- 06/09/2016	Scott Martin	Added field for days until expiration
-- 06/13/2016 - Lokesh Singhal Show appropriate message if user doesnt have any role assigned
-- 07/06/2016 - Sumana Sangapu Checking for NULL passwords
-- 08/25/2016	Gurpreet Singh	Validation if no active Role is assigned
-- 08/26/2016 - Removing the check for the roles while getting Authenticated for AD User and showing a message
-- 09/18/2016 - Modifying the flow of the Authentication for showing the messages for Unauthorized Attempts
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Core].[usp_AuthenticateUser]
	@UserName VARCHAR(100),
	@Password VARCHAR(100),
	@ADAuthenticated int,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'Data load executed successfully'

	BEGIN TRY
	DECLARE @MaxPasswordAge INT;
	DECLARE @ADFlag Bit;

	--Select @ADFlag = ISNULL(ADFLAG,0) from Core.Users where UserName = @UserName
	SELECT @MaxPasswordAge = Cast(SV.Value as int) FROM Core.SettingValues SV INNER JOIN Core.Settings S ON SV.SettingsID = S.SettingsID WHERE S.Settings = 'MaxPasswordAge'

	DECLARE @minDate DATETIME
	SELECT @minDate = cast(-53690 AS DATETIME)

	if (@ADAuthenticated = 0)
	begin
			IF OBJECT_ID('tempdb..#tmpUserDetail') IS NOT NULL
			DROP TABLE #tmpUserDetail
		SELECT
			u.UserID,
			u.UserGUID,
			u.UserName,
			u.FirstName,
			u.LastName,
			u.[Password], 
			u.EffectiveToDate,
			u.LoginAttempts,
			u.LoginCount,
			u.LastLogin,
			u.ModifiedOn,
			u.ModifiedBy,
			u.IsActive
		INTO #tmpUserDetail
		FROM Core.[Users] u
		INNER JOIN Core.UserRole ur
			ON ur.UserID = u.UserID

		INNER JOIN Core.[Role] r
			ON r.RoleID = ur.RoleID
		WHERE
			u.UserName = @UserName
			AND PWDCOMPARE(@Password, u.[Password]) = 1
			AND u.IsActive = 1
			AND ISNULL(u.EffectiveToDate, DATEADD(MINUTE, 1, GETUTCDATE())) >= GETUTCDATE() 
			AND ISNULL(u.EffectiveFromDate, DATEADD(MINUTE, 1, GETUTCDATE())) <= GETUTCDATE()
			AND r.IsActive = 1
			AND ur.IsActive = 1
			AND	u.LoginAttempts < 3
			AND (ISNULL(r.EffectiveDate,@minDate) = @minDate OR  r.EffectiveDate <= convert(date, getdate())) AND (ISNULL(r.ExpirationDate,@minDate) = @minDate OR  r.ExpirationDate >= convert(date, getdate()))

		IF NOT EXISTS(SELECT 1 FROM #tmpUserDetail)
		BEGIN
			IF EXISTS(SELECT 1 FROM Core.[Users] U WHERE u.UserName = @UserName AND u.IsActive <> 1)
			BEGIN
				SET @ResultCode = -1
			END
			ELSE IF EXISTS(SELECT 1 FROM Core.[Users] U WHERE u.UserName = @UserName AND u.IsActive = 1 AND	u.LoginAttempts > 2)
			BEGIN
				SET @ResultCode = -2
			END
			ELSE IF NOT EXISTS(SELECT 1 FROM Core.[Users] U WHERE u.UserName = @UserName AND u.IsActive = 1 AND	ISNULL(u.EffectiveFromDate, DATEADD(MINUTE, 1, GETUTCDATE())) <= GETUTCDATE())
			BEGIN
				SET @ResultCode = -3
			END
			ELSE IF NOT EXISTS(SELECT 1 FROM Core.[Users] U INNER JOIN Core.UserRole ur ON ur.UserID = u.UserID  INNER JOIN Core.Role r ON r.RoleID = ur.RoleID WHERE u.UserName = @UserName AND u.IsActive = 1 AND ur.IsActive = 1
								AND (ISNULL(r.EffectiveDate,@minDate) = @minDate OR  r.EffectiveDate <= convert(date, getdate())) AND (ISNULL(r.ExpirationDate,@minDate) = @minDate OR  r.ExpirationDate >= convert(date, getdate())))
			BEGIN
				SET @ResultCode = -4
			END
			
			DROP TABLE #tmpUserDetail
		END
		ELSE
		BEGIN
		SELECT TOP 1
			U.UserID,
			U.UserGUID,
			U.UserName,
			U.FirstName,
			U.LastName,
			U.Password,
			U.EffectiveToDate,
			U.LoginAttempts,
			U.LoginCount,
			U.LastLogin,
			U.ModifiedOn,
			U.ModifiedBy,
			U.IsActive,
			@MaxPasswordAge - DATEDIFF(DAY, PL.AuditTimeStamp, GETUTCDATE()) AS DaysToExpire
		FROM
			#tmpUserDetail U
			INNER JOIN Core.PasswordLog PL
				ON U.UserID = PL.UserID
				AND ISNULL(u.Password,'') = ISNULL(PL.Password,'')
		ORDER BY
			PL.PasswordLogID DESC;

		DROP TABLE #tmpUserDetail;
		END
	end
	else --if (@ADFlag <> 1) 
	begin
		IF(@ADAuthenticated < 0)
		BEGIN
			SET @ResultCode = @ADAuthenticated
			SET @ResultMessage = 'Please Contact Your Administrator'
		END
		--If User does not have role raise an Error
		ELSE IF ( Not Exists ( SELECT RoleID FROM Core.UserRole WHERE UserID in (Select UserID from Core.Users where UserName = @UserName) AND IsActive = 1) ) 
		BEGIN
			SET @ResultCode = -4
			SET @ResultMessage = 'You do not have a Role. Please contact your administrator.'
		END
		ELSE IF EXISTS(SELECT 1 FROM Core.[Users] U WHERE u.UserName = @UserName AND u.IsActive <> 1)
		BEGIN
			SET @ResultCode = -1
		END
		ELSE IF NOT EXISTS(SELECT 1 FROM Core.[Users] U WHERE u.UserName = @UserName AND u.IsActive = 1 AND	ISNULL(u.EffectiveFromDate, DATEADD(MINUTE, 1, GETUTCDATE())) <= GETUTCDATE())
		BEGIN
			SET @ResultCode = -3
		END
		ELSE IF NOT EXISTS(SELECT 1 FROM Core.[Users] U INNER JOIN Core.UserRole ur ON ur.UserID = u.UserID  INNER JOIN Core.Role r ON r.RoleID = ur.RoleID WHERE u.UserName = @UserName AND u.IsActive = 1 AND ur.IsActive = 1
			AND (ISNULL(r.EffectiveDate,@minDate) = @minDate OR  r.EffectiveDate <= convert(date, getdate())) AND (ISNULL(r.ExpirationDate,@minDate) = @minDate OR  r.ExpirationDate >= convert(date, getdate())))
		BEGIN
			SET @ResultCode = -4
		END
		ELSE 
		BEGIN
			SELECT TOP 1
				u.UserID,
				u.UserGUID,
				u.UserName,
				u.FirstName,
				u.LastName,
				u.[Password], 
				u.EffectiveToDate,
				u.LoginAttempts,
				u.LoginCount,
				u.LastLogin,
				u.ModifiedOn,
				u.ModifiedBy,
				u.IsActive,
				@MaxPasswordAge AS DaysToExpire
			FROM Core.[Users] u
			LEFT JOIN Core.UserRole ur
				ON ur.UserID = u.UserID
			WHERE
				u.UserName = @UserName
				AND u.IsActive = 1
				AND ISNULL(u.EffectiveToDate, DATEADD(MINUTE, 1, GETUTCDATE())) >= GETUTCDATE() 
				AND ISNULL(u.EffectiveFromDate, DATEADD(MINUTE, 1, GETUTCDATE())) <= GETUTCDATE()
				AND (ur.IsActive = 1 or ur.UserRoleID is null)
		END
	end
		
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END