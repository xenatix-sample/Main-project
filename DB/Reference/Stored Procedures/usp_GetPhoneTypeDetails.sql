-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetPhoneTypeDetails]
-- Author:		Rajiv Ranjan
-- Date:		08/16/2015
--
-- Purpose:		Gets the list of Phone Type lookup  details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/16/2015	Rajiv Ranjan	- Initial creation.
-- 09/07/2015	Avikal			- Order by PhoneType
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Reference].[usp_GetPhoneTypeDetails]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		
			PhoneTypeID, 
			PhoneType 
		FROM		
			[Reference].[PhoneType] 
		WHERE		
			IsActive = 1
		ORDER BY	
			PhoneType  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END


GO