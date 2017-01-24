-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetServiceDetails]
-- Author:		Scott Martin
-- Date:		12/27/2016
--
-- Purpose:		Gets the list services
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/27/2016	Scott Martin	Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetServiceDetails]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT
			ServicesID  as ServiceID,
			ServiceName,
			IsInternal
		FROM
			Reference.[Services]
		WHERE
			IsActive = 1
			AND IsInternal = 0
		ORDER BY
			ServiceName ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
