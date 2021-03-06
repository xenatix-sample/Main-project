-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateGroupDetails]
-- Author:		Sumana Sangapu
-- Date:		02/10/2016
--
-- Purpose:		Update Group Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/10/2016	 Sumana Sangapu  Initial creation.
-- 03/25/2016    Justin Spalti   Added the GroupTypeID parameter
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_UpdateGroupDetails]
	@GroupDetailID bigint, 
	@GroupName nvarchar(100),
	@GroupCapacity smallint,
	@GroupTypeID INT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT 
AS

BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

  SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully'

  BEGIN TRY
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Scheduling', 'GroupDetails', @GroupDetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 		UPDATE	[Scheduling].[GroupDetails]  
		SET		GroupName = @GroupName, 
				GroupCapacity = @GroupCapacity, 
				GroupTypeID = @GroupTypeID,
				IsActive = 1, 
				ModifiedBy = @ModifiedBy, 
				ModifiedOn = @ModifiedOn, 
				SystemModifiedOn = GETUTCDATE() 
		WHERE	GroupDetailID = @GroupDetailID;
  
	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Scheduling', 'GroupDetails', @AuditDetailID, @GroupDetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
  
  END TRY

  BEGIN CATCH
    SELECT
      @ResultCode = ERROR_SEVERITY(),
      @ResultMessage = ERROR_MESSAGE()
  END CATCH

END