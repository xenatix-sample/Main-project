-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_VerifyResetIdentifier]
-- Author:		Rajiv Ranjan
-- Date:		08/27/2015
--
-- Purpose:		Verify reset Identifier details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/27/2015	Rajiv Ranjan		Initial creation.
-- 06/09/2016 - Sumana Sangapu - Ensure the user is Active - TFS #11254
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_VerifyResetIdentifier]
	@ResetIdentifier UNIQUEIDENTIFIER,
	@RequestorIPAddress  NVARCHAR(30),
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY		
		IF NOT EXISTS(
		SELECT   
			u.UserID,
			rp.ResetIdentifier
		FROM  
			Core.Users u
			INNER JOIN Core.ResetPassword rp ON u.UserID = rp.UserID AND u.IsActive = 1
		WHERE
			@ResetIdentifier = rp.ResetIdentifier AND 
			rp.RequestorIPAddress=@RequestorIPAddress AND
			DATEDIFF(DAY, rp.ExpiresOn, GETUTCDATE()) <= 0 AND
			rp.IsActive = 1
		)
		BEGIN
			RAISERROR ('Invalid reset link or has been expired.', -- Message text.
               16, -- Severity.
               1 -- State.
               );
		END

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END