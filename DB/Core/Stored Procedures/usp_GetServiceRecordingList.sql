-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetServiceRecordingList]
-- Author:		Deepak Kumar
-- Date:		07/26/2016	
--
-- Purpose:		Get list of ServiceRecordings
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/26/2016	Deepak Kumar		Initial creation.
-- 08/01/2016	Kyle Campbell		TFS #12668 Modified to use Call Status as Status for Call Center services
-- 08/02/2016   Gurpreet Singh		Included new fields
-- 08/02/2016   Gurpreet Singh		Included IsVoided and SignedOn
-- 09/02/2016   Gurpreet Singh		Added DocumentStatusID in case of CrisisLine/LawLiaison
-- 09/14/2016	Gaurav Gupta	    Update Service Recording Document Type ID and Sign Joined with Service Recording.
-- 12/15/2016	Sumana Sangapu		Add date range filters and Duration filter on the ServiceStartDates
-- 12/28/2016	Sumana Sangapu		Handle NULL Start and End Dates
-- 12/29/2016   Arun Choudhary		Handle service end duration for larger duration and fixed duration calculation issue
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetServiceRecordingList]
	@ContactID BIGINT,
	@ServiceRecordingSourceID INT,
	@StartDate DATE,
	@EndDate DATE,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully'

		DECLARE @Source VARCHAR(50)
		SELECT
			@Source = ServiceRecordingSource
		FROM
			Reference.ServiceRecordingSource
		WHERE
			ServiceRecordingSourceID=@ServiceRecordingSourceID

		SELECT @StartDate = CASE WHEN @StartDate IS NULL THEN '1950/01/01' ELSE @StartDate END

		SELECT @EndDate = CASE WHEN @EndDate IS NULL THEN '3001/01/01' ELSE @EndDate END
		
		IF(@Source='CallCenter' OR @Source='LawLiaison')--For CallCenter
		BEGIN
		With Sig (DocumentID, SignedOn)
          AS
          (
              SELECT
                   EDES.DocumentID,
                   MAX(EDES.SystemCreatedOn) AS SignedOn
              FROM
                   ESignature.DocumentEntitySignatures EDES
                   INNER JOIN ESignature.EntitySignatures ES
                        ON EDES.EntitySignatureID = ES.EntitySignatureID
              WHERE
                   EDES.DocumentTypeID = 9
                   AND ES.EntityTypeID = 1
              GROUP BY
                   EDES.DocumentID
          )	
			SELECT
				CH.CallCenterHeaderID AS SourceHeaderID
				,CSR.ServiceRecordingID
				,CSR.ServiceRecordingSourceID
				,CSR.ServicesID AS ServiceItemID
				,CSR.ServiceName
				,CSR.ServiceStartDate
				,CSR.ServiceEndDate
				,CASE WHEN CSR.ServiceEndDate IS NOT NULL THEN 
						    CASE WHEN CAST(DATEDIFF(second, CSR.ServiceStartDate, CSR.ServiceEndDate) / 60 / 60 / 24 AS NVARCHAR(50)) ='0' Then '' Else CAST(DATEDIFF(second,CSR.ServiceStartDate, CSR.ServiceEndDate) / 60 / 60 / 24 AS NVARCHAR(50)) + 'day ' End+
							CAST(DATEDIFF(second, CSR.ServiceStartDate, CSR.ServiceEndDate) / 60 / 60 % 24 AS NVARCHAR(50)) + 'hr '+
							CAST(DATEDIFF(second, CSR.ServiceStartDate, CSR.ServiceEndDate) / 60 % 60 AS NVARCHAR(50)) + 'mins'
						   ELSE NULL END AS Duration
				--,CSR.Duration		
				,ServiceDurationID = (SELECT ServiceDurationID 
										FROM Reference.ServiceDuration d 
										WHERE DATEDIFF(mi, CSR.ServiceStartDate, CSR.ServiceEndDate) BETWEEN 
												d.ServiceDurationStart AND ISNULL(d.ServiceDurationEnd, DATEDIFF(mi, CSR.ServiceStartDate, CSR.ServiceEndDate) + 1))
				,CSR.ProgramUnitID AS OrganizationID
				,ProgramUnitName
				,ServiceProviderID AS UserID
				,ServiceStatus
				,Case When CH.CallStatusID IS NOT NULL Then (SELECT DocumentStatusID FROM Reference.DocumentStatus LEFT JOIN CallCenter.CallStatus ON CallStatus = DocumentStatus WHERE CallStatusID =CH.CallStatusID)
						When CSR.IsVoided =1 Then (SELECT DocumentStatusID FROM Reference.DocumentStatus WHERE DocumentStatus = 'Void') 
						When Sig.SignedOn IS NOT NULL THEN (SELECT DocumentStatusID FROM Reference.DocumentStatus WHERE DocumentStatus = 'Completed')
						Else (SELECT DocumentStatusID FROM Reference.DocumentStatus WHERE DocumentStatus = 'Draft') END AS DocumentStatusID
				,CH.CallStatusID
				,CH.ModifiedOn
				,CASE CSR.IsVoided WHEN 0 THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS IsVoided
				,SIG.SignedOn 
			FROM
				CallCenter.CallCenterHeader CH
				INNER JOIN [CallCenter].[vw_GetCallCenterServiceRecordingDetails] CSR
					ON CSR.ContactID=CH.ContactID AND CH.CallCenterHeaderID=CSR.SourceHeaderID AND CSR.ServiceRecordingSourceID=@ServiceRecordingSourceID
				AND CH.IsActive = 1
				LEFT JOIN Sig ON Sig.DocumentID=CSR.ServiceRecordingID

			WHERE 
				CH.ContactID = @ContactID
				AND ( ISNULL(Cast(CSR.ServiceStartDate AS DATE),GETDATE()) BETWEEN @StartDate AND @EndDate)
		END
		ELSE IF(@Source='BenefitsAssistance')--BenefitsAssistance
		BEGIN
			SELECT
					BA.BenefitsAssistanceID AS SourceHeaderID
					,BASR.ServiceRecordingID
				,BASR.ServiceRecordingSourceID
				,BASR.ServicesID AS ServiceItemID
				,BASR.ServiceName
				,BASR.ServiceStartDate
				,BASR.ServiceEndDate
				--,BASR.ServiceStartTime
				,CASE WHEN BASR.ServiceEndDate IS NOT NULL THEN 
						    CASE WHEN CAST(DATEDIFF(second, BASR.ServiceStartDate, BASR.ServiceEndDate) / 60 / 60 / 24 AS NVARCHAR(50)) ='0' Then '' Else CAST(DATEDIFF(second,BASR.ServiceStartDate, BASR.ServiceEndDate) / 60 / 60 / 24 AS NVARCHAR(50)) + 'day ' End+
							CAST(DATEDIFF(second, BASR.ServiceStartDate, BASR.ServiceEndDate) / 60 / 60 % 24 AS NVARCHAR(50)) + 'hr '+
							CAST(DATEDIFF(second, BASR.ServiceStartDate, BASR.ServiceEndDate) / 60 % 60 AS NVARCHAR(50)) + 'mins'
						   ELSE NULL END AS Duration
				--,BASR.Duration
				,ServiceDurationID = (SELECT ServiceDurationID 
										FROM Reference.ServiceDuration d 
										WHERE DATEDIFF(mi, BASR.ServiceStartDate, BASR.ServiceEndDate) BETWEEN 
												d.ServiceDurationStart AND ISNULL(d.ServiceDurationEnd, DATEDIFF(mi, BASR.ServiceStartDate, BASR.ServiceEndDate) + 1))
				,ProgramUnitID AS OrganizationID
				,ProgramUnitName
				,ServiceProviderID AS UserID
				,ServiceStatus
				,BA.DocumentStatusID
				,NULL As CallStatusID
				,BA.ModifiedOn
				,CASE BASR.IsVoided WHEN 0 THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS IsVoided
				,NULL AS SignedOn
			FROM
				Registration.BenefitsAssistance BA
				INNER JOIN [Registration].[vw_GetBenefitsAssistanceServiceRecordingDetails] BASR
				ON BASR.ContactID=BA.ContactID AND BASR.ProgressNoteID = BA.BenefitsAssistanceID
			WHERE 
				BA.ContactID = @ContactID
					AND BA.IsActive = 1
			AND ( ISNULL(Cast(BASR.ServiceStartDate AS DATE),GETDATE())  BETWEEN @StartDate AND @EndDate)
		END
		ELSE IF(@Source='ContactForms')--ContactForms
		BEGIN
			SELECT
				CF.ContactFormsID AS SourceHeaderID
				,CFSR.ServiceRecordingID
				,CFSR.ServiceRecordingSourceID
				,CFSR.ServicesID AS ServiceItemID
				,CFSR.ServiceName
				,CFSR.ServiceStartDate
				,CFSR.ServiceEndDate
				,CASE WHEN CFSR.ServiceEndDate IS NOT NULL THEN 
						    CASE WHEN CAST(DATEDIFF(second, CFSR.ServiceStartDate, CFSR.ServiceEndDate) / 60 / 60 / 24 AS NVARCHAR(50)) ='0' Then '' Else CAST(DATEDIFF(second,CFSR.ServiceStartDate, CFSR.ServiceEndDate) / 60 / 60 / 24 AS NVARCHAR(50)) + 'day ' End+
							CAST(DATEDIFF(second, CFSR.ServiceStartDate, CFSR.ServiceEndDate) / 60 / 60 % 24 AS NVARCHAR(50)) + 'hr '+
							CAST(DATEDIFF(second, CFSR.ServiceStartDate, CFSR.ServiceEndDate) / 60 % 60 AS NVARCHAR(50)) + 'mins'
						   ELSE NULL END AS Duration
				--,BASR.Duration
				,ServiceDurationID = (SELECT ServiceDurationID 
										FROM Reference.ServiceDuration d 
										WHERE DATEDIFF(mi, CFSR.ServiceStartDate, CFSR.ServiceEndDate) BETWEEN 
												d.ServiceDurationStart AND ISNULL(d.ServiceDurationEnd, DATEDIFF(mi, CFSR.ServiceStartDate, CFSR.ServiceEndDate) + 1))
				--,CFSR.Duration
				,ProgramUnitID AS OrganizationID
				,ProgramUnitName
				,ServiceProviderID AS UserID
				,ServiceStatus
				,CF.DocumentStatusID
				,NULL As CallStatusID
				,CF.ModifiedOn
				,CASE CFSR.IsVoided WHEN 0 THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS IsVoided
				,NULL AS SignedOn
			FROM
				[Registration].[ContactForms] CF
				INNER JOIN [Registration].[vw_GetContactFormsServiceRecordingDetails] CFSR
					ON CFSR.ContactID=CF.ContactID AND CF.ContactFormsID=CFSR.SourceHeaderID
			WHERE 
				CF.ContactID = @ContactID
					AND CF.IsActive = 1
			AND ( ISNULL(Cast(CFSR.ServiceStartDate AS DATE),GETDATE()) BETWEEN @StartDate AND @EndDate)
		END

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END