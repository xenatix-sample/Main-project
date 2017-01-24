-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateRole]
-- Author:		Rajiv Ranjan
-- Date:		07/23/2015
--
-- Purpose:		Update Role details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Rajiv Ranjan		Initial creation.
-- 07/23/2015 - Justin Spalti - Added the ModifiedBy parameter and included the value in crud operations
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-- 9/23/2015    John Crossen                Add Audit
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateRole]
	@RoleID BIGINT,
	@Name NVARCHAR(100),
	@Description NVARCHAR(500),
	@EffectiveDate DATE,
	@ExpirationDate DATE,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);	

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'Role', @RoleID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Core.[Role]
	SET Name = @Name,
		[Description] = @Description,
		EffectiveDate = @EffectiveDate,
		ExpirationDate = @ExpirationDate,
		ModifiedBy = @ModifiedBy,			
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		RoleID = @RoleID

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'Role', @AuditDetailID, @RoleID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO


