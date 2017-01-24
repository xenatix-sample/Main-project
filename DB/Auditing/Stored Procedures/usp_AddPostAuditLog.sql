----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddPostAuditLog]
-- Author:		Scott Martin
-- Date:		1/26/2016
--
-- Purpose:		Logs the values of the current record after any changes
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/26/2016	Scott Martin		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Auditing].[usp_AddPostAuditLog]
	@AuditType NVARCHAR(50),
	@TableSchema NVARCHAR(255),
	@TableName NVARCHAR(255),
	@AuditDetailID BIGINT,
	@PrimaryKeyValue BIGINT,
	@ReasonText NVARCHAR(MAX) = NULL,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
		EXEC Auditing.usp_AddAuditLog 'Post', NULL, @AuditType, @TableSchema, @TableName, @AuditDetailID, @PrimaryKeyValue, @ReasonText, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @ID OUTPUT
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END