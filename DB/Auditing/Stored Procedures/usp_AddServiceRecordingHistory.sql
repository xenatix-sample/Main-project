-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Auditing].[usp_AddServiceRecordingHistory]
-- Author:		Scott Martin
-- Date:		12/12/2016
--
-- Purpose:		Add Service Recording History for Service Recording and all related tables
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/12/2016	Scott Martin	 Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Auditing].[usp_AddServiceRecordingHistory]
	@TransactionLogID BIGINT,
	@SourceHeaderIDList NVARCHAR(MAX),
	@ServiceRecordingSourceID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@AuditDetailID BIGINT;

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	SELECT
		SR.ServiceRecordingID
	INTO #ServiceRecordings
	FROM
		Core.ServiceRecording SR
	WHERE
		SR.SourceHeaderID IN (SELECT CAST(Items AS BIGINT) FROM Core.fn_Split (@SourceHeaderIDList, ','))
		AND SR.ServiceRecordingSourceID = @ServiceRecordingSourceID;

	INSERT INTO Auditing.ServiceRecordingHistory
	(
		[TransactionLogID],
		[ContactID],
		[ServiceRecordingID],
		[ServiceRecordingHeaderID],
		[ParentServiceRecordingID],
		[ServiceRecordingSourceID],
		[SourceHeaderID],
		[OrganizationID],
		[ServiceTypeID],
		[ServiceItemID],
		[AttendanceStatusID],
		[ServiceStatusID],
		[RecipientCodeID],
		[RecipientCode],
		[DeliveryMethodID],
		[ServiceLocationID],
		[ServiceStartDate],
		[ServiceEndDate],
		[TransmittalStatus],
		[NumberOfRecipients],
		[TrackingFieldID],
		[SupervisorUserID],
		[SentToCMHCDate],
		[UserID],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		[CreatedBy],
		[CreatedOn],
		[SystemCreatedOn],
		[SystemModifiedOn]
	)
	SELECT
		@TransactionLogID,
		SRD.ContactID,
		SR.[ServiceRecordingID],
		SR.[ServiceRecordingHeaderID],
		SR.[ParentServiceRecordingID],
		SR.[ServiceRecordingSourceID],
		SR.[SourceHeaderID],
		SR.[OrganizationID],
		SR.[ServiceTypeID],
		SR.[ServiceItemID],
		SR.[AttendanceStatusID],
		SR.[ServiceStatusID],
		SR.[RecipientCodeID],
		SR.[RecipientCode],
		SR.[DeliveryMethodID],
		SR.[ServiceLocationID],
		SR.[ServiceStartDate],
		SR.[ServiceEndDate],
		SR.[TransmittalStatus],
		SR.[NumberOfRecipients],
		SR.[TrackingFieldID],
		SR.[SupervisorUserID],
		SR.[SentToCMHCDate],
		SR.[UserID],
		SR.[IsActive],
		SR.[ModifiedBy],
		SR.[ModifiedOn],
		SR.[CreatedBy],
		SR.[CreatedOn],
		SR.[SystemCreatedOn],
		SR.[SystemModifiedOn]
	FROM
		Core.ServiceRecording SR
		INNER JOIN Core.vw_GetServiceRecordingDetails SRD
			ON SR.ServiceRecordingID = SRD.ServiceRecordingID
		INNER JOIN #ServiceRecordings SRS
			ON SR.ServiceRecordingID = SRS.ServiceRecordingID;

	INSERT INTO Auditing.ServiceRecordingVoidHistory
	(
		TransactionLogID,
		ServiceRecordingVoidID,
		ServiceRecordingID,
		ServiceRecordingVoidReasonID,
		IncorrectOrganization,
		IncorrectServiceType,
		IncorrectServiceItem,
		IncorrectServiceStatus,
		IncorrectSupervisor,
		IncorrectAdditionalUser,
		IncorrectAttendanceStatus,
		IncorrectStartDate,
		IncorrectStartTime,
		IncorrectEndDate,
		IncorrectEndTime,
		IncorrectDeliveryMethod,
		IncorrectServiceLocation,
		IncorrectRecipientCode,
		IncorrectTrackingField,
		Comments,
		NoteHeaderID,
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		[CreatedBy],
		[CreatedOn],
		[SystemCreatedOn],
		[SystemModifiedOn]
	)
	SELECT
		@TransactionLogID,
		ServiceRecordingVoidID,
		SRV.ServiceRecordingID,
		ServiceRecordingVoidReasonID,
		IncorrectOrganization,
		IncorrectServiceType,
		IncorrectServiceItem,
		IncorrectServiceStatus,
		IncorrectSupervisor,
		IncorrectAdditionalUser,
		IncorrectAttendanceStatus,
		IncorrectStartDate,
		IncorrectStartTime,
		IncorrectEndDate,
		IncorrectEndTime,
		IncorrectDeliveryMethod,
		IncorrectServiceLocation,
		IncorrectRecipientCode,
		IncorrectTrackingField,
		Comments,
		NoteHeaderID,
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		[CreatedBy],
		[CreatedOn],
		[SystemCreatedOn],
		[SystemModifiedOn]
	FROM
		Core.ServiceRecordingVoid SRV
		INNER JOIN #ServiceRecordings SR
			ON SRV.ServiceRecordingID = SR.ServiceRecordingID;

	INSERT INTO Auditing.ServiceRecordingAdditionalUserHistory
	(
		TransactionLogID,
		ServiceRecordingAdditionalUserID,
		ServiceRecordingID,
		UserID,
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		[CreatedBy],
		[CreatedOn],
		[SystemCreatedOn],
		[SystemModifiedOn]
	)
	SELECT
		@TransactionLogID,
		ServiceRecordingAdditionalUserID,
		SRAU.ServiceRecordingID,
		UserID,
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		[CreatedBy],
		[CreatedOn],
		[SystemCreatedOn],
		[SystemModifiedOn]
	FROM
		Core.ServiceRecordingAdditionalUser SRAU
		INNER JOIN #ServiceRecordings SR
			ON SRAU.ServiceRecordingID = SR.ServiceRecordingID;

	INSERT INTO Auditing.ServiceRecordingAttendeeHistory
	(
		TransactionLogID,
		ServiceRecordingAttendeeID,
		ServiceRecordingID,
		Name,
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		[CreatedBy],
		[CreatedOn],
		[SystemCreatedOn],
		[SystemModifiedOn]
	)
	SELECT
		@TransactionLogID,
		ServiceRecordingAttendeeID,
		SRA.ServiceRecordingID,
		Name,
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		[CreatedBy],
		[CreatedOn],
		[SystemCreatedOn],
		[SystemModifiedOn]
	FROM
		Core.ServiceRecordingAttendee SRA
		INNER JOIN #ServiceRecordings SR
			ON SRA.ServiceRecordingID = SR.ServiceRecordingID;
	
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
