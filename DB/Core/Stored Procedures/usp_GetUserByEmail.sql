-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_GetUserByEmail
-- Author:		Rajiv Ranjan
-- Date:		08/24/2015
--
-- Purpose:		Gets user details by email
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/24/2015 - Initial Creation
-- 07/21/2016	RAV - Reviewed The Query http://sqlmag.com/sql-server-2000/designing-performance-null-or-not-null
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetUserByEmail]
@Email NVARCHAR(100),
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'Users retrieved successfully'

	BEGIN TRY	
		declare @adUserPasswordResetMessage varchar(255)
		select @adUserPasswordResetMessage = MessageBody
		from Core.messagetemplate
		where MessageTemplateID = 4

		SELECT 
			u.UserID, 
			UserGUID, 
			UserName, 
			FirstName, 
			LastName, 
			[Password], 
			EffectiveToDate, 
			LoginAttempts, 
			LoginCount, 
			LastLogin,
			u.ADFlag,
			--Check This Condition
			case when u.ADFlag = 0 then '' else @adUserPasswordResetMessage end as ADUserPasswordResetMessage
		FROM Core.[Users] u
			INNER JOIN Core.[UserEmail] ue ON u.UserID = ue.UserID
			INNER JOIN Core.[Email] e ON ue.EmailID = e.EmailID
		WHERE e.Email = @Email
			AND u.IsActive = 1
		
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END