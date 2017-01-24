
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Reference].[usp_GetProgressNoteType]
-- Author:		Gurpreet Singh
-- Date:		11/13/2015
--
-- Purpose:		Get list of Note Types for Progress Notes
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/13/2015	Gurpreet Singh	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetProgressNoteType]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully'

		SELECT [NoteTypeID]
			,[NoteType]
		FROM [Reference].[NoteType]
		WHERE [IsActive] = 1
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END