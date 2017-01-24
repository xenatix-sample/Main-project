-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_MergeSelfPay
-- Author:		John Crossen
-- Date:		08/03/2016
--
-- Purpose:		Main procedure for Client Eligibility
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/10/2016 - Initial procedure creation
-- 08/16/2016	Scott Martin	Refactored proc to include auditing and storing results of merging
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].usp_MergeSelfPay
(
	@TransactionLogID BIGINT,
	@ContactID BIGINT,
	@ParentID BIGINT,
	@ChildID BIGINT,
	@TotalRecords INT OUTPUT,
	@TotalRecordsMerged INT OUTPUT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
)
AS
BEGIN
	SELECT @ResultCode = 0,
	
	@ResultMessage = 'Data saved successfully'

	BEGIN TRY	

	DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID)

	-- Get the records from both the Parent and Child and create new records for the new merged Contact
	DECLARE @IDS TABLE (PrimaryKey BIGINT, Completed BIT);

	INSERT INTO Registration.SelfPay
	(
		ContactID,
		OrganizationDetailID,
		SelfPayAmount,
		IsPercent,
		EffectiveDate,
		ExpirationDate,
		IsChildInConservatorship,
		IsNotAttested,
		IsApplyingForPublicBenefits,
		IsEnrolledInPublicBenefits,
		IsRequestingReconsideration,
		IsNotGivingConsent,
		IsOtherChildEnrolled,
		IsReconsiderationOfAdjustment,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	OUTPUT
		inserted.SelfPayID,
		0
	INTO
		@IDS
	SELECT
		@ContactID,
		OrganizationDetailID,
		SelfPayAmount,
		IsPercent,
		EffectiveDate,
		ExpirationDate,
		IsChildInConservatorship,
		IsNotAttested,
		IsApplyingForPublicBenefits,
		IsEnrolledInPublicBenefits,
		IsRequestingReconsideration,
		IsNotGivingConsent,
		IsOtherChildEnrolled,
		IsReconsiderationOfAdjustment,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Registration.SelfPay SP
	WHERE
		SP.ContactID IN (@ParentID, @ChildID)
		AND IsActive = 1;

	DECLARE @PKID BIGINT, @Loop INT

	SELECT @Loop = COUNT(*) FROM @IDS;
	SET @TotalRecords = @Loop;
	SET @TotalRecordsMerged = @Loop;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = PrimaryKey FROM @IDS WHERE Completed = 0;
	SET ROWCOUNT 0
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'SelfPay', @PKID, NULL, @TransactionLogID, 70, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'SelfPay', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE @IDS
	SET Completed = 1
	WHERE
		PrimaryKey = @PKID;
	END
	
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END