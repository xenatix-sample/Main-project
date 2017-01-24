-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddUserDirectReport]
-- Author:		Scott Martin
-- Date:		02/26/2016
--
-- Purpose:		Add a direct report mapping  
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/26/2016	Scott Martin		Initial Creation
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddUserDirectReport]
	@UserID INT,
	@ParentID INT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID INT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@MaxSupervisor INT,
		@MappingID BIGINT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
	SET @MaxSupervisor = (SELECT CAST(SV.Value AS INT) FROM Core.Settings S INNER JOIN Core.SettingValues SV ON S.SID = SV.SID WHERE S.Settings = 'MaxSupervisors');

	IF (SELECT COUNT(*) FROM Core.UsersHierarchyMapping WHERE UserID = @UserID AND IsActive = 1) < @MaxSupervisor
		BEGIN
		IF NOT EXISTS (SELECT TOP 1 * FROM Core.UsersHierarchyMapping WHERE UserID = @UserID AND ParentID = @ParentID)
			BEGIN
			INSERT INTO [Core].[UsersHierarchyMapping]
			(
				UserID,
				ParentID,
				IsActive,
				ModifiedBy,
				ModifiedOn,
				CreatedBy,
				CreatedOn
			)
			VALUES
			(
				@UserID,
				@ParentID,
				1,
				@ModifiedBy,
				@ModifiedOn,
				@ModifiedBy,
				@ModifiedOn
			);

			SELECT @ID = SCOPE_IDENTITY();

			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'UsersHierarchyMapping', @ID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'UsersHierarchyMapping', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END

		IF EXISTS (SELECT TOP 1 * FROM Core.UsersHierarchyMapping WHERE UserID = @UserID AND ParentID = @ParentID AND IsActive = 0)
			BEGIN
			SET @ID = (SELECT TOP 1 MappingID FROM Core.UsersHierarchyMapping WHERE UserID = @UserID AND ParentID = @ParentID AND IsActive = 0);

			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'UsersHierarchyMapping', @ID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			UPDATE Core.UsersHierarchyMapping
			SET IsActive = 1,
				ModifiedBy = @ModifiedBy,
				ModifiedOn = @ModifiedOn,
				SystemModifiedOn = GETUTCDATE()
			WHERE
				MappingID = @ID;

			EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'UsersHierarchyMapping', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END
		END
	ELSE
		BEGIN
		SELECT @ResultCode = -1,
				@ResultMessage = 'user has reached maximum supervisor limit';
		END
		  
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END