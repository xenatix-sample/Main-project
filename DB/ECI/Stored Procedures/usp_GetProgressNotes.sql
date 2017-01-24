-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Reference].[usp_GetReferralProgressNote]
-- Author:		Gurpreet Singh
-- Date:		01/08/2015
--
-- Purpose:		Get a Referral Progress Notes
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/08/2015	Gurpreet Singh		Initial Creation
--01/11/2016	Gurpreet Singh		Adde parameter ContactID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_GetProgressNotes]
	@NoteTypeID INT,
	@ContactID	BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully'
		
		SELECT
			pnote.[ProgressNoteID],
			pnote.[NoteHeaderID],
			pnote.[StartTime],
			pnote.[EndTime],
			pnote.[ContactMethodID],
			pnote.[ContactMethodOther],
			pnote.[FirstName],
			pnote.[LastName],
			pnote.[RelationshipTypeID],
			pnote.[Summary],
			pnote.[ReviewedSourceConcerns],
			pnote.[ReviewedECIProcess],
			pnote.[ReviewedECIServices],
			pnote.[ReviewedECIRequirements],
			pnote.[IsSurrogateParentNeeded],
			pnote.[Comments],
			pnote.[IsDischarged],
			pnote.[IsActive],
			pnote.[ModifiedBy],
			pnote.[ModifiedOn]
		FROM
			[ECI].[ProgressNote] pnote
		INNER JOIN [Registration].[NoteHeader] hnote ON pnote.[NoteHeaderID] = hnote.[NoteHeaderID]
		WHERE (hnote.[NoteTypeID] = @NoteTypeID OR ISNULL(@NoteTypeID, 0) = 0 )
			AND hnote.ContactID = @ContactID 
			AND pnote.[IsActive] = 1 AND hnote.[IsActive] = 1
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END