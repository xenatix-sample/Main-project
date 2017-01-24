
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetPolicyHolderDetails]
-- Author:		Sumana Sangapu
-- Date:		07/23/2015
--
-- Purpose:		Gets the list of PolicyHolder lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Sumana Sangapu		TaskID # 588 -  Initial creation.
-- 07/30/2015	Sumana Sangapu			    1016 -	Change schema from dbo to registration
-- 08/03/2015   Sumana Sangapu	1016 - Changed proc name schema qualifier from dbo to Registration/Core/Reference
-----------------------------------------------------------------------------------------------------------------------



CREATE PROCEDURE [Reference].[usp_GetPolicyHolderDetails]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		PolicyHolderID, PolicyHolder  
		FROM		[Reference].[PolicyHolder] 
		WHERE		IsActive = 1
		ORDER BY	PolicyHolder  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END


GO