-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddReferralProgressNote]
-- Author:		Scott Martin
-- Date:		12/28/2015
--
-- Purpose:		Add Referral Progress Note
--
-- Notes:		
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/28/2015   Scott Martin - Initial Creation
-- 01/04/2016   John Crossen -- Changes to NoteHeader
-- 1/7/2016		Scott Martin	Removed NoteHeader References and added ContactMethodOther, Moved to ECI Schema and removed Referral prefix
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedBy and CreatedOn field
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_AddProgressNote]
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
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID INT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@AuditPost XML,
		@AuditID BIGINT;
				
	SELECT  @ID = 0,
			@ResultCode = 0,
			@ResultMessage = 'executed successfully'

	BEGIN TRY
	INSERT [ECI].[ProgressNote]
	(
		NoteHeaderID,
		StartTime,
		EndTime,
		ContactMethodID,
		ContactMethodOther,
		FirstName,
		LastName,
		RelationshipTypeID,
		Summary,
		ReviewedSourceConcerns,
		ReviewedECIProcess,
		ReviewedECIServices,
		ReviewedECIRequirements,
		IsSurrogateParentNeeded,
		Comments,
		IsDischarged,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@NoteHeaderID,
		@StartTime,
		@EndTime,
		@ContactMethodID,
		@ContactMethodOther,
		@FirstName,
		@LastName,
		@RelationshipTypeID,
		@Summary,
		@ReviewedSourceConcerns,
		@ReviewedECIProcess,
		@ReviewedECIServices,
		@ReviewedECIRequirements,
		@IsSurrogateParentNeeded,
		@Comments,
		@IsDischarged,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'ECI', 'ProgressNote', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ECI', 'ProgressNote', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT 
				@ID = 0,
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END