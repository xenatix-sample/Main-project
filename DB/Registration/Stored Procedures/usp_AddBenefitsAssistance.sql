-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_AddBenefitsAssistance]
-- Author:		Scott Martin
-- Date:		05/19/2016
--
-- Purpose:		Add Benefits Assistance data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/19/2016	Scott Martin	Initial creation.
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddBenefitsAssistance]
	@ContactID BIGINT,
	@DateEntered DATETIME,
	@UserID INT,
	@AssessmentID BIGINT,
	@ResponseID BIGINT,
	@DocumentStatusID INT,
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

	INSERT INTO  [Registration].[BenefitsAssistance]
	(
		ContactID,
		DateEntered,
		UserID,
		AssessmentID,
		ResponseID,
		DocumentStatusID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		[CreatedBy],
		[CreatedOn]
	)
	VALUES
	(
		@ContactID,
		@DateEntered,
		@UserID,
		@AssessmentID,
		@ResponseID,
		@DocumentStatusID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);
		
	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'BenefitsAssistance', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'BenefitsAssistance', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;			

 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE();
	END CATCH
END
GO