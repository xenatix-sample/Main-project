-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetSurgicalHistoryDetail]
-- Author:		Scott Martin
-- Date:		11/30/2015
--
-- Purpose:		Get Surgical History Detail Data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/30/2015	Scott Martin	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_GetSurgicalHistoryDetail]
	@SurgicalHistoryID BIGINT,
	@ModifiedBy int,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		[SurgicalHistoryDetailID],
		[SurgicalHistoryID],
		[Surgery],
		[Date],
		[Comments],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn]
	FROM
		[Clinical].[SurgicalHistoryDetail]
	WHERE
		SurgicalHistoryID = @SurgicalHistoryID
		AND IsActive=1
  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


