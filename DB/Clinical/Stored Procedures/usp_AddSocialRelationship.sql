-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_AddSocialRelationship]
-- Author:		Scott Martin
-- Date:		11/20/2015
--
-- Purpose:		Add Social Relationship Data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/20/2015	Scott Martin	Initial creation.
-- 11/30/2015	Scott Martin	Changed column TakenOn to TakenTime
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_AddSocialRelationship]
	@ContactID BIGINT,
	@EncounterID BIGINT NULL,
	@ReviewedNoChanges BIT,
	@TakenBy INT,
	@TakenTime DATETIME,
	@ChildhoodHistory NVARCHAR(1000),
	@RelationshipHistory NVARCHAR(1000),
	@FamilyHistory NVARCHAR(1000),
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	INSERT INTO [Clinical].[SocialRelationship]
	(
		[EncounterID],
		[ContactID],
		ReviewedNoChanges,
		[TakenBy],
		[TakenTime],
		[ChildhoodHistory],
		[RelationshipHistory],
		[FamilyHistory],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		[CreatedBy],
		[CreatedOn]
	)
	VALUES
	(
		@EncounterID,
		@ContactID,
		@ReviewedNoChanges,
		@TakenBy,
		@TakenTime,
		@ChildhoodHistory,
		@RelationshipHistory,
		@FamilyHistory,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Clinical', 'SocialRelationship', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Clinical', 'SocialRelationship', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
  
 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE();
	END CATCH
END
GO