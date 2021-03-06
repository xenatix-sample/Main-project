
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateGroupSchedulingHeader]
-- Author:		Sumana Sangapu
-- Date:		02/10/2016
--
-- Purpose:		Update Group Scheduling Header
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/10/2016	 Sumana Sangapu  Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_UpdateGroupSchedulingHeader]
	@GroupHeaderID bigint, 
	@Comments nvarchar(1000),
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
  	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Scheduling', 'GroupDetails', @GroupHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 		UPDATE	[Scheduling].[GroupSchedulingHeader]  
		SET		Comments= @Comments, 
				IsActive = 1, 
				ModifiedBy = @ModifiedBy, 
				ModifiedOn = @ModifiedOn, 
				SystemModifiedOn =  GETUTCDATE() 
		WHERE GroupHeaderID = @GroupHeaderID
	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Scheduling', 'GroupDetails', @AuditDetailID, @GroupHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

  
  END TRY
  BEGIN CATCH
    SELECT
      @ResultCode = ERROR_SEVERITY(),
      @ResultMessage = ERROR_MESSAGE()
  END CATCH

END