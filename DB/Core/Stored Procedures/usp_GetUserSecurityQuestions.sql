-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_GetUserSecurityQuestions
-- Author:		Justin Spalti
-- Date:		09/29/2015
--
-- Purpose:		Gets the user's security questions and answers
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/28/2015 - Initial procedure creation for retrieving a user's security questions and answers
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetUserSecurityQuestions]
@UserID INT,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'Executed successfully'

	BEGIN TRY	
		SELECT u.UserID, usq.UserSecurityQuestionID, usq.SecurityQuestionID,
			   usq.IsActive, usq.ModifiedBy, usq.ModifiedOn, 
			   sq.Question, sq.QuestionDescription
		FROM Core.Users u
		INNER JOIN Core.[UserSecurityQuestion] usq
			ON usq.UserID = u.UserID
		INNER JOIN Reference.SecurityQuestion sq 
			ON sq.SecurityQuestionID = usq.SecurityQuestionID
		WHERE u.UserID = @UserID
			AND u.IsActive = 1
			AND usq.IsActive = 1
			AND sq.IsActive = 1
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END