
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [Reference].[usp_GetReferenceClosureReason]
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
-- 10/08/2015	John Crossen		TFS# 2663 - Initial creation.
-----------------------------------------------------------------------------------------------------------------------



CREATE PROCEDURE [Reference].[usp_GetReferenceClosureReason]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		ReferralClosureReasonID, ReferralClosureReason 
		FROM		[Reference].[ReferralClosureReason]
		WHERE		IsActive = 1
		ORDER BY	ReferralClosureReason  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO

