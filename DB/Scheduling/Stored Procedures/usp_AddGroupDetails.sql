-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddGroupDetails]
-- Author:		Sumana Sangapu
-- Date:		03/11/2016
--
-- Purpose:		Add Group Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/11/2016	 Sumana Sangapu  Initial creation.
-- 03/18/2016	 Sumana Sangapu  Added Audit Trail Code
-- 03/24/2016    Justin Spalti   Removed the GroupDetailID parameter and added the GroupTypeID parameter
-----------------------------------------------------------------------------------------------------------------------

 CREATE PROCEDURE [Scheduling].[usp_AddGroupDetails]
	@GroupName NVARCHAR(100),
	@GroupCapacity SMALLINT,
	@GroupTypeID INT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS

BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);
  SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully';

  BEGIN TRY
		  INSERT INTO [Scheduling].[GroupDetails]
		  (GroupName, GroupCapacity, GroupTypeID, IsActive, ModifiedBy, ModifiedOn, CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn  )
		  VALUES(@GroupName, @GroupCapacity, @GroupTypeID, 1, @ModifiedBy, @ModifiedOn, @ModifiedBy, @ModifiedOn, GETUTCDATE(), GETUTCDATE());

		  SELECT @ID = SCOPE_IDENTITY();

		  EXECUTE Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Scheduling', 'GroupDetails', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		  EXECUTE Auditing.usp_AddPostAuditLog 'Insert', 'Scheduling', 'GroupDetails', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;	 
  END TRY
  BEGIN CATCH
    SELECT
      @ResultCode = ERROR_SEVERITY(),
      @ResultMessage = ERROR_MESSAGE();
  END CATCH;

END;