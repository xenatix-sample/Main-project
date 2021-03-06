
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_ConvertReferrerToCollateral]
-- Author:		Sumana Sangapu
-- Date:		05/01/2017
--
-- Purpose:		Add Referrer as Collateral
--
-- Notes:		N/A (or any additional notes)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/01/2017	Sumana Sangapu	Initial Creation
-- 01/18/2017	Sumana Sangapu	Added ReferralHeaderID
-- 01/20/2017	Gurpreet Singh	Fixed: on update new contact type is being created
-- 01/24/2017	Gurpreet Singh	#22132  made ID param nullable, removed @ID from update
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_ConvertReferrerToCollateral]
	@ParentContactID BIGINT,
	@ContactID BIGINT,
	@ContactTypeID INT,

	@IsPolicyHolder BIT,
	@RelationshipTypeID INT,
	@OtherRelationship NVARCHAR (200),
	@LivingWithClientStatus BIT,
	@ReferralHeaderID BIGINT,

	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT = NULL OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ContactRelationshipID BIGINT,
		@CollateralEffectiveDate DATETIME
	
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	SET @CollateralEffectiveDate = GETDATE()

	BEGIN TRY

	
		IF NOT EXISTS (SELECT TOP 1 * FROM Registration.ContactRelationship WHERE ParentContactID = @ParentContactID AND ChildContactID = @ContactID AND ReferralHeaderID = @ReferralHeaderID)
			BEGIN
				INSERT INTO [Registration].[ContactRelationship]
				(
					[ParentContactID],
					[ChildContactID],
					[LivingWithClientStatus],
					[ContactTypeID],
					[IsEmergency],
					[CollateralEffectiveDate],
					[ReferralHeaderID],
					[ModifiedBy],
					[ModifiedOn],
					CreatedBy,
					CreatedOn
				)
				VALUES
				(
					@ParentContactID,
					@ContactID,
					@LivingWithClientStatus,
					@ContactTypeID,
					0,
					@CollateralEffectiveDate,
					@ReferralHeaderID,
					@ModifiedBy,
					@ModifiedOn,
					@ModifiedBy,
					@ModifiedOn
				);

				SELECT @ContactRelationshipID = SCOPE_IDENTITY();

				EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactRelationship', @ContactRelationshipID, NULL, NULL, NULL, @ParentContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

				EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactRelationship', @AuditDetailID, @ContactRelationshipID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
				EXEC [Registration].[usp_AddContactRelationshipType] @ParentContactID, @ContactID, @RelationshipTypeID, @IsPolicyHolder, @OtherRelationship, @CollateralEffectiveDate, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @ID OUTPUT;

				SELECT @ID = @ContactRelationshipID
			END
		ELSE
			BEGIN
				SET @ContactRelationshipID = (SELECT ContactRelationshipID FROM Registration.ContactRelationship WHERE ParentContactID = @ParentContactID AND ChildContactID = @ContactID AND ReferralHeaderID= @ReferralHeaderID);

				EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactRelationship', @ContactRelationshipID, NULL, NULL, NULL, @ParentContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;		

				UPDATE [Registration].[ContactRelationship]
				SET [LivingWithClientStatus] = @LivingWithClientStatus,
					[ContactTypeID] = @ContactTypeID,
					[IsEmergency] = 0,
					[CollateralEffectiveDate] = @CollateralEffectiveDate,
					[ReferralHeaderID] = @ReferralHeaderID,

					[IsActive] = 1,
					[ModifiedBy] = @ModifiedBy,
					[ModifiedOn] = @ModifiedOn,
					[SystemModifiedOn] = GETUTCDATE()
				WHERE
					ContactRelationshipID = @ContactRelationshipID;

				EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactRelationship', @AuditDetailID, @ContactRelationshipID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
				EXEC [Registration].[usp_UpdateContactRelationshipType] @ContactRelationshipID, @ParentContactID, @ContactID, @RelationshipTypeID, @IsPolicyHolder, @OtherRelationship, @CollateralEffectiveDate, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;
			END

	END TRY

	BEGIN CATCH
		SELECT @ID = 0,
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE();
	END CATCH
END

