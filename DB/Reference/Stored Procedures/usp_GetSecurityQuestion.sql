-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetSecurityQuestion]
-- Author:		Rajiv Ranjan
-- Date:		08/05/2015
--
-- Purpose:		Get Security Question details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/05/2015	Rajiv Ranjan		Initial creation.
-- 08/25/2015	Rajiv Ranjan		Moved procedure from Core to Reference.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetSecurityQuestion]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT 
			 sq.SecurityQuestionID,
			 sq.Question,
			 sq.QuestionDescription
	    FROM 
			Reference.SecurityQuestion sq 
		WHERE
			sq.IsActive = 1
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO


