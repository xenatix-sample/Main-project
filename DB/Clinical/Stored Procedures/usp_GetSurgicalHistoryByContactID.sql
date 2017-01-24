-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetSurgicalHistoryByContactID]
-- Author:		Scott Martin
-- Date:		11/30/2015
--
-- Purpose:		Get Surgical History Data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/30/2015	Scott Martin	Initial creation.
-- 11/30/2015	Scott Martin	Changed column TakenOn to TakenTime
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_GetSurgicalHistoryByContactID]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		[SurgicalHistoryID],
		[EncounterID],
		[ContactID],
		[TakenBy],
		[TakenTime],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn]
	FROM
		[Clinical].[SurgicalHistory]
	WHERE
		ContactID = @ContactID
		AND IsActive=1
  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


