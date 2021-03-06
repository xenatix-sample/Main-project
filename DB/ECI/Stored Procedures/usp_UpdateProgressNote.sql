-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateReferralProgressNote]
-- Author:		Scott Martin
-- Date:		12/28/2015
--
-- Purpose:		Update Referral Progress Note
--
-- Notes:		
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/28/2015   Scott Martin - Initial Creation
--01/11/2016	Gurpreet Singh	Removed parameter ContactID
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_UpdateProgressNote]
	@ProgressNoteID BIGINT,
	@NoteHeaderID BIGINT,
	@StartTime TIME,
	@EndTime TIME,
	@ContactMethodID INT,
	@ContactMethodOther NVARCHAR(50),
	@FirstName NVARCHAR(200),
	@LastName NVARCHAR(200),
	@RelationshipTypeID INT,
	@Summary NVARCHAR(1000),
	@ReviewedSourceConcerns BIT,
	@ReviewedECIProcess BIT,
	@ReviewedECIServices BIT,
	@ReviewedECIRequirements BIT,
	@IsSurrogateParentNeeded BIT,
	@Comments NVARCHAR(500),
	@IsDischarged BIT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT  @ResultCode = 0,
			@ResultMessage = 'executed successfully'

	BEGIN TRY
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'ECI', 'ProgressNote', @ProgressNoteID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	UPDATE ECI.[ProgressNote]
	SET	NoteHeaderID = @NoteHeaderID,
		StartTime = @StartTime,
		EndTime = @EndTime,
		ContactMethodID = @ContactMethodID,
		ContactMethodOther = @ContactMethodOther,
		FirstName = @FirstName,
		LastName = @LastName,
		RelationshipTypeID = @RelationshipTypeID,
		Summary = @Summary,
		ReviewedSourceConcerns = @ReviewedSourceConcerns,
		ReviewedECIProcess = @ReviewedECIProcess,
		ReviewedECIServices = @ReviewedECIServices,
		ReviewedECIRequirements = @ReviewedECIRequirements,
		IsSurrogateParentNeeded = @IsSurrogateParentNeeded,
		Comments = @Comments,
		IsDischarged = @IsDischarged,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ProgressNoteID = @ProgressNoteID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'ECI', 'ProgressNote', @AuditDetailID, @ProgressNoteID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END