-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[CallCenter].[usp_GetFollowupHistory]
-- Author:		Deepak Kumar
-- Date:		06/17/2016
--
-- Purpose:		Get a list of FollowupHistory by contact
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/06/2016	Deepak Kumar	Initial creation.
-- 06/29/2016	Rajiv Ranjan	Refactored proc to get incident history.
-- 07/27/2016	Arun Choudhary 	Refactored proc to get ProviderSubmittedBy,ProgramUnitId,IsManagerAccess,IsCreatorAccess and Included all clild all branches
-- 08/04/2016	Deepak Kumar	Removed Contact filter and apply IsActive filter
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [CallCenter].[usp_GetFollowupHistory]
    @CallCenterHeaderID BIGINT,
	@UserID INT,
    @ResultCode INT OUTPUT,
    @ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
    BEGIN TRY
	SELECT @ResultCode = 0,
		@ResultMessage = 'executed successfully'
		Declare @ParentHeaderID Bigint
		 DECLARE @ISMgrPermission bit
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

		;WITH CTEParent
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
				CallCenterHeaderID=@CallCenterHeaderId AND IsActive=1
			UNION ALL
			SELECT 
				CH.CallCenterHeaderID,
				CH.ParentCallCenterHeaderID,
				CH.ContactID,
				CTE.HIERARCHY-1
			FROM 
				CallCenter.CallCenterHeader CH 
				INNER JOIN CTEParent CTE ON CTE.ParentCallCenterHeaderID=CH.CallCenterHeaderID 
					AND CH.CallCenterHeaderID<CTE.CallCenterHeaderID
					AND CTE.HIERARCHY!>0
					AND CH.IsActive=1
			)
			Select @ParentHeaderID= CallCenterHeaderID From CTEParent Where ParentCallCenterHeaderID is null
		
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
				CallCenterHeaderID=@ParentHeaderID AND IsActive=1
			UNION ALL
			SELECT 
				CH.CallCenterHeaderID,
				CH.ParentCallCenterHeaderID,
				CH.ContactID,
				CTE.HIERARCHY+1
			FROM 
				CallCenter.CallCenterHeader CH 
				INNER JOIN CTE ON CTE.CallCenterHeaderID=CH.ParentCallCenterHeaderID 
					AND CTE.ContactID=CH.ContactID 
					AND CH.CallCenterHeaderID>CTE.CallCenterHeaderID
					AND CTE.HIERARCHY!<0
					AND CH.IsActive=1
			
		)
	
	SELECT
		CLI.MRN,
		CH.CallCenterHeaderID,
		CH.CallStartTime AS CallDate,
		CH.CallerID,
		CLR.LastName + ' ' + CLR.FirstName  AS [Caller],
		CH.ContactID,
		CLI.FirstName AS ClientFirstName, 
		CLI.LastName AS ClientLastName,
		USR.FirstName + ' ' + USR.LastName  AS ProviderSubmittedBy, 
		CH.ProgramUnitID,
		Case When USR.UserID=@UserID Then CAST(1 AS bit) Else CAST(0 AS bit) End IsCreatorAccess,
		@ISMgrPermission AS  IsManagerAccess
	FROM
		CallCenter.CallCenterHeader CH 
		INNER JOIN CTE 
				ON CTE.CallCenterHeaderID=CH.CallCenterHeaderID --AND CTE.ContactID=CH.ContactID
		LEFT JOIN Registration.Contact CLR
				ON CH.CallerID=CLR.ContactID AND CLR.IsActive=1
		LEFT JOIN Registration.Contact CLI
				ON CH.ContactID=CLI.ContactID AND CLI.IsActive=1
		LEFT OUTER JOIN Core.Users USR
                ON USR.UserID=CH.ProviderID AND USR.IsActive=1
	WHERE
		CH.IsActive=1
         
    END TRY
    BEGIN CATCH
            SELECT @ResultCode = ERROR_SEVERITY(),
                    @ResultMessage = ERROR_MESSAGE()
    END CATCH
END