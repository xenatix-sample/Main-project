-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_UpdateChiefComplaint]
-- Author:		John Crossen
-- Date:		10/27/2015
--
-- Purpose:		Update Chief Complaints
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/30/2015	John Crossen	TFS# 2886 - Initial creation.
-- 11/18/2015	Scott Martin	TFS 3610	Add audit logging
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn to Update statement
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_UpdateChiefComplaint]
	@ChiefComplaintID BIGINT,
	@ContactID BIGINT,
	@ChiefComplaint NVARCHAR(1000),
	@TakenBy INT,
	@TakenTime DATETIME,
	@EncounterID BIGINT NULL,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Clinical', 'ChiefComplaint', @ChiefComplaintID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Clinical.ChiefComplaint
	SET ContactID = @ContactID,
		TakenBy = @TakenBy,
		TakenTime = @TakenTime,
		ChiefComplaint = @ChiefComplaint,
		EncounterID = @EncounterID,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ChiefComplaintID = @ChiefComplaintID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Clinical', 'ChiefComplaint', @AuditDetailID, @ChiefComplaintID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO