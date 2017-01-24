-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Reference].[usp_GetReferralDischarges]
-- Author:		Scott Martin
-- Date:		12/28/2015
--
-- Purpose:		Get list of Referral Discharges
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/28/2015	Scott Martin		Initial Creation
-- 01/11/2016	Gurpreet Singh		Added parameters ContactID, NoteTypeID and removed ProgressNoteID
-- 01/11/2016	Gurpreet Singh		Added DischargeReasonID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_GetDischarges]
	@ContactID BIGINT,
	@NoteTypeID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'
	SELECT
		dis.[DischargeID],
		dis.[ProgressNoteID],
		dis.[DischargeTypeID],
		dis.[DischargeDate],
		dis.[TakenBy],
		dis.[DischargeReasonID],
		dis.[IsActive],
		dis.[ModifiedBy],
		dis.[ModifiedOn]
	FROM [Registration].[NoteHeader] NH
	INNER JOIN  [ECI].[ProgressNote] PN ON NH.NoteHeaderID = PN.NoteHeaderID
	INNER JOIN [ECI].[Discharge] dis ON PN.ProgressNoteID = dis.ProgressNoteID
	WHERE
		(NH.[NoteTypeID] = @NoteTypeID OR ISNULL(@NoteTypeID, 0) = 0 )
		AND NH.[ContactID] = @ContactID
		AND NH.[IsActive] = 1 AND dis.[IsActive] = 1

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END