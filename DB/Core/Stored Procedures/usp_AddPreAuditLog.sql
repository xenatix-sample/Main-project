----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddPreAuditLog]
-- Author:		Scott Martin
-- Date:		1/26/2016
--
-- Purpose:		Logs the values of the current record before any changes
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/26/2016	Scott Martin		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddPreAuditLog]
	@AuditSource NVARCHAR(255),
	@AuditType NVARCHAR(50),
	@TableSchema NVARCHAR(255),
	@TableName NVARCHAR(255),
	@PrimaryKeyValue BIGINT = NULL,
	@ReasonText NVARCHAR(MAX) = NULL,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
		EXEC Auditing.usp_AddAuditLog 'Pre', @AuditSource, @AuditType, @TableSchema, @TableName, NULL, @PrimaryKeyValue, @ReasonText, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @ID OUTPUT
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END