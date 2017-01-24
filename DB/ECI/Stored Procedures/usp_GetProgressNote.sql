-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Reference].[usp_GetReferralProgressNote]
-- Author:		Scott Martin
-- Date:		12/28/2015
--
-- Purpose:		Get a Referral Progress Note
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/28/2015	Scott Martin		Initial Creation
-- 1/7/2016		Scott Martin		Moved to ECI schema, removed Referral prefix, removed Reference to NoteHeader
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_GetProgressNote]
	@ProgressNoteID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		[ProgressNoteID],
		[NoteHeaderID],
		[StartTime],
		[EndTime],
		[ContactMethodID],
		[ContactMethodOther],
		[FirstName],
		[LastName],
		[RelationshipTypeID],
		[Summary],
		[ReviewedSourceConcerns],
		[ReviewedECIProcess],
		[ReviewedECIServices],
		[ReviewedECIRequirements],
		[IsSurrogateParentNeeded],
		[Comments],
		[IsDischarged],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn]
	FROM
		ECI.[ProgressNote] PN
	WHERE
		ProgressNoteID = @ProgressNoteID
		AND PN.[IsActive] = 1

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END