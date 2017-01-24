-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_AddChiefComplaint]
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
-- 11/11/2015   John Crossen     TFS#3537 -- Move to clinical schema
-- 11/18/2015	Scott Martin	TFS 3610	Add audit logging
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_AddChiefComplaint]
	@ChiefComplaintID BIGINT,
	@ContactID BIGINT,
	@ChiefComplaint NVARCHAR(1000),
	@TakenBy INT,
	@TakenTime DATETIME,
	@EncounterID BIGINT NULL,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	INSERT INTO  Clinical.ChiefComplaint
	(
		ContactID,
		ChiefComplaint,
		TakenBy,
		TakenTime,
		EncounterID,
		ModifiedBy,
		ModifiedOn,
		[CreatedBy],
		[CreatedOn]
	)
	VALUES
	(
		@ContactID,
		@ChiefComplaint,
		@TakenBy,
		@TakenTime,
		@EncounterID,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);
		
	SELECT @ID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Clinical', 'ChiefComplaint', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Clinical', 'ChiefComplaint', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE();
		
		EXEC Core.usp_LogException @ResultMessage, @ProcName, NULL, NULL, @ModifiedBy, NULL, NULL;						
	END CATCH
END