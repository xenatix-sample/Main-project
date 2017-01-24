-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [usp_AddScreening]
-- Author:		Sumana Sangapu
-- Date:		07/23/2015
--
-- Purpose:		Add Contact's ECI Screening Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/06/2015	Sumana Sangapu	TFS:2620	Initial Creation
-- 11/24/2015	Scott Martin		TFS:3636	Added Audit logging
-- 11/26/2015   Satish Singh    TFS# 3799, SubmittedByID
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedBy and CreatedOn field
-- exec  [ECI].[usp_AddScreening] 1,1,'12/10/2015',1,1,'12/20/2015',1,2,10,1,1,'','',''
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_AddScreening]
	@ContactID bigint,
	@ProgramID int,
	@InitialContactDate date,
	@InitialServiceCoordinatorID int,
	@PrimaryServiceCoordinatorID int,
	@ScreeningDate date,
	@ScreeningTypeID smallint,
	@AssessmentID bigint,
	@ScreeningScore int,
	@ScreeningResultsID smallint,
	@ScreeningStatusID smallint,
	@ResponseID bigint,
	@ModifiedOn DATETIME,
	@ModifiedBy int,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT,
	@ID INT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@NewID BIGINT;

 SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully';

	BEGIN TRY
	INSERT INTO ECI.Screening 
	(
		ProgramID,
		InitialContactDate,
		InitialServiceCoordinatorID,
		PrimaryServiceCoordinatorID, 
		ScreeningDate,
		ScreeningTypeID,
		AssessmentID,
		ScreeningResultsID,
		ScreeningScore,
		ScreeningStatusID,
		ResponseID,
		IsActive,
		ModifiedBy,
		SubmittedByID,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES 
	(
		@ProgramID,
		@InitialContactDate,
		@InitialServiceCoordinatorID,
		@PrimaryServiceCoordinatorID, 
		@ScreeningDate,
		@ScreeningTypeID,
		@AssessmentID,
		@ScreeningResultsID,
		@ScreeningScore,
		@ScreeningStatusID,
		@ResponseID,
		1,
		@ModifiedBy,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);
			
	SELECT @ID =  SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'ECI', 'Screening', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ECI', 'Screening', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

---------------------------------Screening Contact----------------------------------------------------------------------------

	INSERT INTO ECI.ScreeningContact
	(
		ScreeningID,
		ContactID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES 
	(
		@ID,
		@ContactID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);
	
	SELECT @NewID =  SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'ECI', 'ScreeningContact', @NewID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ECI', 'ScreeningContact', @AuditDetailID, @NewID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 END TRY
  BEGIN CATCH
    SELECT
	  @ID = 0,
      @ResultCode = ERROR_SEVERITY(),
      @ResultMessage = ERROR_MESSAGE()
  END CATCH
END