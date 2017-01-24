----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetNoteTypes]
-- Author:		Scott Martin
-- Date:		1/7/2016
--
-- Purpose:		Gets Note Types for a specific module
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/7/2016		Scott Martin		Initial creation.
-- 03/16/2016	Sumana Sangapu		Removed ModuleID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetNoteTypes]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
		SELECT
			NT.NoteTypeID,
			NT.NoteType,
			NT.ModifiedBy,
			NT.ModifiedOn
		FROM
			Reference.NoteType NT
			INNER JOIN Reference.ModuleNoteType MNT
				ON NT.ModuleNoteTypeID = MNT.ModuleNoteTypeID
		WHERE
			NT.IsActive = 1
			
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO