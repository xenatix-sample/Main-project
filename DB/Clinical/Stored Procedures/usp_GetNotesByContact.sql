-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetNotesByContact]
-- Author:		Scott Martin
-- Date:		11/13/2015
--
-- Purpose:		Get a single note
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/13/2015	Scott Martin	Initial creation.
-- 11/20/2015	Gurpreet Singh	Added DocumentStatusID parameter in get query
-- 12/09/2015   Arun Choudhary  Added order by takentime desc.
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Clinical].[usp_GetNotesByContact]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

		   SELECT NoteID,Notes,NoteTypeID, ContactID, TakenBy, TakenTime, EncounterID, DocumentStatusID,
		   ModifiedBy, ModifiedOn
		   FROM Clinical.Notes
		   WHERE IsActive=1 AND ContactID=@ContactID
		   ORDER BY TakenTime DESC
	  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO