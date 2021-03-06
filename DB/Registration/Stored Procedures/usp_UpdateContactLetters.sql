-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_UpdateContactLetters]
-- Author:		Deepak Kumar
-- Date:		06/06/2016
--
-- Purpose:		Update ContactLetters
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/06/2016	Deepak Kumar	Initial creation.
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_UpdateContactLetters]
	@ContactLettersID BIGINT,
	@ContactID BIGINT,
	@SentDate DATETIME,
	@UserID INT,
	@AssessmentID BIGINT,
	@ResponseID BIGINT,
	@Comments NVARCHAR(4000),
	@LetterOutcomeID INT,
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

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactLetters', @ContactLettersID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Registration].[ContactLetters]
	SET ContactID = @ContactID,
		SentDate = @SentDate,
		--UserID = @UserID,
		AssessmentID = @AssessmentID,
		ResponseID = @ResponseID,
		Comments = @Comments,
		LetterOutcomeID = @LetterOutcomeID,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ContactLettersID = @ContactLettersID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactLetters', @AuditDetailID, @ContactLettersID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END