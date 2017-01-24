----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetNoteHeaders]
-- Author:		Scott Martin
-- Date:		1/7/2016
--
-- Purpose:		Gets Note Headers for a specific module
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/7/2016	Scott Martin		Initial creation.
-- 01/11/2016	Gurpreet Singh	Added parameter NoteTypeID, removed ModuleID check
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetNoteHeaders]
	@ContactID BIGINT,
	@ModuleID BIGINT,
	@NoteTypeID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
		SELECT
			NH.NoteHeaderID,
			NH.ContactID,
			NT.NoteTypeID,
			NH.TakenBy,
			NH.TakenTime,
			NH.ModifiedBy,
			NH.ModifiedOn
		FROM
			Registration.NoteHeader NH
			INNER JOIN Reference.NoteType NT
				ON NH.NoteTypeID = NT.NoteTypeID
			INNER JOIN Reference.ModuleNoteType MNT
				ON NT.ModuleNoteTypeID = MNT.ModuleNoteTypeID
		WHERE
			NH.IsActive = 1
			AND NH.ContactID = @ContactID
			--AND MNT.ModuleID = @ModuleID
			AND (NT.[NoteTypeID] = @NoteTypeID OR ISNULL(@NoteTypeID, 0) = 0 )
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO
