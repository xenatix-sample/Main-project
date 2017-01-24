-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateContactBenefit]
-- Author:		Avikal Gupta
-- Date:		08/25/2015
--
-- Purpose:		To update association between payor and contact in ContactPayor
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/24/2015	Avikal Task	Initial creation
-- 09/02/2015	Avikal	- Added ContactPayorRank and PolicyHolderName
-- 09/07/2015	Gurpreet Singh	--	Moved @ModifiedBy parameter to order as per the values passed from application
-- 09/08/2015	Gurpreet Singh	--	Added @PayorCode, @GroupID, @PlanName, @PlanCode, @GroupName parameters
-- 09/14/2015	Rajiv Ranjan	--  Update CoPay in ContcatPayor
-- 09/15/2015   John Crossen    -- Add additional fields to ContactBenefit
-- 09/21/2015   Arun Choudhary  -- Changes to add payor plan and payor group.
-- 09/23/2015   Arun Choudhary  -- TFS 2377 - Renamed ID1 to PolicyID.
-- 12/16/2015	Scott Martin	Added audit logging
-- 03/14/2016	Arun Choudhary	Added CoInsurance.
-- 03/16/2016	Kyle Campbell	TFS #7308 Added PayorExpirationReasonID
-- 06/14/2016	Atul Chauhan	Added PolicyHolderEmployer (PBI -11154 Policy Holder Employer Field)
-- 06/15/2016	Atul Chauhan	Added Payor Billing info
-- 06/17/2016	Atul Chauhan	Added GroupID,AdditionalInformation and removed unused parameters
-- 09/13/2016	Gaurav Gupta	Added HasPolicyHolderSameCardName parameter
-- 12/15/2016	Scott Martin	Updated auditing
---------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_UpdateContactBenefit]
	@ContactPayorID INT,
	@ContactID bigint,
	@PolicyHolderID int,
	@PolicyHolderName nvarchar(250),
	@PolicyHolderFirstName nvarchar(200),
	@PolicyHolderMiddleName nvarchar(200),
	@PolicyHolderLastName nvarchar(200),
	@PolicyHolderEmployer nvarchar(500),
	@PolicyHolderSuffixID INT,
	@BillingFirstName nvarchar(200),
	@BillingMiddleName nvarchar(200),
	@BillingLastName nvarchar(200),
	@BillingSuffixID INT,
	@AdditionalInformation nvarchar(3000),
	@PolicyID nvarchar(50) NULL,
	@Deductible decimal(18,2) NULL,
	@CoPay decimal(18,2) NULL,
	@CoInsurance decimal(18,2) NULL,
	@EffectiveDate date NULL,
	@ExpirationDate date NULL,
	@PayorExpirationReasonID int NULL,
	@ExpirationReason nvarchar(255),
	@AddRetroDate date,
	@ContactPayorRank int,

	@PayorID INT,
	@PlanID NVARCHAR(50) NULL, 
	@GroupID NVARCHAR(50) NULL, 
	
	@PayorPlanID INT ,
	@PayorGroupID INT ,
	@PayorAddressID BIGINT, 
	@HasPolicyHolderSameCardName BIT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SET NOCOUNT ON;
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
		
	
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactPayor', @ContactPayorID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Registration].[ContactPayor]
	SET [ContactID] = @ContactID,
		[PayorID] = @PayorID,
		PayorPlanID = @PayorPlanID,
		PayorGroupID= @PayorGroupID	,		
		[PolicyHolderID] = @PolicyHolderID,
		[PolicyHolderName] = @PolicyHolderName,
		[GroupID]=@GroupID,
		[PolicyHolderFirstName] = @PolicyHolderFirstName,
		[PolicyHolderMiddleName] = @PolicyHolderMiddleName,
		[PolicyHolderLastName] = @PolicyHolderLastName,
		[PolicyHolderEmployer] = @PolicyHolderEmployer,
		[PolicyHolderSuffixID] = @PolicyHolderSuffixID,
		[BillingFirstName] = @BillingFirstName,
		[BillingMiddleName] = @BillingMiddleName,
		[BillingLastName] = @BillingLastName,
		[BillingSuffixID] = @BillingSuffixID,
		[AdditionalInformation]=@AdditionalInformation,
		[PolicyID] = @PolicyID,
		[ContactPayorRank] = @ContactPayorRank,
		[Deductible] = @Deductible,
		[Copay] = @CoPay,
		[CoInsurance]=@CoInsurance,
		[EffectiveDate] = @EffectiveDate, 
		[ExpirationDate] = @ExpirationDate,
		[PayorExpirationReasonID] = @PayorExpirationReasonID,
		[ExpirationReason] = @ExpirationReason,
		[AddRetroDate] = @AddRetroDate,
		[PayorAddressID] =@PayorAddressID,
		[HasPolicyHolderSameCardName]=@HasPolicyHolderSameCardName,
		[ModifiedBy] = @ModifiedBy,
		[ModifiedOn] = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE 
		[ContactPayorID] = @ContactPayorID
		AND [IsActive] = 1;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactAddress', @AuditDetailID, @ContactPayorID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 	
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
