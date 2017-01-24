
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetCallStatusDetails]
-- Author:		Sumana Sangapu
-- Date:		01/18/2016
--
-- Purpose:		Gets the list of CallStatus lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/18/2016	Sumana Sangapu	- Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [CallCenter].[usp_GetCallStatusDetails]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		CallStatusID, [CallStatus]  
		FROM		[CallCenter].[CallStatus] 
		WHERE		IsActive = 1
		ORDER BY	[CallStatus] ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END