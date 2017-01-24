-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[ECI].[usp_GetDischarge]
-- Author:		Gurpreet Singh
-- Date:		01/09/2016
--
-- Purpose:		Get list of Referral Discharge
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/09/2016	Gurpreet Singh		Initial Creation
-- 01/11/2016	Gurpreet Singh		Added DischargeReasonID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_GetDischarge]
	@DischargeID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		[DischargeID],
		[ProgressNoteID],
		[DischargeTypeID],
		[DischargeDate],
		[TakenBy],
		[DischargeReasonID],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn]
	FROM
		[ECI].[Discharge]
	WHERE
		DischargeID = @DischargeID
		AND [IsActive] = 1

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
