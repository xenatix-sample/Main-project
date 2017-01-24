-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetRoomDetails]
-- Author:		John Crossen
-- Date:		09/11/2015
--
-- Purpose:		Gets the list of Room lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/11/2015	John Crossen	TFS# 2271 - Initial creation.
-- 01/15/2016   Satish Singh    @FacilityID  optional for where clause
-- 01/17/2016	D. Christopher	Ditto
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_GetRoomDetails]
    @FacilityID INT=NULL,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT	R.[RoomID], R.[RoomName],R.[FacilityID], R.RoomCapacity, R.IsSchedulable
		FROM [Scheduling].[Room] R
		WHERE ISNULL(@FacilityID,R.FacilityID) = R.FacilityID
		AND R.IsActive = 1
		ORDER BY [RoomName] ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


