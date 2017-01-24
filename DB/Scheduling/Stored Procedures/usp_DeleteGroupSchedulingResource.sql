

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_DeleteGroupSchedulingResource]
-- Author:		John Crossen
-- Date:		03/11/2016
--
-- Purpose:		Insert for Appointment Status Detail
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/11/2016	John Crossen   7687	- Initial creation.

-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].usp_DeleteeGroupSchedulingResource
	@GroupResourceID BIGINT ,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS

BEGIN
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	BEGIN TRY
	

				EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Scheduling', 'GroupSchedulingResource', @GroupResourceID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
			
			
			UPDATE [Scheduling].[GroupSchedulingResource]
			SET 
			
			IsActive=0,
			ModifiedBy=@ModifiedBy ,
		    ModifiedOn =@ModifiedOn
			WHERE GroupResourceID=@GroupResourceID


				
			EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Scheduling', 'GroupSchedulingResource', @AuditDetailID, @GroupResourceID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			



	END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END