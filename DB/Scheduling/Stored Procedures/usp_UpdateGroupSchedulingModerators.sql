
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateGroupSchedulingModerators]
-- Author:		Sumana Sangapu
-- Date:		02/10/2016
--
-- Purpose:		Update Group Scheduling Moderators
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/10/2016	 Sumana Sangapu  Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_UpdateGroupSchedulingModerators]
	@GroupModeratorID bigint,
	@GroupHeaderID bigint,
	@UserID int,
	@IsPrimary bit,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT,
	@ID BIGINT OUTPUT
AS

BEGIN
  SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully'

  BEGIN TRY
  
			UPDATE g
			SET		@GroupHeaderID = @GroupHeaderID,
					@UserID = @UserID
			FROM    [Scheduling].[GroupSchedulingModerators] g
			WHERE	g.GroupModeratorID = @GroupModeratorID

			SELECT @ID=SCOPE_IDENTITY()

  END TRY
  BEGIN CATCH
    SELECT
      @ResultCode = ERROR_SEVERITY(),
      @ResultMessage = ERROR_MESSAGE()
  END CATCH

END