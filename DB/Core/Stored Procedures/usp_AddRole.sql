-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddRole]
-- Author:		Rajiv Ranjan
-- Date:		07/23/2015
--
-- Purpose:		Add Role details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Rajiv Ranjan		Initial creation.
-- 07/23/2015 - Justin Spalti - Added the ModifiedBy parameter and included the value in crud operations
-- 08/03/2015 - Rajiv Ranjan - Added logic to check duplicate role
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-- 08/03/2015 - John Crossen -- Change @@Identity to Scope_Identity
-- 09/21/2015   John Crossen        Add Audit
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedOn and CreatedBy fields
-- 05/14/2016	Scott Martin		Removed RoleID and XML param; Added EffectiveDate and ExpirationDate
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddRole]
	@Name NVARCHAR(100),
	@Description NVARCHAR(500),
	@EffectiveDate DATE,
	@ExpirationDate DATE,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	IF EXISTS(SELECT 1 FROM Core.[Role] WHERE Name = @Name AND IsActive = 1)
		BEGIN
		--RAISERROR ('Role name can not be duplicate.', -- Message text.
		--	16, -- Severity.
		--	1 -- State.
		--	);
		SELECT @ResultCode = -1,
		  @ResultMessage = 'Role name can not be duplicate.'
		  SELECT @ID = 0
		END
		else
		begin
	INSERT INTO Core.[Role]
	(
		Name,
		[Description],
		EffectiveDate,
		ExpirationDate,
		ModifiedOn,
		ModifiedBy,
		IsActive,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@Name,
		@Description,
		@EffectiveDate,
		@ExpirationDate,
		@ModifiedOn,
		@ModifiedBy,
		1,
		@ModifiedBy,
		@ModifiedOn
	);
	
	SELECT @ID = SCOPE_IDENTITY();
	DECLARE @AuditDetailID BIGINT;

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'Role', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'Role', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	end
	

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END