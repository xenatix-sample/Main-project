-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetPayorType]
-- Author:		Atul Chauhan
-- Date:		12/07/2016
--
-- Purpose:		Gets the list of Payor Type
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/07/2016	Atul Chauhan		- Initial creation.
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Reference].[usp_GetPayorType]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		
		PayorTypeID, 
		PayorType 
		FROM		
				[Reference].[PayorType] 
		WHERE		
				IsActive = 1
		ORDER BY	SortOrder ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END


GO