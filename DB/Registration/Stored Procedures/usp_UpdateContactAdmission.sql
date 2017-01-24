----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateContactAdmission]
-- Author:		Scott Martin
-- Date:		03/21/2016
--
-- Purpose:		Update specific organization level for contact
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/21/2016	Scott Martin		Initial creation.
-- 03/25/2016	Scott Martin		Removed DischargeDate and IsDischarged
-- 03/29/2016	Scott Martin		Merged EffectiveDate and EffectiveStartTime
-- 04/06/2016	Satish Singh 		Added IsCompanyActive flag: TODO - Need to update in respective table (Scott Martin)
-- 04/17/2016   Satish Singh		Updated filed IsActive
-- 11/01/2016   Gaurav Gupta		Updated filed AdmissionReasonID
-- 12/15/2016	Scott Martin		Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_UpdateContactAdmission]
	@ContactAdmissionID BIGINT,
	@ContactID BIGINT,
	@OrganizationID BIGINT,
	@EffectiveDate DATETIME,
	@UserID INT,
	@IsDocumentationComplete BIT,
	@Comments NVARCHAR(255),
	@AdmissionReasonID int,
	@IsCompanyActive BIT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactAdmission', @ContactAdmissionID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Registration.ContactAdmission
	SET ContactID = @ContactID,
		OrganizationID = @OrganizationID,
		EffectiveDate = @EffectiveDate,
		UserID = @UserID,
		IsDocumentationComplete = @IsDocumentationComplete,
		AdmissionReasonID = @AdmissionReasonID,
		Comments = @Comments,
		IsActive = @IsCompanyActive,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ContactAdmissionID = @ContactAdmissionID;
	 
	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactAdmission', @AuditDetailID, @ContactAdmissionID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
