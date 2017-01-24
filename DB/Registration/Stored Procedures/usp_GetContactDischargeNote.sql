----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetContactDischargeNote]
-- Author:		Scott Martin
-- Date:		03/28/2016
--
-- Purpose:		Gets specific discharge note
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/28/2016	Scott Martin		Initial creation.
-- 10/06/2016   Vishal Joshi    Added IsDeceased and DeceasedDate to result set
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetContactDischargeNote]
	@ContactDischargeNoteID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	SELECT
		CDN.ContactDischargeNoteID,
		CDN.ContactAdmissionID,
		CDN.NoteHeaderID,
		CDN.DischargeDate,
		CDN.DischargeReasonID,
		CDN.NoteText,
		CDN.SignatureStatusID,
		CDN.ModifiedBy,
		CDN.ModifiedOn,
		C.IsDeceased,
		C.DeceasedDate
	FROM
		Registration.ContactDischargeNote CDN
		LEFT JOIN Registration.NoteHeader NH ON CDN.NoteHeaderID = NH.NoteHeaderID 
		LEFT JOIN Registration.Contact C ON NH.ContactID = C.ContactID
	WHERE
		CDN.IsActive = 1
		AND CDN.ContactDischargeNoteID = @ContactDischargeNoteID;
			
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO
