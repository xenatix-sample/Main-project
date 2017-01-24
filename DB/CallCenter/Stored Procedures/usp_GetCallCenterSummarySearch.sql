-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[CallCenter].[usp_GetCallCenterSummarySearch]
-- Author:		Gurpreet Singh
-- Date:		01/27/2016
--
-- Purpose:		Gets the search results for Call Center Summary
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/27/2016	Gurpreet Singh	- Initial creation.
-- 02/04/2016	Arun Choudhary	- Return ContactId
-- 02/08/2016	Arun Choudhary	- Added isactive check.
-- 02/16/2016	Rajiv Ranjan	- Added providerID into condition check
-- 03/02/2016   Gaurav Gupta    - Added Program Unit id in select
-- 03/16/2016	Scott Martin	- Added new params for UserID and Incoming/Outgoing views and modified the query to pull records for the user's program unit or if there is not program unit
-- 03/22/2016	Scott Martin	- Fixed an issue where call center records weren't being sorted correctly into Incoming/Outgoing. Fixed and issue where program unit was not searchable
-- 04/17/2016   Lokesh Singhal  - Get callcenterheaderid as incidentid
-- 04/21/2016   Lokesh Singhal  - Get IsVoided in select query
-- 05/17/2016   Lokesh Singhal  - Get phone detail through sub query to avoid duplicacy of record
-- 06/01/2016	Scott Martin	Adjusted query output to include additional fields
-- 06/26/2016	Gaurav Gupta	remove union all from void dup
-- 06/29/2016	Rajiv Ranjan	Store proc was faild to execute
-- 07/01/2016	Kyle Campbell	TFS 12133	Remove program unit filter for now until further clarification on requirements
-- 07/05/2016	Kyle Campbell	TFS 12186	Aliased CallEndTime as EndDate
-- 07/06/2016	Kyle Campbell	TFS 12193	Add MRN, IncidentID, and DOB to search fields
-- 07/16/2016	Deepak Kumar	Display parent's contact details to child (followup cases)
-- 07/19/2016	Deepak Kumar	Added HasChild
-- 07/29/2016	Arun Choudhary	Refactored proc to get IsManagerAccess,IsCreatorAccess.
-- 08/02/2016	Rajiv Ranjan	Added CalledDate into search pattern
-- 08/02/2016   Gurpreet Singh	Corrected join
-- 08/02/2016	Gurmant Singh	Remove the UserID check from the EntitySignatures table as to get data which is signed by any User
-- 09/06/2016	Deepak Kumar	Remove Parent Call Center Header id if Parent is InActive
-- 09/14/2016	Deepak Kumar	Change reference of ProgramUnitID
-- 09/14/2016	Gaurav Gupta	Update Service Recording Document Type ID and Sign Joined with Service Recording.
-- 10/05/2016	Scott Martin	Added a parameter to return Pending/Needs Review if Needs Review/Pending selected
-- 10/13/2016	Scott Martin	Removed previous changes. Updated the proc to return all records if Pending/Needs approval is selected
-- 10/14/2016	Scott Martin	Fixed an issue with unbound columns
-- 10/16/2016	Scott Martin	Fixed an issue where LL records weren't being returned
-- 11/22/2016	Scott Martin	Refactored proc to incorporate filtering based on UserID and current view (Summary/Approval). Refactored search functionality
-- 11/23/2016	Scott Martin	Moved input parameters into body of procedure until UI enhancements are finished
-- 11/28/2016	Gaurav Gupta	Added NatureofCall for Landing Screen of LL
-- 11/29/2016   Gaurav Gupta    Order by callstarttime and last 3 days data for default view.(17833)
-- 11/30/2016	Vishal Yadav	Getting Service End Date explicitly
-- 12/05/2016	Gaurav Gupta	Added duration column in select
-- 12/09/2016	Scott Martin	Merged old proc with update one from alter script in patch 1.1.4
-- 12/09/2016	Sumana Sangapu	Return CallerPhoneNumber based on IsPrimary and Effective and Expiration Dates
-- 12/13/2016	Scott Martin	Changed default date range for Crisis Line
-- 12/14/2016	Scott Martin	Duration was displaying incorrectly when there is not CallEndTime
-- 12/20/2016	Scott Martin	Duration was displaying incorrectly when CallEndTime was before CallStartTime
-- 12/29/2016	Scott Martin	Order by clause was not merged from 1.1.4 branch
-- 12/19/2016					Duration day was displaying incorrectly
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [CallCenter].[usp_GetCallCenterSummarySearch]
     @SearchCriteria VARCHAR(MAX) = '""',
     @UserID INT,
     @CallCenterTypeID INT,
     @CallStatusID INT,
     @SearchView BIT = 0, -- 0 = Summary; 1 = Approval
     @UserIDFilter INT = 0,
     @ResultCode INT OUTPUT,
     @ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
     SELECT @ResultCode = 0,
             @ResultMessage = 'executed successfully'

     BEGIN TRY

              DECLARE @ISMgrPermission bit,
                           @WordCount int,
                           @SearchCriteriaOR nvarchar(1000)

              SET @ISMgrPermission=(     
              SELECT 
                     DISTINCT
                     1 Result
              FROM
                     Core.UserRole UR
                     INNER JOIN Core.RoleModule RM
                     ON UR.RoleID = RM.RoleID
                     INNER JOIN Core.RoleModuleComponent RMC
                     ON RM.RoleModuleID = RMC.RoleModuleID
                     INNER JOIN Core.ModuleComponent MC
                     ON RMC.ModuleComponentID = MC.ModuleComponentID
                     INNER JOIN Core.RoleModuleComponentPermission RMCP
                     ON RMC.RoleModuleComponentID = RMCP.RoleModuleComponentID
                     INNER JOIN Core.Permission P 
                     ON P.PermissionID=RMCP.PermissionID
              WHERE
                     RM.IsActive = 1
                     AND RMC.IsActive = 1
                     AND RMCP.IsActive = 1
                     AND UR.UserID = @UserID
                     AND MC.DataKey = 'CrisisLine-CrisisLine-Approver'
                     AND P.Name = 'Update' 
                     AND UR.IsActive=1
                     AND P.IsActive=1
                     AND MC.IsActive=1
                     AND RMCP.PermissionLevelID IS NOT NULL
              )
              Set @ISMgrPermission=ISNULL(@ISMgrPermission,0)

        IF ISNULL(@SearchCriteria,'') = ''
                     BEGIN
                     SELECT
                           CH.CallCenterTypeID, 
                           CH.CallCenterHeaderID,
                           CASE WHEN PCH.IsActive = 1 THEN CH.ParentCallCenterHeaderID ELSE Null END ParentCallCenterHeaderID,
                           CH.CallerID,
                           ISNULL(PCH.ContactID, CH.ContactID) ContactID, 
                           CH.CallStartTime AS CallDate, 
                           CH.ProviderID,
                           CH.CallEndTime AS EndDate,
						   CASE WHEN CH.CallEndTime IS NOT NULL THEN 
						    CASE WHEN CAST(DATEDIFF(second, CH.CallStartTime, CH.CallEndTime) / 60 / 60 / 24 AS NVARCHAR(50)) ='0' Then '' Else CAST(DATEDIFF(second, CH.CallStartTime, CH.CallEndTime) / 60 / 60 / 24 AS NVARCHAR(50)) + 'day ' End+
							CAST(DATEDIFF(second, CH.CallStartTime, CH.CallEndTime) / 60 / 60 % 24 AS NVARCHAR(50)) + 'hr '+
							CAST(DATEDIFF(second, CH.CallStartTime, CH.CallEndTime) / 60 % 60 AS NVARCHAR(50)) + 'mins'
						   ELSE NULL END AS Duration,
                           CH.CallStatusID,
                           CCSRD.ProgramUnitID,
                           CCSRD.ProgramUnitName, 
                           CH.CountyID,
                           CC.CallCenterPriorityID, 
                           CC.SuicideHomicideID, 
                           CC.ReasonCalled, 
                           CC.Disposition, 
                           CC.OtherInformation,
                           CC.Comments,
                           CC.FollowUpRequired,
                           ISNULL(PCLR.LastName + ' ' + PCLR.FirstName, CLR.LastName + ' ' + CLR.FirstName) as [Caller],
							(SELECT Number FROM 
									(  SELECT  P.Number AS Number, ROW_NUMBER() OVER(PARTITION BY CP.ContactID ORDER BY CP.IsPrimary DESC, CP.ModifiedOn DESC) as RN, CP.ContactID as ContactID
										FROM Registration.ContactPhone CP
										INNER JOIN Core.Phone P
										ON CP.PhoneID = P.PhoneID
										WHERE CP.IsActive = 1 AND CP.ContactID =  (CASE WHEN ISNULL(CH.ContactID,0) = ISNULL(CH.CallerID,0) THEN CH.ContactID ELSE CH.CallerID END)
										AND	 (( GETDATE() BETWEEN ISNULL(EffectiveDate,GETDATE()) and ISNULL(ExpirationDate,GETDATE())) OR  ( GETDATE() >= ISNULL(EffectiveDate,GETDATE()) AND ExpirationDate IS NULL) ) 
										) Phone1
						   WHERE RN =1 ) as    CallerContactNumber, 
                           ISNULL(PC.MRN, CLI.MRN) AS MRN,
                           ISNULL(PC.FirstName, CLI.FirstName) ClientFirstName, 
                           ISNULL(PC.LastName, CLI.LastName) ClientLastName, 
                           CLI.DOB, 
                           CH.ModifiedBy, 
                           CH.ModifiedOn,
                           CS.CallStatus AS CallStatus, 
                           USR.FirstName + ' ' + USR.LastName  AS ProviderSubmittedBy, 
                           CCSRD.ServiceRecordingID,
                           CCSRD.ServicesID AS ServiceItemID, --ServiceID
                           CCSRD.ServiceStatusID,
                           CCSRD.ServiceTypeID,
						   CCSRD.ServiceEndDate,
                           NULL AS TrackingField,
                           SR.RecipientCodeID,
                           CCSRD.AttendanceStatusID,
                           CAST(ISNULL(CCSRD.IsVoided, 0) AS BIT) AS IsVoided,
                           CCSRD.SignatureDate AS SignedOn,
                           CAST(CASE WHEN CCSRD.SignatureDate IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS IsSignedByUser,
                           PN.NoteHeaderID AS NoteHeaderID,
                           Case When USR.UserID = @UserID Then CAST(1 AS bit) Else CAST(0 AS bit) End IsCreatorAccess,
                           @ISMgrPermission AS IsManagerAccess,
                           PN.NatureofCall,
                           CAST(LTRIM(STUFF((SELECT DISTINCT ',' + CONVERT(NVARCHAR(MAX), OSD.MappingID) FROM Registration.vw_ContactAdmissionDischarge vCAD INNER JOIN Core.vw_GetOrganizationStructureDetails OSD ON vCAD.ProgramUnitID = OSD.MappingID WHERE ContactID = ISNULL(PCH.ContactID, CH.ContactID) AND vCAD.DataKey = 'ProgramUnit' FOR XML PATH(''), ROOT ('List'), type).value('/List[1]','NVARCHAR(MAX)'), 1, 1, '')) AS NVARCHAR(MAX)) AS ContactProgramUnit,
                           CASE WHEN TotalChild > 0 THEN 1 ELSE 0 END HasChild    
                     FROM
                           CallCenter.CallCenterHeader CH 
                           LEFT JOIN CallCenter.CrisisCall CC
                                  ON CC.CallCenterHeaderID = CH.CallCenterHeaderID
                           LEFT JOIN Registration.Contact CLR
                                  ON CH.CallerID = CLR.ContactID
                           LEFT JOIN Registration.Contact CLI
                                  ON CH.ContactID = CLI.ContactID
                           LEFT OUTER JOIN Core.Users USR
                                  ON USR.UserID=CH.ProviderID 
                           LEFT OUTER JOIN CallCenter.CallStatus CS
                                  ON CH.CallStatusID = CS.CallStatusID
                           LEFT OUTER JOIN CallCenter.vw_GetCallCenterServiceRecordingDetails CCSRD
                                  ON CCSRD.SourceHeaderID = CH.CallCenterHeaderID
                                  AND CCSRD.ServiceRecordingSourceID IN (1, 6)
                           LEFT OUTER JOIN CallCenter.ProgressNote PN
                                  ON CH.CallCenterHeaderID = PN.CallCenterHeaderID
                           LEFT OUTER JOIN Core.ServiceRecording SR
                                  ON CCSRD.ServiceRecordingID = SR.ServiceRecordingID
                           LEFT OUTER JOIN 
                           (
                                  SELECT
                                         CH.ParentCallCenterHeaderID AS CallCenterHeaderID,
                                         COUNT(*) TotalChild
                                  FROM
                                         CallCenter.CallCenterHeader CH
                                  WHERE
                                         CH.ParentCallCenterHeaderID IS NOT NULL
                                  GROUP BY
                                         CH.ParentCallCenterHeaderID
                           ) Cnt
                                  ON CH.CallCenterHeaderID = Cnt.CallCenterHeaderID
                           LEFT JOIN CallCenter.CallCenterHeader PCH
                                  ON CH.ParentCallCenterHeaderID = PCH.CallCenterHeaderID
                           LEFT JOIN Registration.Contact PC
                                  ON PCH.ContactID = PC.ContactID
                           LEFT JOIN Registration.Contact PCLR
                                  ON PCH.CallerID = PCLR.ContactID
                     WHERE
                           (
                                  CH.IsActive = 1
                                  AND CH.CallCenterTypeID = @CallCenterTypeID
                                  AND @CallCenterTypeID = 2
                                  AND CAST(CH.CallStartTime AS DATE) BETWEEN DATEADD(d, -15, CAST(CURRENT_TIMESTAMP AS DATE)) AND CAST(CURRENT_TIMESTAMP AS DATE)
                                  AND (CH.ProviderID = @UserIDFilter OR ISNULL(@UserIDFilter, 0) = 0)
                           )
                           OR
                           (
                                  CH.IsActive = 1
                                  AND CH.CallCenterTypeID = @CallCenterTypeID
                                  AND @CallCenterTypeID = 1
                                  AND @SearchView = 1
                                  AND
                                  (
                                         (CH.CallStatusID = 5)
                                         OR (CH.CallStatusID = 3)
                                  )
                                  AND (CH.ProviderID = @UserIDFilter OR ISNULL(@UserIDFilter, 0) = 0)
                                  AND (CH.CallStatusID = @CallStatusID OR ISNULL(@CallStatusID, 0) = 0)
                           )
                           OR
                           (
                                  CH.IsActive = 1
                                  AND CH.CallCenterTypeID = @CallCenterTypeID
                                  AND @CallCenterTypeID = 1
                                  AND @SearchView = 0
                                  AND
                                  (
                                         (
                                                (CH.ProviderID = @UserIDFilter OR ISNULL(@UserIDFilter, 0) = 0)
                                                AND CAST(CH.CallStartTime AS DATE) BETWEEN DATEADD(d, -2, CAST(CURRENT_TIMESTAMP AS DATE)) AND CAST(CURRENT_TIMESTAMP AS DATE)
                                         )
                                         OR
                                         (
                                                (CH.ProviderID = @UserIDFilter OR ISNULL(@UserIDFilter, 0) = 0)
                                                AND (CH.CallStatusID IN (3, 5))
                                         )
                                         OR
                                         (
                                                CC.FollowUpRequired = 1
                                         )
                                  )
                                  AND (CH.CallStatusID = @CallStatusID OR ISNULL(@CallStatusID, 0) = 0)
                           )
                          ORDER BY CASE WHEN @CallCenterTypeID = 1 THEN CH.CallStartTime ELSE CH.ModifiedOn END DESC
                     END
              ELSE
                     BEGIN

                     -- Holds the words to look for
                           CREATE TABLE #WordsToLookUp
                           (
                                  Item NVARCHAR(255) PRIMARY KEY,
                                  SearchItem NVARCHAR(255)
                           );

                           -- Split the search criteria string into rows in a table
                           INSERT  INTO #WordsToLookUp (Item, SearchItem)
                           SELECT Item, '%'+ Item +'%' FROM  [Core].[fn_IterativeWordChop] (@SearchCriteria) WHERE Item IS NOT NULL AND Item <> '';

                           SELECT @WordCount = COUNT(*) FROM #WordsToLookUp;

                           -- Generate the string to facilitate partial searches
                           DECLARE @string TABLE ( c1 NVARCHAR(100))
              
                           INSERT INTO @string
                           SELECT CONCAT ('"', Items ,'*"') FROM Core.fn_Split (@SearchCriteria, ' ') 
                     
                           SELECT @SearchCriteriaOR = COALESCE(@SearchCriteriaOR + ' OR ' , '') + c1 FROM @string

                           --Check search values for dates
                           DECLARE @Item NVARCHAR(255),
                                         @SearchDate DATE;

                           DECLARE @Dates TABLE (SearchDate DATE);

                           DECLARE @DateCursor CURSOR;
                           SET @DateCursor = CURSOR FOR
                           SELECT Item FROM #WordsToLookUp;    

                           OPEN @DateCursor 
                           FETCH NEXT FROM @DateCursor 
                           INTO @Item

                           WHILE @@FETCH_STATUS = 0
                           BEGIN

                           BEGIN TRY
                           SET @SearchDate = CONVERT(DATE, @SearchCriteria, 101);

                           INSERT INTO @Dates VALUES(@SearchDate);
                           END TRY

                           BEGIN CATCH
                           FETCH NEXT FROM @DateCursor 
                           INTO @Item
                           END CATCH
       
                           FETCH NEXT FROM @DateCursor 
                           INTO @Item
                           END; 

                           CLOSE @DateCursor;
                           DEALLOCATE @DateCursor;

                     SELECT
                           CH.CallCenterTypeID, 
                           CH.CallCenterHeaderID,
                           CASE WHEN PCH.IsActive = 1 THEN CH.ParentCallCenterHeaderID ELSE Null END ParentCallCenterHeaderID,
                           CH.CallerID,
                           ISNULL(PCH.ContactID, CH.ContactID) ContactID, 
                           CH.CallStartTime AS CallDate, 
                           CH.ProviderID,
                           CH.CallEndTime AS EndDate,
						   CASE WHEN CH.CallEndTime IS NOT NULL THEN 
						   CASE WHEN CAST(DATEDIFF(second, CH.CallStartTime, CH.CallEndTime) / 60 / 60 / 24 AS NVARCHAR(50)) ='0' Then '' Else CAST(DATEDIFF(second, CH.CallStartTime, CH.CallEndTime) / 60 / 60 / 24 AS NVARCHAR(50)) + 'day ' End+
							CAST(DATEDIFF(second, CH.CallStartTime, CH.CallEndTime) / 60 / 60 % 24 AS NVARCHAR(50)) + 'hr '+
							CAST(DATEDIFF(second, CH.CallStartTime, CH.CallEndTime) / 60 % 60 AS NVARCHAR(50)) + 'mins'
						   ELSE NULL END AS Duration,
                           CH.CallStatusID,
                           CCSRD.ProgramUnitID,
                           CCSRD.ProgramUnitName, 
                           CH.CountyID,
                           CC.CallCenterPriorityID, 
                           CC.SuicideHomicideID, 
                           CC.ReasonCalled, 
                           CC.Disposition, 
                           CC.OtherInformation,
                           CC.Comments,
                           CC.FollowUpRequired,
                           ISNULL(PCLR.LastName + ' ' + PCLR.FirstName, CLR.LastName + ' ' + CLR.FirstName) as [Caller],
							(SELECT Number FROM 
									(  SELECT  P.Number AS Number, ROW_NUMBER() OVER(PARTITION BY CP.ContactID ORDER BY CP.IsPrimary DESC, CP.ModifiedOn DESC) as RN, CP.ContactID as ContactID
										FROM Registration.ContactPhone CP
										INNER JOIN Core.Phone P
										ON CP.PhoneID = P.PhoneID
										WHERE CP.IsActive = 1 AND CP.ContactID =  (CASE WHEN ISNULL(CH.IsLinkedToContact,0) = 1 THEN CH.ContactID ELSE CH.CallerID END)
										AND	 (( GETDATE() BETWEEN ISNULL(EffectiveDate,GETDATE()) and ISNULL(ExpirationDate,GETDATE())) OR  ( GETDATE() >= ISNULL(EffectiveDate,GETDATE()) AND ExpirationDate IS NULL) ) 
										) Phone1
						   WHERE RN =1 ) as    CallerContactNumber, 
                           ISNULL(PC.MRN, CLI.MRN) AS MRN,
                           ISNULL(PC.FirstName, CLI.FirstName) ClientFirstName, 
                           ISNULL(PC.LastName, CLI.LastName) ClientLastName, 
                           CLI.DOB, 
                           CH.ModifiedBy, 
                           CH.ModifiedOn,
                           CS.CallStatus AS CallStatus, 
                           USR.FirstName + ' ' + USR.LastName  AS ProviderSubmittedBy, 
                           CCSRD.ServiceRecordingID,
                           CCSRD.ServicesID AS ServiceItemID, --ServiceID
                           CCSRD.ServiceStatusID,
                           CCSRD.ServiceTypeID,
						   CCSRD.ServiceEndDate,
                           NULL AS TrackingField,
                           SR.RecipientCodeID,
                           CCSRD.AttendanceStatusID,
                           CAST(ISNULL(CCSRD.IsVoided, 0) AS BIT) AS IsVoided,
                           CCSRD.SignatureDate AS SignedOn,
                           CAST(CASE WHEN CCSRD.SignatureDate IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS IsSignedByUser,
                           PN.NoteHeaderID AS NoteHeaderID,
                           Case When USR.UserID = @UserID Then CAST(1 AS bit) Else CAST(0 AS bit) End IsCreatorAccess,
                           @ISMgrPermission AS IsManagerAccess,
                           PN.NatureofCall,
                           CAST(LTRIM(STUFF((SELECT DISTINCT ',' + CONVERT(NVARCHAR(MAX), OSD.MappingID) FROM Registration.vw_ContactAdmissionDischarge vCAD INNER JOIN Core.vw_GetOrganizationStructureDetails OSD ON vCAD.ProgramUnitID = OSD.MappingID WHERE ContactID = ISNULL(PCH.ContactID, CH.ContactID) AND vCAD.DataKey = 'ProgramUnit' FOR XML PATH(''), ROOT ('List'), type).value('/List[1]','NVARCHAR(MAX)'), 1, 1, '')) AS NVARCHAR(MAX)) AS ContactProgramUnit,
                           CASE WHEN TotalChild > 0 THEN 1 ELSE 0 END HasChild    
                     FROM
                           CallCenter.CallCenterHeader CH 
                           LEFT JOIN CallCenter.CrisisCall CC
                                  ON CC.CallCenterHeaderID = CH.CallCenterHeaderID
                           LEFT JOIN Registration.Contact CLR
                                  ON CH.CallerID = CLR.ContactID
                           LEFT JOIN Registration.Contact CLI
                                  ON CH.ContactID = CLI.ContactID
                           LEFT OUTER JOIN Core.Users USR
                                  ON USR.UserID=CH.ProviderID 
                           LEFT OUTER JOIN CallCenter.CallStatus CS
                                  ON CH.CallStatusID = CS.CallStatusID
                           LEFT OUTER JOIN CallCenter.vw_GetCallCenterServiceRecordingDetails CCSRD
                                  ON CCSRD.SourceHeaderID = CH.CallCenterHeaderID
                                  AND CCSRD.ServiceRecordingSourceID IN (1, 6)
                           LEFT OUTER JOIN CallCenter.ProgressNote PN
                                  ON CH.CallCenterHeaderID = PN.CallCenterHeaderID
                           LEFT OUTER JOIN Core.ServiceRecording SR
                                  ON CCSRD.ServiceRecordingID = SR.ServiceRecordingID
                           LEFT OUTER JOIN 
                           (
                                  SELECT
                                         CH.ParentCallCenterHeaderID AS CallCenterHeaderID,
                                         COUNT(*) TotalChild
                                  FROM
                                         CallCenter.CallCenterHeader CH
                                  WHERE
                                         CH.ParentCallCenterHeaderID IS NOT NULL
                                  GROUP BY
                                         CH.ParentCallCenterHeaderID
                           ) Cnt
                                  ON CH.CallCenterHeaderID = Cnt.CallCenterHeaderID
                           LEFT JOIN CallCenter.CallCenterHeader PCH
                                  ON CH.ParentCallCenterHeaderID = PCH.CallCenterHeaderID
                           LEFT JOIN Registration.Contact PC
                                  ON PCH.ContactID = PC.ContactID
                           LEFT JOIN Registration.Contact PCLR
                                  ON PCH.CallerID = PCLR.ContactID
                           LEFT OUTER JOIN @Dates D
                                  ON DATEDIFF(DAY, CH.CallStartTime, D.SearchDate) = 0
                     WHERE
                           CH.IsActive = 1
                           AND CH.CallCenterTypeID = @CallCenterTypeID
                           AND (CH.CallStatusID = @CallStatusID OR ISNULL(@CallStatusID, 0) = 0)
                           AND (CH.ProviderID = @UserIDFilter OR ISNULL(@UserIDFilter, 0) = 0)
                           AND
                           (
                                  CONTAINS(CLI.SearchableFields, @SearchCriteriaOR)
                                  OR CONTAINS(CLR.SearchableFields, @SearchCriteriaOR)
                                  OR CONTAINS(PC.SearchableFields, @SearchCriteriaOR)
                                  OR CONTAINS(PCLR.SearchableFields, @SearchCriteriaOR)
                                  OR CONTAINS(CC.SearchableFields, @SearchCriteriaOR)
                                  OR (CCSRD.ProgramUnitID IN
                                                (
                                                       SELECT
                                                              MappingID
                                                       FROM
                                                              Core.vw_GetOrganizationStructureDetails OSD
                                                              INNER JOIN #WordsToLookUp WLU
                                                                     ON OSD.Name LIKE WLU.SearchItem
                                                       WHERE
                                                              OSD.DataKey = 'ProgramUnit'
                                                ))
                                  OR D.SearchDate IS NOT NULL
                           )
                           ORDER BY CASE WHEN @CallCenterTypeID = 1 THEN CH.CallStartTime ELSE CH.ModifiedOn END DESC
                     DROP TABLE #WordsToLookUp
          END

     END TRY
     BEGIN CATCH
          SELECT @ResultCode = ERROR_SEVERITY(),
                 @ResultMessage = ERROR_MESSAGE()
     END CATCH
END
