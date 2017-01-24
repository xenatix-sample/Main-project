-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Reference].[usp_GetReferralProgressNoteTypes]
-- Author:		Scott Martin
-- Date:		12/28/2015
--
-- Purpose:		Get list of Progress Note Types
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/28/2015	Scott Martin		Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetReferralProgressNoteTypes]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		[ProgressNoteTypeID],
		[ProgressNoteType],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn]
	FROM
		[Reference].[ProgressNoteType]
	WHERE
		[IsActive] = 1
	ORDER BY
		[ProgressNoteType]


 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END