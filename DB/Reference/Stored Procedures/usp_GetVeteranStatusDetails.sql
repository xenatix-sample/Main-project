
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_GetVeteranStatusDetails
-- Author:		Sumana Sangapu
-- Date:		07/23/15
--
-- Purpose:		Lists the Veteran Status Details
--
-- Notes:		
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/15	Sumana Sangapu		TFS# 674 - Initial creation.
-- 07/30/2015	Sumana Sangapu			 1016	Change schema from dbo to Registration/Reference/Core
-- 08/03/2015   Sumana Sangapu	1016 - Changed proc name schema qualifier from dbo to Registration/Core/Reference
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Reference].[usp_GetVeteranStatusDetails] 
(
		@ResultCode INT OUTPUT,
		@ResultMessage NVARCHAR(500) OUTPUT
)
AS
BEGIN

SET NOCOUNT ON ;

	SELECT @ResultCode = 0,
		   @ResultMessage = OBJECT_NAME(@@PROCID) + ' executed successfully'

	BEGIN TRY	
		SELECT		VeteranStatusID, VeteranStatus
		FROM		[Reference].[VeteranStatus] 
		WHERE		IsActive = 1
		ORDER BY	VeteranStatus  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH

END
