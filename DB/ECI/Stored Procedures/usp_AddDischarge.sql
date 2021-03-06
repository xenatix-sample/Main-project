-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddReferralDischarge]
-- Author:		Scott Martin
-- Date:		12/28/2015
--
-- Purpose:		Add Referral Discharge
--
-- Notes:		
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/28/2015   Scott Martin - Initial Creation
-- 01/11/2016	Gurpreet Singh	Added DischargeReasonID
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedBy and CreatedOn field
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_AddDischarge]
	@ProgressNoteID BIGINT,
	@DischargeTypeID INT,
	@DischargeDate DATE,
	@TakenBy INT,
	@DischargeReasonID INT,
    @ModifiedOn DATETIME,
	@ModifiedBy	INT,
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
	INSERT ECI.Discharge
	(
		ProgressNoteID,
		DischargeTypeID,
		DischargeDate,
		TakenBy,
		DischargeReasonID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@ProgressNoteID,
		@DischargeTypeID,
		@DischargeDate,
		@TakenBy,
		@DischargeReasonID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'ECI', 'Discharge', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ECI', 'Discharge', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT 
				@ID = 0,
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END