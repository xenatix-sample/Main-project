-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddContactBenefit]
-- Author:		Avikal Gupta
-- Date:		08/28/2015
--
-- Purpose:		To insert association between payor and contact in ContactPayor
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/28/2015	Avikal Task	Initial creation
-- 09/02/2015	Avikal	- Added ContactPayorRank and PolicyHolderName
-- 09/04/2015   John Crossen -- Add ability to add a payor and payor group plan  TFS# 1882
-- 09/07/2015	Gurpreet Singh	--	Moved @ModifiedBy parameter to order as per the values passed from application
-- 09/08/2015	Gurpreet Singh	--	Added @PayorCode, @GroupID, @PlanName, @PlanCode, @GroupName parameters
-- 09/08/2015	Gurpreet Singh	--	Passed IsActive value as 1
-- 09/14/2015	Rajiv Ranjan	--  Add CoPay in ContcatPayor
-- 09/15/2015   John Crossen    -- Move data elements to COntactBenefit from Group Payor
-- 09/17/2015    John Crossen    -- Refactor Address
-- 09/21/2015    Arun Choudhary  -- Changes to accomodate new benefit structure.
-- 09/23/2015    Arun Choudhary  -- TFS 2377 - Renamed ID1 to PolicyID.
-- 12/16/2015	Scott Martin		Added audit logging
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-- 03/14/2016	Arun Choudhary	Added CoInsurance.
-- 03/16/2016	Kyle Campbell	TFS #7308	Added PayorExpirationReasonID
-- 06/14/2016	Atul Chauhan	Added PolicyHolderEmployer (PBI -11154 Policy Holder Employer Field)
-- 06/15/2016	Atul Chauhan	Added Payor Billing info
-- 06/17/2016	Atul Chauhan	Added GroupID,AdditionalInformation and removed unused parameters
-- 09/13/2016	Gaurav Gupta	Added HasPolicyHolderSameCardName field
-- 12/15/2016	Scott Martin	Updated auditing
---------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddContactBenefit]
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
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
		
	INSERT INTO [Registration].[ContactPayor]
	(
		[ContactID],
		[PayorID],
		PayorPlanID,
		PayorGroupID,
		[PolicyHolderID],
		[PolicyHolderName],
		[GroupID],
		[PolicyHolderFirstName],
		[PolicyHolderMiddleName],
		[PolicyHolderLastName],
		[PolicyHolderEmployer],
		[PolicyHolderSuffixID],
		[BillingFirstName],
		[BillingMiddleName],
		[BillingLastName],
		[BillingSuffixID],
		[AdditionalInformation],
		[ContactPayorRank],
		[PolicyID],
		[Deductible],
		[Copay],
		[CoInsurance],
		PayorAddressID,
		[EffectiveDate],
		[ExpirationDate],
		[PayorExpirationReasonID],
		[ExpirationReason],
		[AddRetroDate],
		[HasPolicyHolderSameCardName],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@ContactID,
		@PayorID,
		@PayorPlanID,
		@PayorGroupID,
		@PolicyHolderID,
		@PolicyHolderName,
		@GroupID,
		@PolicyHolderFirstName,
		@PolicyHolderMiddleName,
		@PolicyHolderLastName,
		@PolicyHolderEmployer,
		@PolicyHolderSuffixID,
		@BillingFirstName,
		@BillingMiddleName,
		@BillingLastName,
		@BillingSuffixID,
		@AdditionalInformation,
		@ContactPayorRank,
		@PolicyID,
		@Deductible,
		@CoPay,
		@CoInsurance,
		@PayorAddressID,
		@EffectiveDate,
		@ExpirationDate,
		@PayorExpirationReasonID,
		@ExpirationReason,
		@AddRetroDate,
		@HasPolicyHolderSameCardName,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ContactPayorID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactPayor', @ContactPayorID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactPayor', @AuditDetailID, @ContactPayorID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END