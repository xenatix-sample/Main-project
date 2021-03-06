-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetServiceRecording]
-- Author:		Sumana Sangapu
-- Date:		01/28/2016
--
-- Purpose:		Get ServiceRecording
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/28/2016	Sumana Sangapu	  - Initial creation.
-- 03/23/2016	Scott Martin		Refactored the query to include new fields and join to a void table. Also changed the input params to just use ServiceRecordingID
-- 06/16/2016	Scott Martin		Added ServiceRecordingHeaderID and ParentServiceRecordingID
-- 10/152016	Sumana Sangapu		Added SentToCMHCDate 
-- 11/15/2016	Gurpreet Singh		Added SystemModifiedOn
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetServiceRecording]
	@SourceHeaderID BIGINT,
	@ServiceRecordingSourceID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully'

		SELECT
			SR.ServiceRecordingID,
			SR.ServiceRecordingHeaderID,
			SR.ParentServiceRecordingID,
			SR.ServiceRecordingSourceID,
			SR.SourceHeaderID,
			SR.OrganizationID,
			SR.ServiceTypeID,
			SR.ServiceItemID,
			SR.AttendanceStatusID,
			SR.ServiceStatusID,
			SR.RecipientCodeID,
			SR.RecipientCode,
			SR.DeliveryMethodID,
			SR.ServiceLocationID,
			SR.ServiceStartDate,
			SR.ServiceEndDate,
			SR.TransmittalStatus,
			SR.NumberOfRecipients,
			SR.TrackingFieldID,
			SR.SupervisorUserID,
			SRV.ServiceRecordingVoidID,
			SR.UserID,
			CAST(CASE
				WHEN SRV.ServiceRecordingVoidID IS NOT NULL THEN 1
				ELSE 0 END AS BIT) AS IsVoided,
			SRV.ServiceRecordingVoidID,
			SR.ModifiedBy,
			SR.ModifiedOn,
			SR.SentToCMHCDate,
			SR.SystemModifiedOn
		FROM
			[Core].[ServiceRecording] SR
			LEFT OUTER JOIN Core.ServiceRecordingVoid SRV
				ON SR.ServiceRecordingID = SRV.ServiceRecordingID
				AND SRV.IsActive = 1
		WHERE	SR.SourceHeaderID = @SourceHeaderID 
				AND	SR.ServiceRecordingSourceID = @ServiceRecordingSourceID
				AND SR.IsActive = 1 
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

