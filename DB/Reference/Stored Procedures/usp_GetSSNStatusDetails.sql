-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetSSNStatusDetails]
-- Author:		Sumana Sangapu
-- Date:		07/22/2015
--
-- Purpose:		Gets the list of SSN Status lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/22/2015	Sumana Sangapu		TFS# 674 - Initial creation.
-- 07/30/2015	Sumana Sangapu			 1016	Change schema from dbo to Registration/Reference/Core
-- 08/03/2015   Sumana Sangapu	1016 - Changed proc name schema qualifier from dbo to Registration/Core/Reference
-----------------------------------------------------------------------------------------------------------------------



CREATE PROCEDURE [Reference].[usp_GetSSNStatusDetails]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		SSNStatusID, SSNStatus 
		FROM		[Reference].[SSNStatus] 
		WHERE		IsActive = 1
		ORDER BY	SSNStatus  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END


GO