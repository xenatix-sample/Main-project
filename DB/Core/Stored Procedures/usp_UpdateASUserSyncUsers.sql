-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_UpdateADUserSyncUsers]
-- Author:		Sumana Sangapu
-- Date:		03/01/2016
--
-- Purpose:		Update the Users for AD Services. Used in ADUserSync SSIS package.
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
--03/01/2016	Sumana Sangapu	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateADUserSyncUsers]
@UserName nvarchar(100),
@UserGUID  nvarchar(500),
@FirstName nvarchar(50), 
@LastName nvarchar(50), 
@IsTemporaryPassword bit, 
@EffectiveToDate datetime, 
@LoginAttempts int, 
@LoginCount int, 
@LastLogin datetime, 
@IsActive bit, 
@ModifiedBy int
AS

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE u
	SET 
		FirstName = @FirstName, 
		LastName = @LastName, 
		IsTemporaryPassword = @IsTemporaryPassword, 
		EffectiveToDate = @EffectiveToDate, 
		LoginAttempts = @LoginAttempts, 
		LoginCount = @LoginCount, 
		LastLogin = @LastLogin, 
		IsActive = @IsActive, 
		ModifiedBy = @ModifiedBy, 
		ModifiedOn = GETUTCDATE(), 
		SystemModifiedOn = GETUTCDATE()
	FROM Core.Users u
	WHERE	u.UserName = @UserName 
	AND		u.UserGUID = @UserGUID
 
END
