-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AssignRolePermission]
-- Author:		Rajiv Ranjan
-- Date:		07/23/2015
--
-- Purpose:		Assign Module Permission and Feature Permission to Role
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Rajiv Ranjan		Initial creation.
-- 07/23/2015 - Justin Spalti -- Added the ModifiedBy parameter and included the value in crud operations
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-- 08/19/2015 - Rajiv Ranjan -- Implemented logic to add/delete permission rather than deactivate all permission and insert all permissions.
-- 08/19/2015 - Sumana Sangapu	-- Modified the code to use MERGE statement.
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedOn and CreatedBy fields
-- 02/24/2016	Scott Martin		Added audit logging
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AssignRolePermission]
	@rolePermission VARCHAR(MAX),
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @RoleID BIGINT,
		@ModifiedOn DATETIME,
		@IDOC INT,
		@AuditDetailID BIGINT,
		@AuditCursor CURSOR,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ID BIGINT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';
		   
	BEGIN TRY		
	EXEC sp_xml_preparedocument @IDOC OUTPUT, @rolePermission;

	SELECT
		@RoleID = RoleID,
		@ModifiedOn = ModifiedOn
	FROM
       OPENXML(@IDOC,'/Permissions',1) 
       WITH
       (
			RoleID VARCHAR(10) '@RoleID',
			ModifiedOn DATETIME '@ModifiedOn'
       );

	-- Insert or Update the Core.ModuleRolePermission  based on the existence
	DECLARE  @TempModuleRolePermission TABLE 
	(
		ModuleRolePermissionID BIGINT,
		RoleID BIGINT,
		ModuleID BIGINT,
		PermissionID BIGINT,
		ModifiedOn DATETIME,
		AuditDetailID BIGINT
	);

	INSERT INTO @TempModuleRolePermission 
	(
		RoleID,
		ModuleID,
		PermissionID,
		ModifiedOn
	) 
    SELECT 
		@RoleID,
		ModuleID,
		Permission,
		@ModifiedOn
	FROM
       OPENXML(@IDOC,'/Permissions/Modules/Module/Permission',2) 
       WITH
       (
		    ModuleID VARCHAR(10) '../@ModuleID',
		    Permission VARCHAR(10) '.'
       );

	UPDATE tMRP
	SET ModuleRolePermissionID = MRP.ModuleRolePermissionID
	FROM
		@TempModuleRolePermission tMRP
		INNER JOIN Core.ModuleRolePermission MRP
			ON tMRP.RoleID = MRP.RoleID
			AND tMRP.ModuleID = MRP.ModuleID
			AND tMRP.PermissionID = MRP.PermissionID
	WHERE
		MRP.RoleID = MRP.RoleID
		AND tMRP.ModuleID = MRP.ModuleID
		AND tMRP.PermissionID = MRP.PermissionID;

	--Inactivate any ModuleRolePermissions which don't have a record in the temp table. Create audit record.
	SET @AuditCursor = CURSOR FOR
	SELECT ModuleRolePermissionID FROM Core.ModuleRolePermission WHERE ModuleRolePermissionID NOT IN (SELECT ModuleRolePermissionID FROM @TempModuleRolePermission) AND RoleID = @RoleID AND IsActive = 1;    

	OPEN @AuditCursor 
	FETCH NEXT FROM @AuditCursor 
	INTO @ID

	WHILE @@FETCH_STATUS = 0
	BEGIN
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Core', 'ModuleRolePermission', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Core.ModuleRolePermission
	SET IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ModuleRolePermissionID = @ID
		AND IsActive = 1;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Core', 'ModuleRolePermission', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	FETCH NEXT FROM @AuditCursor 
	INTO @ID
	END; 

	CLOSE @AuditCursor;
	DEALLOCATE @AuditCursor;

	--Create audit record and save the original values of any ModuleRolePermissions that are being updated
	SET @AuditCursor = CURSOR FOR
	SELECT
		ModuleRolePermissionID
	FROM
		@TempModuleRolePermission  

	OPEN @AuditCursor 
	FETCH NEXT FROM @AuditCursor 
	INTO @ID

	WHILE @@FETCH_STATUS = 0
	BEGIN
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'ModuleRolePermission', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE @TempModuleRolePermission
	SET AuditDetailID = @AuditDetailID
	WHERE
		ModuleRolePermissionID = @ID;

	FETCH NEXT FROM @AuditCursor 
	INTO @ID
	END; 

	CLOSE @AuditCursor;
	DEALLOCATE @AuditCursor;

	DECLARE @MRP_ID TABLE (MergeAction NVARCHAR(10), ID BIGINT);

	MERGE Core.ModuleRolePermission AS TARGET
	USING @TempModuleRolePermission AS SOURCE
		ON SOURCE.ModuleID = TARGET.ModuleID 
		AND SOURCE.PermissionID = TARGET.PermissionID
		AND TARGET.RoleID = @RoleID
	WHEN NOT MATCHED BY TARGET THEN
		INSERT
		(
			RoleID,
			ModuleID,
			PermissionID,
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			@RoleID,
			ModuleID,
			PermissionID,
			1,
			@ModifiedBy,
			SOURCE.ModifiedOn,
			@ModifiedBy,
			SOURCE.ModifiedOn
		)
	--WHEN NOT MATCHED BY SOURCE AND TARGET.RoleID = @RoleID THEN
	--	UPDATE
	--	SET TARGET.IsActive = 0,
	--		TARGET.ModifiedBy = @ModifiedBy,
	--		TARGET.ModifiedOn = @ModifiedOn,
	--		TARGET.SystemModifiedOn = GETUTCDATE()
	WHEN MATCHED THEN
		UPDATE
		SET TARGET.IsActive = 1,
			TARGET.ModifiedBy = @ModifiedBy,
			TARGET.ModifiedOn = SOURCE.ModifiedOn,
			TARGET.SystemModifiedOn = GETUTCDATE()
	OUTPUT
		$action,
		inserted.ModuleRolePermissionID
	INTO
		@MRP_ID;

	IF EXISTS (SELECT TOP 1 * FROM @MRP_ID WHERE MergeAction = 'Insert')
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ID FROM @MRP_ID WHERE MergeAction = 'Insert';    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'ModuleRolePermission', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'ModuleRolePermission', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END

	IF EXISTS (SELECT TOP 1 * FROM @MRP_ID WHERE MergeAction = 'Update')
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT
			ModuleRolePermissionID,
			AuditDetailID
		FROM
			@TempModuleRolePermission 
		WHERE
			AuditDetailID IS NOT NULL

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @AuditDetailID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'ModuleRolePermission', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @AuditDetailID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END
	   
	-- Insert or Update the Core.FeatureRolePermission  based on the existence
	DECLARE  @TempFeatureRolePermission TABLE 
	(
		FeatureRolePermissionID BIGINT,
		RoleID BIGINT,
		FeatureID BIGINT,
		PermissionID BIGINT,
		ModifiedOn DATETIME,
		AuditDetailID BIGINT
	);

	INSERT INTO @TempFeatureRolePermission 
	(
		RoleID,
		FeatureID,
		PermissionID,
		ModifiedOn
	) 
    SELECT  
		@RoleID,
		FeatureID,
		Permission,
		@ModifiedOn
	FROM
       OPENXML(@IDOC,'/Permissions/Features/Feature/Permission',2) 
       WITH
       (
		   FeatureID VARCHAR(10) '../@FeatureID',
		   Permission VARCHAR(10) '.'
       );

	UPDATE tFRP
	SET FeatureRolePermissionID = FRP.FeatureRolePermissionID
	FROM
		@TempFeatureRolePermission tFRP
		INNER JOIN Core.FeatureRolePermission FRP
			ON tFRP.RoleID = FRP.RoleID
			AND tFRP.FeatureID = FRP.FeatureID
			AND tFRP.PermissionID = FRP.PermissionID
	WHERE
		tFRP.RoleID = FRP.RoleID
		AND tFRP.FeatureID = FRP.FeatureID
		AND tFRP.PermissionID = FRP.PermissionID;

	--Inactivate any FeatureRolePermissions which don't have a record in the temp table. Create audit record.
	SET @AuditCursor = CURSOR FOR
	SELECT FeatureRolePermissionID FROM Core.FeatureRolePermission WHERE FeatureRolePermissionID NOT IN (SELECT FeatureRolePermissionID FROM @TempFeatureRolePermission) AND RoleID = @RoleID AND IsActive = 1;    

	OPEN @AuditCursor 
	FETCH NEXT FROM @AuditCursor 
	INTO @ID

	WHILE @@FETCH_STATUS = 0
	BEGIN
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Core', 'FeatureRolePermission', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Core.FeatureRolePermission
	SET IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		FeatureRolePermissionID = @ID
		AND IsActive = 1;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Core', 'FeatureRolePermission', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	FETCH NEXT FROM @AuditCursor 
	INTO @ID
	END; 

	CLOSE @AuditCursor;
	DEALLOCATE @AuditCursor;

	--Create audit record and save the original values of any FeatureRolePermissions that are being updated
	SET @AuditCursor = CURSOR FOR
	SELECT
		FeatureRolePermissionID
	FROM
		@TempFeatureRolePermission  

	OPEN @AuditCursor 
	FETCH NEXT FROM @AuditCursor 
	INTO @ID

	WHILE @@FETCH_STATUS = 0
	BEGIN
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'FeatureRolePermission', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE @TempFeatureRolePermission
	SET AuditDetailID = @AuditDetailID
	WHERE
		FeatureRolePermissionID = @ID;

	FETCH NEXT FROM @AuditCursor 
	INTO @ID
	END; 

	CLOSE @AuditCursor;
	DEALLOCATE @AuditCursor;

	DECLARE @FRP_ID TABLE (MergeAction NVARCHAR(10), ID BIGINT);

	MERGE Core.FeatureRolePermission AS TARGET
	USING @TempFeatureRolePermission AS SOURCE
		ON SOURCE.FeatureID = TARGET.FeatureID 
		AND	SOURCE.PermissionID = TARGET.PermissionID
		AND TARGET.RoleID = @RoleID
	WHEN NOT MATCHED BY TARGET THEN
		INSERT
		(
			RoleID,
			FeatureID,
			PermissionID,
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			@RoleID,
			FeatureID,
			PermissionID,
			1,
			@ModifiedBy,
			SOURCE.ModifiedOn,
			@ModifiedBy,
			SOURCE.ModifiedOn
		)
	--WHEN NOT MATCHED BY SOURCE AND TARGET.RoleID = @RoleID THEN
	--	UPDATE
	--	SET TARGET.IsActive = 0,
	--		TARGET.ModifiedBy = @ModifiedBy,
	--		TARGET.ModifiedOn = @ModifiedOn,
	--		TARGET.SystemModifiedOn = GETUTCDATE()
	WHEN MATCHED THEN
		UPDATE
		SET TARGET.IsActive = 1,
			TARGET.ModifiedBy = @ModifiedBy,
			TARGET.ModifiedOn = SOURCE.ModifiedOn,
			TARGET.SystemModifiedOn = GETUTCDATE()
	OUTPUT
		$action,
		inserted.FeatureRolePermissionID
	INTO
		@FRP_ID;

	IF EXISTS (SELECT TOP 1 * FROM @FRP_ID WHERE MergeAction = 'Insert')
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ID FROM @FRP_ID WHERE MergeAction = 'Insert';    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'FeatureRolePermission', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'FeatureRolePermission', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END

	IF EXISTS (SELECT TOP 1 * FROM @FRP_ID WHERE MergeAction = 'Update')
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT
			FeatureRolePermissionID,
			AuditDetailID
		FROM
			@TempFeatureRolePermission 
		WHERE
			AuditDetailID IS NOT NULL

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @AuditDetailID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'FeatureRolePermission', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @AuditDetailID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END