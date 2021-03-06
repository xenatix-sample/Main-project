-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateReferralDischarge]
-- Author:		Scott Martin
-- Date:		12/28/2015
--
-- Purpose:		Update Referral Discharge
--
-- Notes:		
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/28/2015   Scott Martin - Initial Creation
-- 01/11/2016	Gurpreet Singh	Added DischargeReasonID
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_UpdateDischarge]
	@DischargeID BIGINT,
	@ProgressNoteID BIGINT,
	@DischargeTypeID INT,
	@DischargeDate DATE,
	@TakenBy INT,
	@DischargeReasonID INT,
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
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'ECI', 'Discharge', @DischargeID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [ECI].[Discharge]
	SET	ProgressNoteID = @ProgressNoteID,
		DischargeTypeID = @DischargeTypeID,
		DischargeDate = @DischargeDate,
		DischargeReasonID = @DischargeReasonID,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		DischargeID = @DischargeID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'ECI', 'Discharge', @AuditDetailID, @DischargeID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END