-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetGroupSchedulingHeader]
-- Author:		Sumana Sangapu
-- Date:		02/10/2016
--
-- Purpose:		Get Group Scheduling Header
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/10/2016	 Sumana Sangapu  Initial creation.
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Scheduling].[usp_GetGroupSchedulingHeader]
	@GroupHeaderID BIGINT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS

BEGIN
  SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully'

  BEGIN TRY

 
		SELECT	GroupHeaderID, GroupDetailID,  Comments, ModifiedBy, ModifiedOn
		FROM	Scheduling.GroupSchedulingHeader g
		WHERE	GroupHeaderID = @GroupHeaderID
		AND		IsActive = 1 


  END TRY
  BEGIN CATCH
    SELECT
      @ResultCode = ERROR_SEVERITY(),
      @ResultMessage = ERROR_MESSAGE()
  END CATCH

END