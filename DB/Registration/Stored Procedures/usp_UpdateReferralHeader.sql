-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateReferralHeader]
-- Author:		Sumana Sangapu
-- Date:		12/15/2015
--
-- Purpose:		Update Referral Header information n Referral Requester Screen
--
-- Notes:		
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015  Sumana Sangapu - Initial Creation
-- 12/16/2015	Scott Martin	Added audit logging
-- 01/06/2016	Lokesh Singhal	Rename @ReferralOrganization to @OtherOrganization
-- 04/12/2016   Lokesh Singhal  Added @OtherSource parameter
-- 12/15/2016	Scott Martin	Updated auditing
-- 01/04/2016	Sumana Sangapu	Added IsLinkedContact and IsReferrerConvertedToContact
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_UpdateReferralHeader]
	@ReferralHeaderID BIGINT,
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
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);
					
	SELECT @ResultCode = 0,
			@ResultMessage = 'executed successfully'

	BEGIN TRY
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ReferralHeader', @ReferralHeaderID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Registration].[ReferralHeader]
	SET ContactID = @ContactID	,
		ReferralStatusID = 	@ReferralStatusID,
		ReferralTypeID = @ReferralTypeID,
		ResourceTypeID = @ResourceTypeID,
		ReferralCategorySourceID = @ReferralCategorySourceID,
		ReferralOriginID = @ReferralOriginID,
		ProgramID	= @ProgramID,
		ReferralOrganizationID = @ReferralOrganizationID,
		OtherOrganization = @OtherOrganization,
		ReferralSourceID = @ReferralSourceID,
		OtherSource = @OtherSource,
		ReferralDate = @ReferralDate,
		IsLinkedToContact = @IsLinkedToContact,
		IsReferrerConvertedToCollateral = @IsReferrerConvertedToContact,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ReferralHeaderID = @ReferralHeaderID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ReferralHeader', @AuditDetailID, @ReferralHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	END TRY
	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH

END