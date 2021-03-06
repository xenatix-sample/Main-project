-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_AddReferralForwardedDetail]
-- Author:		Scott Martin
-- Date:		12/15/2015
--
-- Purpose:		Add Referral Outcome Detail
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015	Scott Martin	Initial creation. 
-- 12/22/2015   Tejareddy Maryada  Commented @ID 
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddReferralForwardedDetail]
	@ReferralHeaderID BIGINT,
	@OrganizationID INT,
	@SendingReferralToID INT,
	@Comments NVARCHAR(500),
	@UserID INT,
	@ReferralSentDate DATE,	
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ID BIGINT,
		@ContactID BIGINT;

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	SELECT @ContactID = ContactID FROM Registration.ReferralAdditionalDetails WHERE ReferralHeaderID = @ReferralHeaderID;
	
	INSERT INTO [Registration].[ReferralForwardedDetails]
	(
		[ReferralHeaderID],
		[OrganizationID],
		[SendingReferralToID],
		[Comments],
		[UserID],
		[ReferralSentDate],
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
		@SendingReferralToID,
		@Comments,
		@UserID,
		@ReferralSentDate,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ReferralForwardedDetails', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ReferralForwardedDetails', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	  
 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


