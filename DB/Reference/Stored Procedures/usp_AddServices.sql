-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Reference.[usp_AddServices]
-- Author:		Kyle Campbell
-- Date:		01/05/2017
--
-- Purpose:		Add Services
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/05/2017	Kyle Campbell	TFS #14007	Initial Creation
-- 01/10/2017	Atul Chauhan	Removed ISActive and changed sort order
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_AddServices]
	@ServiceName NVARCHAR(255),
	@ServiceCode NVARCHAR(20),
	@ServiceConfigServiceTypeID INT NULL,
	@EffectiveDate DATE NULL,
	@ExpirationDate DATE NULL,
	@ExpirationReason NVARCHAR(100) NULL,
	@EncounterReportable BIT,
	@ServiceDefinition NVARCHAR(1000),
	@Notes NVARCHAR(MAX),
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
	DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'Executed successfully'

	BEGIN TRY
	INSERT INTO [Reference].[Services]
	(
		ServiceName,
		ServiceCode,
		ServiceConfigServiceTypeID,
		EffectiveDate,
		ExpirationDate,
		ExpirationReason,
		EncounterReportable,
		ServiceDefinition,
		Notes,
		IsInternal,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@ServiceName,
		@ServiceCode,
		@ServiceConfigServiceTypeID,
		@EffectiveDate,
		@ExpirationDate,
		@ExpirationReason,
		@EncounterReportable,
		@ServiceDefinition,
		@Notes,
		0,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();
           
	DECLARE @AuditDetailID BIGINT;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Reference', 'Services', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Reference', 'Services', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END