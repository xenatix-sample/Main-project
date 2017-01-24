-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[CallCenter].[usp_GetRelatedItemsHistoryLawLiaison]
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

CREATE PROCEDURE [CallCenter].[usp_GetRelatedItemsHistoryLawLiaison] 
    @CallCenterHeaderID BIGINT,
	@UserID INT,
    @ResultCode INT OUTPUT,
    @ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
    BEGIN TRY
 SELECT @ResultCode = 0,
  @ResultMessage = 'executed successfully'

 SELECT
   CH.CallCenterHeaderID,
   CH.ParentCallCenterHeaderID,
   CH.ContactID,
   SR.ServiceItemID,
   Case When SR.ServiceItemID IS NULL THEN 'Progress Note' ELSE S.ServiceName END AS ITEM,
   SR.ServiceTypeID,
   Case When SR.ServiceItemID IS NULL THEN 'Note' ELSE 'Service' END  AS TYPE,
   U.FirstName FirstName,
   U.LastName LastName,
   CAST(CASE WHEN SRV.ServiceRecordingVoidID IS NOT NULL THEN 1 ELSE 0 END AS BIT) IsVoided,
   CH.ModifiedOn AS DATE,
   Case When U.UserID=@UserID Then CAST(1 AS bit) Else CAST(0 AS bit) End IsCreatorAccess
 FROM 
  CallCenter.CallCenterHeader CH
  LEFT JOIN [Core].[ServiceRecording] SR
    ON SR.SourceHeaderID=CH.CallCenterHeaderID
  LEFT OUTER JOIN Core.ServiceRecordingVoid SRV
    ON SR.ServiceRecordingID = SRV.ServiceRecordingID
  LEFT JOIN Reference.Services S
    ON S.ServicesID=SR.ServiceItemID
  LEFT JOIN Core.Users U
    ON U.UserID=CH.ModifiedBy
 WHERE
  CH.IsActive = 1 AND
  CH.CallCenterHeaderID IN (SELECT CallCenterHeaderID FROM 
  (SELECT 
    CallCenterHeaderID,
    ParentCallCenterHeaderID,
    ContactID, 
    1 AS HIERARCHY
   FROM 
    CallCenter.CallCenterHeader 
   WHERE 
    ParentCallCenterHeaderID=@CallCenterHeaderID
   UNION ALL
   SELECT 
    CH1.CallCenterHeaderID,
    CH1.ParentCallCenterHeaderID,
    CH1.ContactID,  
    2 AS HIERARCHY
   FROM 
    CallCenter.CallCenterHeader CH
    JOIN CallCenter.CallCenterHeader CH1 
     ON CH1.ParentCallCenterHeaderID=CH.ParentCallCenterHeaderID 
   WHERE 
    CH.CallCenterHeaderID=@CallCenterHeaderID
    )A
  )
  
    END TRY
    BEGIN CATCH
            SELECT @ResultCode = ERROR_SEVERITY(),
                    @ResultMessage = ERROR_MESSAGE()
    END CATCH
END