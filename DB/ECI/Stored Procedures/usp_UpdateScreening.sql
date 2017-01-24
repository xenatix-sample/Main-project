-----------------------------------------------------------------------------------------------------------------------
-- Procedure:  [usp_UpdateScreening]
-- Author:		Sumana Sangapu
-- Date:		07/23/2015
--
-- Purpose:		Update Contact's ECI Screening Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/06/2015	Sumana Sangapu	TFS:2620	Initial Creation
-- 10/08/2015	Chad Roberts	TFS:1437	Program is read only and should never get updated
-- 11/24/2015	Scott Martin	TFS:3636	Added Audit logging
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_UpdateScreening]
	@ScreeningID bigint,
	@ContactID bigint,
	@InitialContactDate date,
	@InitialServiceCoordinatorID int,
	@PrimaryServiceCoordinatorID int,
	@ScreeningDate date,
	@ScreeningTypeID smallint,
	@AssessmentID bigint,
	@ScreeningScore int,
	@ScreeningResultsID smallint,
	@ScreeningStatusID smallint,
	@SubmittedByID int,
	@ResponseID bigint,
	@ModifiedOn DATETIME,
	@ModifiedBy int,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

SELECT
	@ResultCode = 0,
	@ResultMessage = 'Executed Successfully';

	BEGIN TRY
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'ECI', 'Screening', @ScreeningID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE s
	SET	InitialContactDate = @InitialContactDate,
		InitialServiceCoordinatorID = @InitialServiceCoordinatorID, 
		PrimaryServiceCoordinatorID = @PrimaryServiceCoordinatorID, 
		ScreeningDate = @ScreeningDate, 
		ScreeningTypeID = @ScreeningTypeID, 
		AssessmentID = @AssessmentID, 
		ScreeningResultsID = @ScreeningResultsID, 
		ScreeningScore = @ScreeningScore,
		ScreeningStatusID = @ScreeningStatusID,
		SubmittedByID = @SubmittedByID,
		ResponseID = @ResponseID,
		IsActive = 1, 
		ModifiedBy= @ModifiedBy, 
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()  
	FROM
		ECI.Screening s
	WHERE
		ScreeningID = @ScreeningID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'ECI', 'Screening', @AuditDetailID, @ScreeningID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode		= ERROR_SEVERITY(),
		@ResultMessage	= ERROR_MESSAGE()
	END CATCH
END