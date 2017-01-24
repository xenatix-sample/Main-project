

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [ECI].[usp_GetIFSPType]
-- Author:		Gurpreet Singh
-- Date:		08/24/2015
--
-- Purpose:		Gets the list of IFSP Type lookup Details 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/24/2015	Gurpreet Singh		Initial creation.

-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_GetIFSPType]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	

		SELECT [IFSPTypeID]
			,[IFSPType]
		FROM [ECI].[IFSPType]
		ORDER BY IFSPType ASC

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO

