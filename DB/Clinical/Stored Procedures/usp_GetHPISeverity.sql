-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetHPISeverity]
-- Author:		Scott Martin
-- Date:		11/20/2015
--
-- Purpose:		Get list of HPI Severities
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/20/2015	Scott Martin	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_GetHPISeverity]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		[HPISeverityID],
		[HPISeverity],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn]
	 FROM
		Clinical.[HPISeverity]


 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


