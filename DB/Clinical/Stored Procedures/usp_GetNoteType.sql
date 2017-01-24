-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetNoteType]
-- Author:		John Crossen
-- Date:		11/13/2015
--
-- Purpose:		Get list of Note Types
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/13/2015	Scott Martin	TFS# 3468 - Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Clinical.[usp_GetNoteType]

@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

SELECT [NoteTypeID]
      ,[NoteType]
      ,[IsActive]
      ,[ModifiedBy]
      ,[ModifiedOn]
 FROM [Clinical].[NoteType]


 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END