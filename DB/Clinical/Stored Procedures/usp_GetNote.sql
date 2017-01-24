-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetNote]
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
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Clinical].[usp_GetNote]
	@NoteID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

		   SELECT NoteID,Notes,NoteTypeID, ContactID, TakenBy, TakenTime, EncounterID,
		   ModifiedBy, ModifiedOn
		   FROM Clinical.Notes
		   WHERE IsActive=1 AND NoteID=@NoteID
	  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO