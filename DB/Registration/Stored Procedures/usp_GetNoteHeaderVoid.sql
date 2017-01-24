-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetNoteHeaderVoid]
-- Author:		Scott Martin
-- Date:		04/06/2016
--
-- Purpose:		Get Note Header Void record
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/06/2016	Scott Martin	  - Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetNoteHeaderVoid]
	@NoteHeaderID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
SELECT @ResultCode = 0,
		@ResultMessage = 'executed successfully'
	BEGIN TRY
	SELECT
		NoteHeaderVoidID,
		NoteHeaderID,
		NoteHeaderVoidReasonID,
		Comments
	FROM
		Registration.NoteHeaderVoid
	WHERE
		NoteHeaderID = @NoteHeaderID
		AND IsActive = 1;		 
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

