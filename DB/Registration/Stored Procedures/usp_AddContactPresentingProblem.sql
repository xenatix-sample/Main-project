-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_AddContactPresentingProblem]
-- Author:		Scott Martin
-- Date:		03/28/2016
--
-- Purpose:		Add Contact Presenting Problem Data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/28/2016	Scott Martin	Initial creation.
-- 03/31/2016	Scott Martin	Removed OrganizationID and replaced it with PresentingProblemTypeID
-- 09/16/2016	Scott Martin		Added TransactionLogID and ModuleComponentID and modified Audit proc call
-- 09/16/2016	Kyle Campbell	TFS #14793	Add Change Log proc call
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddContactPresentingProblem]
	@TransactionLogID BIGINT,
	@ModuleComponentID BIGINT,
	@ContactID BIGINT,
	@PresentingProblemTypeID SMALLINT NULL,
	@EffectiveDate DATE,
	@ExpirationDate DATE,
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

	INSERT INTO [Registration].[ContactPresentingProblem]
	(
		[ContactID],
		[PresentingProblemTypeID],
		[EffectiveDate],
		[ExpirationDate],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@ContactID,
		@PresentingProblemTypeID,
		@EffectiveDate,
		@ExpirationDate,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactPresentingProblem', @ID, NULL, @TransactionLogID, @ModuleComponentID, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactPresentingProblem', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	EXEC Auditing.usp_AddContactDemographicChangeLog @TransactionLogID, @ContactID, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


