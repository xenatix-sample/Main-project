----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetContactDischargeNotes]
-- Author:		Scott Martin
-- Date:		03/28/2016
--
-- Purpose:		Gets contact admission discharge notes
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/28/2016	Scott Martin		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetContactDischargeNotes]
	@ContactID BIGINT,
	@NoteTypeID INT,
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
		CDN.ModifiedOn
	FROM
		Registration.NoteHeader NH
		INNER JOIN Registration.ContactDischargeNote CDN
			ON NH.NoteHeaderID = CDN.NoteHeaderID
	WHERE
		CDN.IsActive = 1
		AND NH.ContactID = @ContactID
		AND NH.NoteTypeID = @NoteTypeID;
			
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO