-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Reference].[usp_GetDocumentStatus]
-- Author:		Scott Martin
-- Date:		11/19/2015
--
-- Purpose:		Get list of Note Types
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/19/2015	Scott Martin	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Reference.[usp_GetDocumentStatus]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT

AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		[DocumentStatusID],
		[DocumentStatus],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn]
	FROM
		[Reference].[DocumentStatus]

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END