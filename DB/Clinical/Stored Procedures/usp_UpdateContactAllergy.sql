-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_UpdateContactAllergy]
-- Author:		John Crossen
-- Date:		11/13/2015
--
-- Purpose:		Add Contact Allergy
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/13/2015	John Crossen	TFS# 3566 - Initial creation.
-- 11/18/2015	Scott Martin	TFS 3610	Add audit logging and fixed update statement coding
-- 11/23/2015   Justin Spalti - Removed a few unnecessary parameters(ContactID and ID)
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn to Update statement
-- 02/17/2016	Scott Martin	Refactored audit logging
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_UpdateContactAllergy]
	@ContactAllergyID BIGINT,
	@EncounterID BIGINT NULL,
	@AllergyTypeID SMALLINT,
	@NoKnownAllergy BIT,
	@TakenBy INT,
	@TakenTime DATETIME,
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
		   @ResultMessage = 'executed successfully'

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Clinical', 'ContactAllergy', @ContactAllergyID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Clinical.ContactAllergy
	SET EncounterID = @EncounterID,
		AllergyTypeID = @AllergyTypeID,
		NoKnownAllergy = @NoKnownAllergy,
		TakenBy = @TakenBy,
		TakenTime = @TakenTime,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ContactAllergyID = @ContactAllergyID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Clinical', 'ContactAllergy', @AuditDetailID, @ContactAllergyID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO