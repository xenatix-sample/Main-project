
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_VerifySecurityDetailsMyProfile]
-- Author:		Sumana Sangapu
-- Date:		03/28/2016
--
-- Purpose:		Verify security details on the My profile screen for Password and Digital Passwords
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/28/2016	Sumana Sangapu		Initial creation. 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_VerifySecurityDetailsMyProfile]
	@UserID BIGINT,
	@SecurityQuestionID INT,
	@SecurityAnswer NVARCHAR(250),
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID INT OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY		
		
		SELECT @ID = CASE WHEN PWDCOMPARE(@SecurityAnswer, usq.SecurityAnswer) = 1 THEN 1 ELSE 0 END
		FROM  
			Core.Users u
			INNER JOIN Core.[UserSecurityQuestion] usq ON u.UserID = usq.UserID 
			INNER JOIN Reference.SecurityQuestion sq ON usq.SecurityQuestionID = sq.SecurityQuestionID
		WHERE
			u.UserID = @UserID AND
			usq.SecurityQuestionID = @SecurityQuestionID AND 
			u.IsActive = 1

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO
