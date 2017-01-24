-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Reference].[usp_GetConfirmationStatementGroup]
-- Author:		Scott Martin
-- Date:		04/09/2016
--
-- Purpose:		Get Confirmation Statement Groups
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/09/2016	Scott Martin	Initial Creation
-- 04/12/2016	Scott Martin	Removed DocumentType parameter
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetConfirmationStatementGroup]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
	SELECT	@ResultCode = 0,
			@ResultMessage = 'executed successfully';

	SELECT		
		ConfirmationStatementGroupID,
		ConfirmationStatementGroup		
	FROM
		Reference.ConfirmationStatementGroup CSG
	WHERE
		IsActive = 1

	END TRY

	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE();
	END CATCH
END
GO