-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AssignModuleToRole]
-- Author:		Rajiv Ranjan
-- Date:		07/23/2015
--
-- Purpose:		Assign Module to Role
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Rajiv Ranjan		Initial creation.
-- 07/23/2015 - Justin Spalti - Added the ModifiedBy parameter and included the value in crud operations
-- 08/03/2015 - Rajiv Ranjan - Added check for ModuleID is not zero while assigning module to role
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-- 09/07/2015 - Rajiv Ranjan -- Used merge statement to inset, update 
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedOn and CreatedBy fields
-- 02/24/2016	Scott Martin		Added audit logging
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AssignModuleToRole]
	@dataFilter VARCHAR(MAX),
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @RoleID BIGINT,
		@AuditDetailID BIGINT,
		@AuditCursor CURSOR,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ID BIGINT,
		@IDOC INT,
		@ModifiedOn DATETIME;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY   
    EXEC sp_xml_preparedocument @IDOC OUTPUT, @dataFilter;

	SELECT
		@RoleID = RoleID
	FROM
		OPENXML(@IDOC,'/RoleModule', 1) 
    WITH
    (
		RoleID VARCHAR(10) '@RoleID'
    );
      
	DECLARE  @TempRoleModule TABLE
	(
		RoleModuleID BIGINT,
		RoleID BIGINT,
		ModuleID BIGINT,
		ModifiedOn DATETIME,
		AuditDetailID BIGINT
	);

    INSERT INTO @TempRoleModule
	(
		RoleID,
		ModuleID,
		ModifiedOn
	) 
    SELECT
		@RoleID, 			
		ModuleID,
		ModifiedOn  
	FROM
		OPENXML(@IDOC,'/RoleModule/Modules/Module', 1) 
    WITH
    (		   
		ModuleID VARCHAR(10) '@ModuleID',
		ModifiedOn DATETIME '@ModifiedOn'
    );

	UPDATE @TempRoleModule
	SET RoleModuleID = RM.RoleModuleID
	FROM
		@TempRoleModule tRM
		INNER JOIN Core.RoleModule RM
			ON tRM.ModuleID = RM.ModuleID
			AND tRM.RoleID = RM.RoleID
	WHERE
		tRM.ModuleID = RM.ModuleID
		AND tRM.RoleID = RM.RoleID;

	SELECT TOP 1 @ModifiedOn = ModifiedOn FROM @TempRoleModule;

	--Inactivate any RoleModules which don't have a record in the temp table. Create audit record.
	SET @AuditCursor = CURSOR FOR
	SELECT RoleModuleID FROM Core.RoleModule WHERE RoleModuleID NOT IN (SELECT RoleModuleID FROM @TempRoleModule) AND RoleID = @RoleID AND IsActive = 1;    

	OPEN @AuditCursor 
	FETCH NEXT FROM @AuditCursor 
	INTO @ID

	WHILE @@FETCH_STATUS = 0
	BEGIN
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Core', 'RoleModule', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Core.RoleModule
	SET IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		RoleModuleID = @ID
		AND IsActive = 1;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Core', 'RoleModule', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	FETCH NEXT FROM @AuditCursor 
	INTO @ID
	END; 

	CLOSE @AuditCursor;
	DEALLOCATE @AuditCursor;

	--Create audit record and save the original values of any ModuleRolePermissions that are being updated
	SET @AuditCursor = CURSOR FOR
	SELECT
		RoleModuleID
	FROM
		@TempRoleModule  

	OPEN @AuditCursor 
	FETCH NEXT FROM @AuditCursor 
	INTO @ID

	WHILE @@FETCH_STATUS = 0
	BEGIN
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'RoleModule', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE @TempRoleModule
	SET AuditDetailID = @AuditDetailID
	WHERE
		RoleModuleID = @ID;

	FETCH NEXT FROM @AuditCursor 
	INTO @ID
	END; 

	CLOSE @AuditCursor;
	DEALLOCATE @AuditCursor;
	   
	DECLARE @RM_ID TABLE (MergeAction NVARCHAR(10), ID BIGINT);

	MERGE Core.RoleModule AS TARGET
	USING @TempRoleModule AS SOURCE
		ON SOURCE.ModuleID = TARGET.ModuleID 
		AND TARGET.RoleID = @RoleID
	WHEN NOT MATCHED BY TARGET THEN
		INSERT
		(
			RoleID,
			ModuleID,
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
	--		TARGET.ModifiedOn = GetUTCDATE(),
	--		TARGET.SystemModifiedOn = GETUTCDATE()
	WHEN MATCHED THEN
		UPDATE
		SET TARGET.IsActive = 1,
			TARGET.ModifiedBy = @ModifiedBy,
			TARGET.ModifiedOn = SOURCE.ModifiedOn,
			TARGET.SystemModifiedOn = GETUTCDATE()
	OUTPUT
		$action,
		inserted.RoleModuleID
	INTO
		@RM_ID;
		
	IF EXISTS (SELECT TOP 1 * FROM @RM_ID WHERE MergeAction = 'Insert')
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ID FROM @RM_ID WHERE MergeAction = 'Insert';    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'RoleModule', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'RoleModule', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END

	IF EXISTS (SELECT TOP 1 * FROM @RM_ID WHERE MergeAction = 'Update')
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT
			RoleModuleID,
			AuditDetailID
		FROM
			@TempRoleModule 
		WHERE
			AuditDetailID IS NOT NULL

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @AuditDetailID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'RoleModule', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

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

GO


