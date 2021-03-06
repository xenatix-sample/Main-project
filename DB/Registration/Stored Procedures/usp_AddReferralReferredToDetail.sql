-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_AddReferralReferredToDetail]
-- Author:		Scott Martin
-- Date:		12/15/2015
--
-- Purpose:		Add Referral 'Referred To' Detail
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015	Scott Martin	Initial creation.
-- 12/16/2015   Satish Singh    Added ProgramID and ReferredDateTime
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddReferralReferredToDetail]
	@ReferralHeaderID BIGINT,	
	@OrganizationID BIGINT,
	@ReferredDateTime DATETIME,
	@ActionTaken NVARCHAR(500),
	@Comments NVARCHAR(500),
	@UserID INT,
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
	
	INSERT INTO [Registration].[ReferralReferredToDetails]
	(
		[ReferralHeaderID],		
		[OrganizationID],
		[ReferredDateTime],
		[ActionTaken],
		[Comments],
		[UserID],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@ReferralHeaderID,		
		@OrganizationID,
		@ReferredDateTime,
		@ActionTaken,
		@Comments,
		@UserID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ReferralReferredToDetails', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ReferralReferredToDetails', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	  
 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END


