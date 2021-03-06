-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddResourceOverrride]
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
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedBy and CreatedOn field
-- 02/17/2016	Scott Martin		Added audit logging
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE Scheduling.usp_AddResourceOverrride
	@ResourceID INT,
	@ResourceTypeID SMALLINT,
	@FacilityID INT,
	@OverrideDate DATE,
	@Comments NVARCHAR(4000),
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT,
	@ResourceOverrideID BIGINT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully';

	BEGIN TRY
	INSERT INTO [Scheduling].[ResourceOverrides]
	(
		[ResourceID],
		[ResourceTypeID],
		[OverrideDate],
		[Comments],
		[FacilityID],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@ResourceID,
		@ResourceTypeID,
		@OverrideDate,
		@Comments,
		@FacilityID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ResourceOverrideID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Scheduling', 'ResourceOverrides', @ResourceOverrideID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Scheduling', 'ResourceOverrides', @AuditDetailID, @ResourceOverrideID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;	 

	END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH

END