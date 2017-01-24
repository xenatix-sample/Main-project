
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateResourceOverrride]
-- Author:		John Crossen
-- Date:		10/05/2015
--
-- Purpose:		Add Resource Override details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/05/2015	John Crossen    TFS#2590		Initial creation.
-- 02/17/2016	Scott Martin		Added audit logging
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Scheduling.usp_UpdateResourceOverrride
	@ResourceOverrideID BIGINT,
	@ResourceID INT,
	@ResourceTypeID SMALLINT,
	@FacilityID INT,
	@OverrideDate DATE,
	@Comments NVARCHAR(4000),
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
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Scheduling', 'ResourceOverrides', @ResourceOverrideID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	UPDATE [Scheduling].[ResourceOverrides]
	SET [ResourceID] = @ResourceID,
		[ResourceTypeID] = @ResourceTypeID,
		[OverrideDate] = @OverrideDate,
		[Comments] = @Comments,
		[FacilityID] = @FacilityID,
		[ModifiedBy] = @ModifiedBy,
		[ModifiedOn] = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ResourceOverrideID = @ResourceOverrideID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Scheduling', 'ResourceOverrides', @AuditDetailID, @ResourceOverrideID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 

	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH

END