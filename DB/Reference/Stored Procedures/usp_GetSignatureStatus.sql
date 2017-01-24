-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetSignatureStatus]
-- Author:		Kyle Campbell
-- Date:		03/15/2016
--
-- Purpose:		Gets the list of Signature Status
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/16/2016	Kyle Campbell	TFS #7829 Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Reference].[usp_GetSignatureStatus]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS

BEGIN
	SELECT	@ResultCode = 0,
			@ResultMessage = 'executed successfully';

	BEGIN TRY
		SELECT SignatureStatusID, SignatureStatus
		FROM Reference.SignatureStatus
		WHERE IsActive = 1
		ORDER BY SortOrder
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END