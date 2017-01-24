-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_UpdateSocialRelationship]
-- Author:		Scott Martin
-- Date:		11/20/2015
--
-- Purpose:		Update Social Relationship
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/20/2015	Scott Martin	Initial creation.
-- 11/30/2015	Scott Martin	Changed column TakenOn to TakenTime
-- 12/03/2015	Arun Choudhary	Changed parameter ordering 
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn to Update statement
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_UpdateSocialRelationship]
	@SocialRelationshipID BIGINT,
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
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Clinical', 'SocialRelationship', @SocialRelationshipID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Clinical.SocialRelationship
	SET ContactID = @ContactID,
		ReviewedNoChanges=@ReviewedNoChanges,
		EncounterID = @EncounterID,
		TakenBy = @TakenBy,
		TakenTime = @TakenTime,
		ChildhoodHistory = @ChildhoodHistory,
		RelationshipHistory = @RelationshipHistory,
		FamilyHistory = @FamilyHistory,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		SocialRelationshipID = @SocialRelationshipID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Clinical', 'SocialRelationship', @AuditDetailID, @SocialRelationshipID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO