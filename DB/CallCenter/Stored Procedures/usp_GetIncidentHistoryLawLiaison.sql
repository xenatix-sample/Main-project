-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[CallCenter].[usp_GetIncidentHistoryLawLiaison]
-- Author:		Deepak Kumar
-- Date:		07/12/2016
--
-- Purpose:		Get a list of FollowupHistory by Header Id for Law Liaison
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/12/2016	Deepak Kumar	Initial creation.
-- 08/08/2016	Arun Choudhary 	Refactored proc to get IsCreatorAccess 
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [CallCenter].[usp_GetIncidentHistoryLawLiaison] 
    @CallCenterHeaderID BIGINT,
	@UserID INT,
    @ResultCode INT OUTPUT,
    @ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
    BEGIN TRY
 SELECT @ResultCode = 0,
  @ResultMessage = 'executed successfully'
  ;WITH CTE
  AS
  (
   SELECT 
    CallCenterHeaderID,
    ParentCallCenterHeaderID,
    ContactID, 
    0 AS HIERARCHY
   FROM 
    CallCenter.CallCenterHeader 
   WHERE 
    CallCenterHeaderID=@CallCenterHeaderID
   UNION ALL
   SELECT 
    CH1.CallCenterHeaderID,
    CH1.ParentCallCenterHeaderID,
    CH1.ContactID,  
    2 AS HIERARCHY
   FROM 
    CallCenter.CallCenterHeader CH
    JOIN CallCenter.CallCenterHeader CH1 
     ON CH1.CallCenterHeaderID=CH.ParentCallCenterHeaderID 
   WHERE 
    CH.CallCenterHeaderID=@CallCenterHeaderID
  )
 SELECT
  CLI.MRN,
  CH.CallCenterHeaderID,
  CH.ParentCallCenterHeaderID,
  CH.ContactID,
  CH.CallStartTime AS CallDate,
  CH.CallerID,
  CLR.LastName + ' ' + CLR.FirstName  AS [Caller],
  CH.ContactID,
  CLI.FirstName AS ClientFirstName, 
  CLI.LastName AS ClientLastName,
  CC.CallCenterPriorityID,
  CCP.CallCenterPriority AS Priority,
  CAST(CASE WHEN SRV.ServiceRecordingVoidID IS NOT NULL THEN 1 ELSE 0 END AS BIT) IsVoided,
  Case When USR.UserID=@UserID Then CAST(1 AS bit) Else CAST(0 AS bit) End IsCreatorAccess
 FROM
  CallCenter.CallCenterHeader CH 
  INNER JOIN CTE 
    ON CTE.CallCenterHeaderID=CH.CallCenterHeaderID AND CTE.ContactID=CH.ContactID
  LEFT JOIN Registration.Contact CLR
    ON CH.CallerID=CLR.ContactID
  LEFT JOIN CallCenter.CrisisCall CC
    ON CC.CallCenterHeaderID=CH.CallCenterHeaderID
  LEFT JOIN CallCenter.CallCenterPriority CCP
    ON CCP.CallCenterPriorityID=CC.CallCenterPriorityID
  LEFT JOIN Registration.Contact CLI
    ON CH.ContactID=CLI.ContactID
  LEFT JOIN [Core].[ServiceRecording] SR
    ON SR.SourceHeaderID=CH.CallCenterHeaderID
  LEFT OUTER JOIN Core.ServiceRecordingVoid SRV
    ON SR.ServiceRecordingID = SRV.ServiceRecordingID
  LEFT OUTER JOIN Core.Users USR
	ON USR.UserID=CH.ProviderID
 WHERE
  CH.IsActive=1
  AND CTE.ParentCallCenterHeaderID IS NULL
         
    END TRY
    BEGIN CATCH
            SELECT @ResultCode = ERROR_SEVERITY(),
                    @ResultMessage = ERROR_MESSAGE()
    END CATCH
END