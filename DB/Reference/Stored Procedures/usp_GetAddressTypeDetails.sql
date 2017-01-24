-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [usp_GetAddressTypeDetails]
-- Author:		Sumana Sangapu
-- Date:		07/22/2015
--
-- Purpose:		Gets the list of  Address Type lookup Details 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/22/2015	Sumana Sangapu		TFS# 674 - Initial creation.
-- 07/30/2015   Sumana Sangapu	1016 - Changed schema from dbo to Registration/Core/Reference
-- 08/03/2015   Sumana Sangapu	1016 - Changed schema from dbo to Registration/Core/Reference
-- 12/14/2015   Gaurav Gupta    4301 -  Changed proc to sort values based on SortOrder column
-----------------------------------------------------------------------------------------------------------------------



CREATE PROCEDURE [Reference].[usp_GetAddressTypeDetails]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		AddressTypeID, AddressType 
		FROM		[Reference].[AddressType] 
		WHERE		IsActive = 1
		ORDER BY	SortOrder  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END


GO
