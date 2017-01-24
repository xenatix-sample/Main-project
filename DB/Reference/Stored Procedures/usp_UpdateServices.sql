-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Reference.[usp_UpdateServices]
-- Author:		Kyle Campbell
-- Date:		01/05/2017
--
-- Purpose:		Update Services
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/05/2017	Kyle Campbell	TFS #14007	Initial Creation
-- 01/10/2017	Atul Chauhan	Removed ISActive and changed sort order
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_UpdateServices]
	@ServicesID INT,
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
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@AuditDetailID BIGINT;

	SELECT @ResultCode = 0,
		@ResultMessage = 'Executed successfully'

	BEGIN TRY

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Reference', 'Services', @ServicesID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Reference].[Services]
	SET
		ServiceName = @ServiceName,
		ServiceCode = @ServiceCode,
		ServiceConfigServiceTypeID = @ServiceConfigServiceTypeID,
		EffectiveDate = @EffectiveDate,
		ExpirationDate = @ExpirationDate,
		ExpirationReason = @ExpirationReason,
		EncounterReportable = @EncounterReportable,
		ServiceDefinition = @ServiceDefinition,
		Notes = @Notes,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn
	WHERE ServicesID = @ServicesID
           
	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Reference', 'Services', @AuditDetailID, @ServicesID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END