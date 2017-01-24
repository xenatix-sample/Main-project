-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetResource]
-- Author:		John Crossen
-- Date:		10/16/2015
--
-- Purpose:		Gets the list of Appointment Resource lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/16/2015	John Crossen	TFS# 2765 - Initial creation.
-- 10/29/2015	Rajiv Ranjan	Changed result set field name, corrected table join for resourceType 2 & 3
-- 11/03/2015	Rajiv Ranjan	Put space between firstname & lastname
-- 12/10/2015	Rajiv Ranjan	Corrected query to get resource availabity for room type resource
-- 12/10/2015	D. Christopher	Ensured that no duplicates are returned from the GetResources query for ResourceTypeID = 1
-- 12/11/2015	Rajiv Ranjan	Handled @facilityId, if it is null or zero
-- 01/15/2016   Satish Singh    @ResourceTypeID, @FacilityID  optional for where clause. Union in case of null ResourceTypeID. 1-Rooms, 2- Providers
-- 03/24/2016   Semalai Muthusamy  Inculded U.IsActive=1 condition for get only active user from Users Table 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_GetResource]
    @ResourceTypeID SMALLINT = NULL,
	@FacilityID INT = NULL,	
    @ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		
		IF @ResourceTypeID IN (2,3)
		BEGIN

		      SELECT 
				DISTINCT 
				U.UserID AS ResourceID, 
				@ResourceTypeID AS ResourceTypeID, 
				U.FirstName + ' ' +  U.LastName  AS ResourceName,
				F.FacilityID
              FROM 
				Scheduling.ResourceAvailability R 
				JOIN Core.Users U ON R.ResourceID=U.UserID             
				JOIN Reference.Facility F ON F.FacilityID=R.FacilityID
              WHERE 			
				 (ISNULL(@FacilityID,0) = 0 OR F.FacilityID = @FacilityID)
				AND (ISNULL(@ResourceTypeID,0)=0 OR R.ResourceTypeID = @ResourceTypeID) 
				AND R.IsActive=1 
				AND F.IsActive=1
				AND U.IsActive=1
		END
		
		IF @ResourceTypeID =1
		BEGIN
			SELECT
				RF.RoomID AS ResourceID, 
				@ResourceTypeID AS ResourceTypeID, 
				RF.RoomName AS ResourceName,
				RF.FacilityID
			FROM
				Scheduling.Room RF
			WHERE (ISNULL(@FacilityID,0) = 0 OR RF.FacilityID = @FacilityID)
			 AND RF.IsActive=1 and RF.IsSchedulable=1
		END

		IF(ISNULL(@ResourceTypeID,0) = 0)
		BEGIN
			 SELECT 
				DISTINCT 
				U.UserID AS ResourceID, 
				CAST(2 as SMALLINT) AS ResourceTypeID, 
				U.FirstName + ' ' +  U.LastName  AS ResourceName,
				F.FacilityID
              FROM 
				Scheduling.ResourceAvailability R 
				JOIN Core.Users U ON R.ResourceID=U.UserID             
				JOIN Reference.Facility F ON F.FacilityID=R.FacilityID
              WHERE 			
				 R.IsActive=1 and F.IsActive=1 AND U.IsActive=1

			UNION
			
			SELECT
				RF.RoomID AS ResourceID, 
				CAST(1 as SMALLINT) AS ResourceTypeID, 
				RF.RoomName AS ResourceName,
				RF.FacilityID
			FROM
				Scheduling.Room RF
			WHERE 
				RF.IsActive=1
		END
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
