-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_VerifySecurityDetails]
-- Author:		Rajiv Ranjan
-- Date:		08/26/2015
--
-- Purpose:		Verify security details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/26/2015	Rajiv Ranjan		Initial creation.
-- 08/27/2015	Rajiv Ranjan		Added RequestorIPAddress & also applied expiry check
-- 08/28/2015	Rajiv Ranjan		Join corrected for Email & UserEmail
-- 06/09/2016 - Sumana Sangapu - Ensure the user is Active - TFS #11254
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_VerifySecurityDetails]
	@SecurityQuestionID INT,
	@SecurityAnswer NVARCHAR(250),
	@ResetIdentifier UNIQUEIDENTIFIER,
	@RequestorIPAddress  NVARCHAR(30),
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY		
		DECLARE @UserID INT
			
		SELECT   
			@UserID=u.UserID
		FROM  
			Core.Users u
			INNER JOIN Core.[UserSecurityQuestion] usq ON u.UserID = usq.UserID 
			INNER JOIN Reference.SecurityQuestion sq ON usq.SecurityQuestionID = sq.SecurityQuestionID
			INNER JOIN Core.ResetPassword rp ON u.UserID = rp.UserID 
		WHERE
			u.IsActive = 1 AND
			usq.SecurityQuestionID=@SecurityQuestionID AND 
			PWDCOMPARE(@SecurityAnswer, usq.SecurityAnswer) = 1 AND 
			rp.ResetIdentifier = @ResetIdentifier AND
			rp.RequestorIPAddress=@RequestorIPAddress AND
			DATEDIFF(DAY, rp.ExpiresOn, GETUTCDATE()) <= 0 AND
			rp.IsActive = 1

		IF (ISNULL(@UserID, 0) = 0 )
		BEGIN
			RAISERROR ('Invalid security details.', -- Message text.
               16, -- Severity.
               1 -- State.
               );
		END
		ELSE
		BEGIN
			SELECT
				u.UserID,
				e.Email
			FROM
				Core.Users u
				INNER JOIN Core.UserEmail ue ON u.UserID = ue.UserID
				INNER JOIN Core.Email e ON e.EmailID = ue.EmailID
			WHERE
				u.UserID=@UserID ANd 
				u.IsActive = 1
		END
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END