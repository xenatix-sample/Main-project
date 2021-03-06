
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [Reference].[usp_GetReferralCategory]
-- Author:		John Crossen
-- Date:		10/08/2015
--
-- Purpose:		Gets the list of  ReferralCategory lookup Details 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/08/2015	John Crossen		TFS# 1487 - Initial creation.
-- 12/21/2015	Gurpreet Singh		Removed ProgramID parameter
-----------------------------------------------------------------------------------------------------------------------



CREATE PROCEDURE Reference.usp_GetReferralCategory
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		ReferralCategoryID, [ReferralCategory] 
		FROM		[Reference].[ReferralCategory]
		WHERE		IsActive = 1 
		ORDER BY	[ReferralCategory]  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
