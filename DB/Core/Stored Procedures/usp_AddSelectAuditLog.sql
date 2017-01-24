----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddSelectAuditLog]
-- Author:		Scott Martin
-- Date:		1/26/2016
--
-- Purpose:		Logs the values of the data being selected
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/26/2016	Scott Martin		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddSelectAuditLog]
	@AuditType NVARCHAR(50),
	@TableSchema NVARCHAR(255),
	@TableName NVARCHAR(255),
	@PrimaryKeyValue BIGINT,
	@SelectQuery NVARCHAR(MAX),
	@ModifiedBy INT,
	@ModifiedOn DATETIME,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
		EXEC Core.usp_AddAuditLog NULL, 'Pre', @AuditType, @TableSchema, @TableName, NULL, @PrimaryKeyValue, NULL, @SelectQuery,  @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @ID OUTPUT
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END