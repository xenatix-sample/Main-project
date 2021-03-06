-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_AddReferralDispositionDetail]
-- Author:		Scott Martin
-- Date:		12/15/2015
--
-- Purpose:		Add Referral Disposition Detail
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015	Scott Martin	Initial creation.
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Registration.[usp_AddReferralDispositionDetail]
	@ReferralHeaderID BIGINT,
	@ReferralDispositionID INT,
	@ReasonForDenial NVARCHAR(500),
	@ReferralDispositionOutcomeID INT,
	@AdditionalNotes NVARCHAR(500),
	@UserID INT,
	@DispositionDate DATE,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ContactID BIGINT;

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	SELECT @ContactID = ContactID FROM Registration.ReferralAdditionalDetails WHERE ReferralHeaderID = @ReferralHeaderID;
	
	INSERT INTO [Registration].[ReferralDispositionDetails]
	(
		[ReferralHeaderID],
		[ReferralDispositionID],
		[ReasonforDenial],
		[ReferralDispositionOutcomeID],
		[AdditionalNotes],
		[UserID],
		[DispositionDate],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@ReferralHeaderID,
		@ReferralDispositionID,
		@ReasonForDenial,
		@ReferralDispositionOutcomeID,
		@AdditionalNotes,
		@UserID,
		@DispositionDate,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ReferralDispositionDetails', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ReferralDispositionDetails', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END


