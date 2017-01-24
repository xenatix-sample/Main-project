

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [ECI].[usp_GetECISource]
-- Author:		John Crossen
-- Date:		08/24/2015
--
-- Purpose:		Gets the list of  ECISource lookup Details 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/24/2015	John Crossen		TFS# 1487 - Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].usp_GetECISource
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		[ECISourceID], ECISource 
		FROM		[ECI].[ECISource]
		WHERE		IsActive = 1
		ORDER BY	ECISource  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END


GO

