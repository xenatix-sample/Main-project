----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddContactDischargeNote]
-- Author:		Scott Martin
-- Date:		03/28/2016
--
-- Purpose:		Adds an Admission Discharge Note
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/28/2016	Scott Martin		Initial creation.
-- 04/19/2016	Scott Martin	Added filter for Program Unit when cancelling appointments
-- 05/27/2016	Scott Martin	Added IsNull to SignatureStatus parameter
-- 10/05/2016   Vishal Joshi		Added IsDeceased and DeceasedDate parameters
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddContactDischargeNote]
	@ContactID BIGINT,
	@ContactAdmissionID BIGINT,
	@DischargeReasonID INT,
	@DischargeDate DATETIME,
	@NoteTypeID INT,
	@SignatureStatusID INT = 1,
	@NoteText NVARCHAR(MAX),
	@IsDeceased BIT,
	@DeceasedDate DATETIME,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@OrganizationID BIGINT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
	DECLARE @NoteHeaderID BIGINT;

	INSERT INTO Registration.NoteHeader
	(
		ContactID ,
		NoteTypeID ,
		TakenBy ,
		TakenTime ,
		IsActive ,
		ModifiedBy ,
		ModifiedOn ,
		CreatedBy ,
		CreatedOn
	)
	VALUES
	(
		@ContactID,
		@NoteTypeID,
		@ModifiedBy,
		@ModifiedOn,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @NoteHeaderID = SCOPE_IDENTITY();
			
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'NoteHeader', @NoteHeaderID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'NoteHeader', @AuditDetailID, @NoteHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	INSERT INTO Registration.ContactDischargeNote
	(
		NoteHeaderID,
		ContactAdmissionID,
		DischargeReasonID,
		DischargeDate,
		NoteText,
		SignatureStatusID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@NoteHeaderID,
		@ContactAdmissionID,
		@DischargeReasonID,
		@DischargeDate,
		@NoteText,
		ISNULL(@SignatureStatusID, 1),
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactDischargeNote', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactDischargeNote', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @OrganizationID = OrganizationID FROM Registration.ContactAdmission WHERE ContactAdmissionID = @ContactAdmissionID;

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
