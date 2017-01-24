-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddReferralConcernDetails]
-- Author:		Sumana Sangapu
-- Date:		12/15/2015
--
-- Purpose:		Add Referral Concern details for Referral Client screen
--
-- Notes:		
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015   Sumana Sangapu - Initial Creation
-- 12/16/2015	Scott Martin	Added audit logging
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddReferralConcernDetails]
	@ReferralAdditionalDetailID BIGINT,
	@ReferralConcernID INT,
	@Diagnosis NVARCHAR(1000),
	@ReferralPriorityID INT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT, 
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID INT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ContactID BIGINT;
						
	SELECT  @ID = 0,
			@ResultCode = 0,
			@ResultMessage = 'executed successfully';

	SELECT @ContactID = ContactID FROM Registration.ReferralAdditionalDetails WHERE ReferralAdditionalDetailID = @ReferralAdditionalDetailID;

	BEGIN TRY
	INSERT INTO [Registration].[ReferralConcernDetails]
	(
		ReferralAdditionalDetailID,
		ReferralConcernID,
		Diagnosis,
		ReferralPriorityID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@ReferralAdditionalDetailID,
		@ReferralConcernID,
		@Diagnosis,
		@ReferralPriorityID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ReferralConcernDetails', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ReferralConcernDetails', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT 
				@ID = 0,
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END