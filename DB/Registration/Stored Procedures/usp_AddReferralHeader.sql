-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddReferralHeader]
-- Author:		Sumana Sangapu
-- Date:		12/15/2015
--
-- Purpose:		Add Referral Header details for Referral Header / Requestor screen
--
-- Notes:		
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015   Sumana Sangapu  Initial Creation
-- 12/16/2015	Scott Martin	Added audit logging
-- 12/22/2015	Sumana Sangapu	Mapped to ContactID 
-- 12/22/2015	Sumana Sangapu	Added ProgramID
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-- 04/12/2016   Lokesh Singhal  Added @OtherSource parameter
-- 01/04/2016	Sumana Sangapu	Added IsLinkedContact and IsReferrerConvertedToContact
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddReferralHeader]
	@ContactID BIGINT,
	@ReferralStatusID INT,
	@ReferralTypeID INT,
	@ResourceTypeID INT,
	@ReferralCategorySourceID INT,
	@ReferralOriginID INT, 
	@ProgramID INT,
	@ReferralOrganizationID INT,
	@OtherOrganization NVARCHAR(100),
	@ReferralSourceID INT,
	@OtherSource NVARCHAR(100),
	@ReferralDate DATE,
	@IsLinkedToContact BIT,
	@IsReferrerConvertedToContact BIT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID INT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);
					
	SELECT  @ID = 0,
			@ResultCode = 0,
			@ResultMessage = 'executed successfully'

	BEGIN TRY
	INSERT [Registration].[ReferralHeader]
	(
		ContactID,
		ReferralStatusID,
		ReferralTypeID,
		ResourceTypeID,
		ReferralCategorySourceID,
		ReferralOriginID,
		ProgramID,
		ReferralOrganizationID,
		OtherOrganization,
		ReferralSourceID,
		OtherSource,
		ReferralDate,
		IsLinkedToContact,
		IsReferrerConvertedToCollateral,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@ContactID,
		@ReferralStatusID,
		@ReferralTypeID,
		@ResourceTypeID,
		@ReferralCategorySourceID,
		@ReferralOriginID,
		@ProgramID,
		@ReferralOrganizationID,
		@OtherOrganization,
		@ReferralSourceID,
		@OtherSource,
		@ReferralDate,
		@IsLinkedToContact,
		@IsReferrerConvertedToContact,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ReferralHeader', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ReferralHeader', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT 
				@ID = 0,
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
