-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[usp_SaveRoleModulePermissionDetails]
-- Author:		Scott Martin
-- Date:		05/14/2016
--
-- Purpose:		Insert, Update, Delete for Role Module Permissions
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/14/2016	Scott Martin	Initial Creation
-- 05/20/2016	Scott Martin	Addressed several bugs in saving permissions and separated Role Module from Role Module Component permissions
-- 05/23/2016	Scott Martin	Added an update to the RMCPermission table for RoleModuleComponentID after an insert into the RoleModuleComponent table; Fixed several issues with performance and deactivating records
-- 07/19/2016	Scott Martin	Fixed a small bug where the ID value wasn't being saved correctly into the Audit table for RoleModulePermission
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_SaveRoleModulePermissionDetails]
	@RoleModulePermissionsXML XML,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @ID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@AuditCursor CURSOR,
		@AuditDetailID BIGINT,
		@docHandle int,
		@RoleModuleID BIGINT,
		@PermissionLevelID INT,
		@PermissionID BIGINT,
		@ModuleComponentID BIGINT;

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	CREATE TABLE #RMPermission
	(	
		RoleModuleID BIGINT,
		RMPermissionLevelID INT,
		RoleModulePermissionID BIGINT,
		PermissionLevelID INT,
		PermissionID BIGINT
	);

	CREATE TABLE #RMCPermission
	(	
		RoleModuleID BIGINT,
		RoleModuleComponentID BIGINT,
		ModuleComponentID BIGINT,
		ReportID BIGINT,
		RMCPermissionLevelID INT,
		RoleModuleComponentPermissionID BIGINT,
		PermissionLevelID INT,
		PermissionID BIGINT
	);

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @RoleModulePermissionsXML;

	INSERT INTO #RMPermission
	(
		RoleModuleID,
		RMPermissionLevelID,
		RoleModulePermissionID,
		PermissionLevelID,
		PermissionID
	)
	SELECT
		RoleModuleID,
		RMPermissionLevelID,
		RoleModulePermissionID,
		PermissionLevelID,
		PermissionID
	FROM OPENXML(@docHandle, N'/Role/RoleModule/RoleModulePermissions/RoleModulePermission')
	WITH
	(
		RoleModuleID						BIGINT				'..//../RoleModuleID',
		RMPermissionLevelID					INT					'..//../RMPermissionLevelID',
		RoleModulePermissionID				BIGINT				'RoleModulePermissionID',
		PermissionLevelID					INT					'PermissionLevelID',
		PermissionID						BIGINT				'PermissionID'
	);

	INSERT INTO #RMCPermission
	(
		RoleModuleID,
		RoleModuleComponentID,
		ModuleComponentID,
		RMCPermissionLevelID,
		RoleModuleComponentPermissionID,
		PermissionLevelID,
		PermissionID
	)
	SELECT
		RoleModuleID,
		RoleModuleComponentID,
		ModuleComponentID,
		RMCPermissionLevelID,
		RoleModuleComponentPermissionID,
		PermissionLevelID,
		PermissionID
	FROM OPENXML(@docHandle, N'/Role/RoleModule/RoleModuleComponents/RoleModuleComponent/RoleModuleComponentPermissions/RoleModuleComponentPermission')
	WITH
	(
		RoleModuleID						BIGINT				'..//..//..//../RoleModuleID',
		RoleModuleComponentID				BIGINT				'..//../RoleModuleComponentID',
		ModuleComponentID					BIGINT				'..//../ModuleComponentID',
		ReportID							BIGINT				'..//../ReportID',
		RMCPermissionLevelID				INT					'..//../RMCPermissionLevelID',
		RoleModuleComponentPermissionID		BIGINT				'RoleModuleComponentPermissionID',
		PermissionLevelID					INT					'PermissionLevelID',
		PermissionID						BIGINT				'PermissionID'
	);

	UPDATE #RMPermission
	SET RoleModulePermissionID = RMP.RoleModulePermissionID
	FROM
		#RMPermission RM
		INNER JOIN Core.RoleModulePermission RMP
			ON RM.RoleModuleID = RMP.RoleModuleID
			AND RM.PermissionID = RMP.PermissionID
	WHERE
		RM.RoleModuleID = RMP.RoleModuleID
		AND RM.PermissionID = RMP.PermissionID;

	UPDATE #RMCPermission
	SET RoleModuleComponentID = RMC.RoleModuleComponentID
	FROM
		#RMCPermission RMCP
		INNER JOIN Core.RoleModuleComponent RMC
			ON RMCP.RoleModuleID = RMC.RoleModuleID
			AND RMCP.ModuleComponentID = RMC.ModuleComponentID
	WHERE
		RMCP.RoleModuleID = RMC.RoleModuleID
		AND RMCP.ModuleComponentID = RMC.ModuleComponentID;

	UPDATE #RMCPermission
	SET RoleModuleComponentPermissionID = RMCP.RoleModuleComponentPermissionID
	FROM
		#RMCPermission RMC
		INNER JOIN Core.RoleModuleComponentPermission RMCP
			ON RMC.RoleModuleComponentID = RMCP.RoleModuleComponentID
			AND RMC.PermissionID = RMCP.PermissionID
	WHERE
		RMC.RoleModuleComponentID = RMCP.RoleModuleComponentID
		AND RMC.PermissionID = RMCP.PermissionID;

	--Update Role Module Permission Level
	SET @AuditCursor = CURSOR FOR
	SELECT DISTINCT RoleModuleID, RMPermissionLevelID FROM #RMPermission WHERE RoleModuleID > 0;    

	OPEN @AuditCursor 
	FETCH NEXT FROM @AuditCursor 
	INTO @ID, @PermissionLevelID

	WHILE @@FETCH_STATUS = 0
	BEGIN
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'RoleModule', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Core.RoleModule
	SET PermissionLevelID = @PermissionLevelID,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		RoleModuleID = @ID
		AND IsActive = 1;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'RoleModule', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	FETCH NEXT FROM @AuditCursor 
	INTO @ID, @PermissionLevelID
	END; 

	CLOSE @AuditCursor;
	DEALLOCATE @AuditCursor;

	--Inactivate Role Module Permission
	IF EXISTS (SELECT TOP 1 RoleModulePermissionID FROM Core.RoleModulePermission RMP WHERE RoleModulePermissionID NOT IN (SELECT DISTINCT RoleModulePermissionID FROM #RMPermission) AND RoleModuleID IN (SELECT DISTINCT RoleModuleID FROM #RMPermission) AND IsActive = 1)
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT RoleModulePermissionID FROM Core.RoleModulePermission WHERE RoleModulePermissionID NOT IN (SELECT DISTINCT RoleModulePermissionID FROM #RMPermission) AND RoleModuleID IN (SELECT DISTINCT RoleModuleID FROM #RMPermission) AND IsActive = 1;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Core', 'RoleModulePermission', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Core.RoleModulePermission
		SET IsActive = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE
			RoleModulePermissionID = @ID
			AND IsActive = 1;

		EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Core', 'RoleModulePermission', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END

	--Update Role Module Permission
	IF EXISTS (SELECT TOP 1 RoleModulePermissionID, PermissionLevelID FROM #RMPermission P WHERE P.RoleModulePermissionID > 0)
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT RoleModulePermissionID, PermissionLevelID FROM #RMPermission P WHERE P.RoleModulePermissionID > 0;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @PermissionLevelID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'RoleModulePermission', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Core.RoleModulePermission
		SET PermissionLevelID = @PermissionLevelID,
			IsActive = 1,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE
			RoleModulePermissionID = @ID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'RoleModulePermission', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @PermissionLevelID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END

	--Insert Role Module Permission
	IF EXISTS (
		SELECT TOP 1
			P.RoleModuleID,
			P.PermissionLevelID,
			P.PermissionID
		FROM
			#RMPermission P
			LEFT OUTER JOIN Core.RoleModulePermission RMP
				ON P.RoleModuleID = RMP.RoleModuleID
				AND P.PermissionID = RMP.PermissionID
		WHERE
			ISNULL(RMP.RoleModulePermissionID, 0) = 0
		)
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT
			P.RoleModuleID,
			P.PermissionLevelID,
			P.PermissionID
		FROM
			#RMPermission P
			LEFT OUTER JOIN Core.RoleModulePermission RMP
				ON P.RoleModuleID = RMP.RoleModuleID
				AND P.PermissionID = RMP.PermissionID
		WHERE
			ISNULL(RMP.RoleModulePermissionID, 0) = 0;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @RoleModuleID, @PermissionLevelID, @PermissionID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		INSERT INTO Core.RoleModulePermission
		(
			RoleModuleID,
			PermissionLevelID,
			PermissionID,
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			@RoleModuleID,
			@PermissionLevelID,
			@PermissionID,
			1,
			@ModifiedBy,
			@ModifiedOn,
			@ModifiedBy,
			@ModifiedOn
		);

		SELECT @ID = SCOPE_IDENTITY();

		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'RoleModulePermission', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'RoleModulePermission', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @RoleModuleID, @PermissionLevelID, @PermissionID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END

	--Inactivate Role Module Component
	IF EXISTS (SELECT TOP 1 RoleModuleComponentID FROM Core.RoleModuleComponent WHERE RoleModuleComponentID NOT IN (SELECT DISTINCT RoleModuleComponentID FROM #RMCPermission) AND RoleModuleID IN (SELECT DISTINCT RoleModuleID FROM #RMCPermission) AND IsActive = 1)
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT RoleModuleComponentID FROM Core.RoleModuleComponent WHERE RoleModuleComponentID NOT IN (SELECT DISTINCT RoleModuleComponentID FROM #RMCPermission) AND RoleModuleID IN (SELECT DISTINCT RoleModuleID FROM #RMCPermission) AND IsActive = 1;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Core', 'RoleModuleComponent', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Core.RoleModuleComponent
		SET IsActive = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE
			RoleModuleComponentID = @ID
			AND IsActive = 1;

		EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Core', 'RoleModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END

	--Update Role Module Component
	IF EXISTS (SELECT TOP 1 RoleModuleComponentID, RMCPermissionLevelID FROM #RMCPermission P WHERE P.RoleModuleComponentID > 0)
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT RoleModuleComponentID, RMCPermissionLevelID FROM #RMCPermission P WHERE P.RoleModuleComponentID > 0;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @PermissionLevelID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'RoleModuleComponent', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Core.RoleModuleComponent
		SET PermissionLevelID = @PermissionLevelID,
			IsActive = 1,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE
			RoleModuleComponentID = @ID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'RoleModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @PermissionLevelID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END

	--Insert Role Module Component
	IF EXISTS (
		SELECT TOP 1
			P.RoleModuleID,
			P.ModuleComponentID,
			P.RMCPermissionLevelID
		FROM
			#RMCPermission P
			LEFT OUTER JOIN Core.RoleModuleComponent RMC
				ON P.RoleModuleID = RMC.RoleModuleID
				AND P.ModuleComponentID = RMC.ModuleComponentID
		WHERE
			ISNULL(RMC.RoleModuleComponentID, 0) = 0
		)
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT
			P.RoleModuleID,
			P.ModuleComponentID,
			P.RMCPermissionLevelID
		FROM
			#RMCPermission P
			LEFT OUTER JOIN Core.RoleModuleComponent RMC
				ON P.RoleModuleID = RMC.RoleModuleID
				AND P.ModuleComponentID = RMC.ModuleComponentID
		WHERE
			ISNULL(RMC.RoleModuleComponentID, 0) = 0;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @ModuleComponentID, @PermissionLevelID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		IF Not EXISTS (SELECT * FROM Core.RoleModuleComponent WHERE RoleModuleID = @ID and ModuleComponentID = @ModuleComponentID)
		Begin
		INSERT INTO Core.RoleModuleComponent
		(
			RoleModuleID,
			ModuleComponentID,
			PermissionLevelID,
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			@ID,
			@ModuleComponentID,
			@PermissionLevelID,
			1,
			@ModifiedBy,
			@ModifiedOn,
			@ModifiedBy,
			@ModifiedOn
		);
		END
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'RoleModuleComponent', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'RoleModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @ModuleComponentID, @PermissionLevelID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;

		UPDATE #RMCPermission
		SET RoleModuleComponentID = RMC.RoleModuleComponentID
		FROM
			#RMCPermission RMCP
			INNER JOIN Core.RoleModuleComponent RMC
				ON RMCP.RoleModuleID = RMC.RoleModuleID
				AND RMCP.ModuleComponentID = RMC.ModuleComponentID
		WHERE
			RMCP.RoleModuleID = RMC.RoleModuleID
			AND RMCP.ModuleComponentID = RMC.ModuleComponentID
			AND ISNULL(RMCP.RoleModuleComponentID, 0) = 0;
		END

	--Inactivate Role Module Component Permission
	IF EXISTS (SELECT TOP 1 RoleModuleComponentPermissionID FROM Core.RoleModuleComponentPermission RMCP INNER JOIN Core.RoleModuleComponent RMC ON RMC.RoleModuleComponentID = RMCP.RoleModuleComponentID WHERE RoleModuleComponentPermissionID NOT IN (SELECT DISTINCT RoleModuleComponentPermissionID FROM #RMCPermission) AND RoleModuleID IN (SELECT DISTINCT RoleModuleID FROM #RMCPermission) AND RMCP.IsActive = 1)
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT RoleModuleComponentPermissionID FROM Core.RoleModuleComponentPermission RMCP INNER JOIN Core.RoleModuleComponent RMC ON RMC.RoleModuleComponentID = RMCP.RoleModuleComponentID WHERE RoleModuleComponentPermissionID NOT IN (SELECT DISTINCT RoleModuleComponentPermissionID FROM #RMCPermission) AND RoleModuleID IN (SELECT DISTINCT RoleModuleID FROM #RMCPermission) AND RMCP.IsActive = 1;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Core', 'RoleModuleComponentPermission', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Core.RoleModuleComponentPermission
		SET IsActive = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE
			RoleModuleComponentPermissionID = @ID
			AND IsActive = 1;

		EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Core', 'RoleModuleComponentPermission', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END

	--Update Role Module Component Permission
	IF EXISTS (SELECT TOP 1 RoleModuleComponentPermissionID, P.PermissionLevelID FROM #RMCPermission P WHERE P.RoleModuleComponentPermissionID > 0)
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT RoleModuleComponentPermissionID, P.PermissionLevelID FROM #RMCPermission P WHERE P.RoleModuleComponentPermissionID > 0;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @PermissionLevelID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'RoleModuleComponentPermission', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Core.RoleModuleComponentPermission
		SET PermissionLevelID = @PermissionLevelID,
			IsActive = 1,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE
			RoleModuleComponentPermissionID = @ID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'RoleModuleComponentPermission', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @PermissionLevelID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END

	--Insert Role Module Component Permission
	IF EXISTS (
		SELECT TOP 1
			P.RoleModuleComponentID,
			P.PermissionLevelID,
			P.PermissionID
		FROM
			#RMCPermission P
			LEFT OUTER JOIN Core.RoleModuleComponentPermission RMCP
				ON P.RoleModuleComponentID = RMCP.RoleModuleComponentID
				AND P.PermissionID = RMCP.PermissionID
		WHERE
			RMCP.RoleModuleComponentPermissionID IS NULL
		)
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT
			P.RoleModuleComponentID,
			P.PermissionLevelID,
			P.PermissionID
		FROM
			#RMCPermission P
			LEFT OUTER JOIN Core.RoleModuleComponentPermission RMCP
				ON P.RoleModuleComponentID = RMCP.RoleModuleComponentID
				AND P.PermissionID = RMCP.PermissionID
		WHERE
			RMCP.RoleModuleComponentPermissionID IS NULL;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @PermissionLevelID, @PermissionID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		INSERT INTO Core.RoleModuleComponentPermission
		(
			RoleModuleComponentID,
			PermissionLevelID,
			PermissionID,
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			@ID,
			@PermissionLevelID,
			@PermissionID,
			1,
			@ModifiedBy,
			@ModifiedOn,
			@ModifiedBy,
			@ModifiedOn
		);

		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'RoleModuleComponentPermission', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'RoleModuleComponentPermission', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @PermissionLevelID, @PermissionID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END

	EXEC sp_xml_removedocument @docHandle;
	DROP TABLE #RMPermission;
	DROP TABLE #RMCPermission;
  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END