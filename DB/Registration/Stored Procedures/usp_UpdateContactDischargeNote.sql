----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateContactDischargeNote]
-- Author:		Scott Martin
-- Date:		03/28/2016
--
-- Purpose:		Updates an Admission Discharge Note
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/28/2016	Scott Martin		Initial creation.
-- 04/19/2016	Scott Martin	Added filter for Program Unit when cancelling appointments
-- 10/05/2016   Vishal Joshi		Added IsDeceased and DeceasedDate parameters
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_UpdateContactDischargeNote]
	@ContactDischargeNoteID BIGINT,
	@ContactAdmissionID BIGINT,
	@DischargeReasonID INT,
	@DischargeDate DATETIME,
	@NoteTypeID INT,
	@SignatureStatusID INT,
	@NoteText NVARCHAR(MAX),
	@IsDeceased BIT,
	@DeceasedDate DATETIME,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ContactID BIGINT,
		@OrganizationID BIGINT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
	SELECT @ContactID = ContactID, @OrganizationID = OrganizationID FROM Registration.ContactAdmission WHERE ContactAdmissionID = @ContactAdmissionID;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactDischargeNote', @ContactDischargeNoteID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Registration.ContactDischargeNote
	SET ContactAdmissionID = @ContactAdmissionID,
		DischargeReasonID = @DischargeReasonID,
		DischargeDate = @DischargeDate,
		NoteText = @NoteText,
		SignatureStatusID = @SignatureStatusID,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ContactDischargeNoteID = @ContactDischargeNoteID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactDischargeNote', @AuditDetailID, @ContactDischargeNoteID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	IF @IsDeceased = 1
		BEGIN
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'Contact', @ContactID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE [Registration].[Contact] SET IsDeceased=@IsDeceased, DeceasedDate = @DeceasedDate WHERE ContactID = @ContactID

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'Contact', @AuditDetailID, @ContactID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END

	IF @SignatureStatusID = 2
		BEGIN
		EXEC Scheduling.usp_CancelAllApointmentsforIndividual @ContactID, 7, @OrganizationID, 'Appointment cancelled due to discharge', @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;
		END
	  
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
