----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddContactAdmission]
-- Author:		Scott Martin
-- Date:		03/21/2016
--
-- Purpose:		Adds specific organization level for contact
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/21/2016	Scott Martin		Initial creation.
-- 03/25/2016	Scott Martin		Removed DischargeDate and IsDischarged
-- 03/29/2016	Scott Martin		Merged EffectiveDate and EffectiveStartTime
-- 04/05/2016	Scott Martin		Added IsCompanyActive flag: Added code to add Company Admission if contact is discharged or no company record exists
-- 04/16/2016	Scott Martin		Removed section of code to add company record
-- 11/01/2016   Gaurav Gupta        Added AdmissionReasonID field
-- 12/15/2016	Scott Martin		Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddContactAdmission]
	@ContactID BIGINT,
	@OrganizationID BIGINT,
	@EffectiveDate DATETIME,
	@UserID INT,
	@IsDocumentationComplete BIT,
	@Comments NVARCHAR(255),
	@AdmissionReasonID INT,
	@IsCompanyActive BIT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@CompanyID INT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
	INSERT INTO Registration.ContactAdmission
	(
		ContactID,
		OrganizationID,
		EffectiveDate,
		UserID,
		IsDocumentationComplete,
		Comments,
		AdmissionReasonID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@ContactID,
		@OrganizationID,
		@EffectiveDate,
		@UserID,
		@IsDocumentationComplete,
		@Comments,
		@AdmissionReasonID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactAdmission', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactAdmission', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	  
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
