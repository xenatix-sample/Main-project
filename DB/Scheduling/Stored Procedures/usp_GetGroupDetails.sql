
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetGroupDetails]
-- Author:		Sumana Sangapu
-- Date:		02/10/2016
--
-- Purpose:		Get Group Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/10/2016	 Sumana Sangapu  Initial creation.
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Scheduling].[usp_GetGroupDetails]
	@GroupDetailID BIGINT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS

BEGIN
  SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully'

  BEGIN TRY

 
		SELECT	GroupDetailID, GroupName, GroupCapacity, GroupTypeID,
				ModifiedBy, ModifiedOn
		FROM	Scheduling.GroupDetails g
		WHERE	GroupDetailID = @GroupDetailID
		AND		IsActive = 1 


  END TRY
  BEGIN CATCH
    SELECT
      @ResultCode = ERROR_SEVERITY(),
      @ResultMessage = ERROR_MESSAGE()
  END CATCH

END