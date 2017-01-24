-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetStateDetails]
-- Author:		Saurabh Sahu
-- Date:		07/21/2015
--
-- Purpose:		Retrieves state/province entries
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/21/2015	Saurabh Sahu			TFS# --- - Initial creation
-- 07/29/2015	Demetrios Christopher	TFS# 675 - Updated stored proc which retrieves state information to reflect new table and column names.
-- 07/30/2015	Sumana Sangapu			 1016	Change schema from dbo to Registration/Reference/Core
-- 08/03/2015   Sumana Sangapu	1016 - Changed proc name schema qualifier from dbo to Registration/Core/Reference
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetStateDetails]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		[StateProvinceID], [StateProvinceCode], [StateProvinceName] 
		FROM		[Reference].[StateProvince]
		WHERE		IsActive = 1
		ORDER BY	[StateProvinceName]  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

Go


