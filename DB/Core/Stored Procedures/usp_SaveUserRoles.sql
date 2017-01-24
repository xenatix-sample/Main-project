-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_SaveUserRoles
-- Author:		Justin Spalti
-- Date:		02/10/2016
--
-- Purpose:		This will update the user's recod as well as merging in their assigned roles
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/10/2016 - Justin Spalti - Initial Creation
-- 02/24/2016 - Justin Spalti - Added the new audit logging logic
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-- 01/13/2017	Scott Martin	Fixed an issue where not all of the required parameters were being passed into Audit proc
-- 01/19/2017	Scott Martin	Fixed an issue where not all of the required parameters were being passed into Audit proc
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_SaveUserRoles]
	@UserID INT,
	@RolesXMLValue XML,	
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT

AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'User updated successfully'
		   --<RolesXMLValue><RoleID>1</RoleID><RoleID>2</RoleID></RolesXMLValue>

	BEGIN TRY			
		DECLARE @ID BIGINT,
				@RoleID BIGINT,
				@AuditDetailID BIGINT,
				@AuditCursor CURSOR,
				@EmailXML XML,
				@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

		DECLARE @UR TABLE
		(
			UserRoleID BIGINT,
			RoleID BIGINT,
			UserID BIGINT,
			AuditDetailID BIGINT
		);

		INSERT INTO @UR(UserRoleID, RoleID, UserID, AuditDetailID)
		SELECT
			UR.UserRoleID,
			T.C.value('(.)[1]','int') AS RoleID,
			UR.UserID,
			NULL
		FROM
			@RolesXMLValue.nodes('RolesXMLValue/RoleID') AS T(C)
			FULL OUTER JOIN (SELECT * FROM Core.UserRole WHERE UserID = @UserID) AS UR
				ON T.C.value('(.)[1]','int') = UR.RoleID;

		IF EXISTS (SELECT TOP 1 * FROM @UR WHERE UserRoleID IS NOT NULL)
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT UserRoleID, RoleID FROM @UR WHERE UserRoleID IS NOT NULL;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @RoleID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		IF @RoleID IS NOT NULL
			BEGIN
			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'UserRole', @ID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END
		ELSE
			BEGIN
			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Core', 'UserRole', @ID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END

		UPDATE @UR
		SET AuditDetailID = @AuditDetailID
		WHERE
			UserRoleID = @ID;
		
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @RoleID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END;

		DECLARE @UR_ID TABLE
		(
			Operation NVARCHAR(12),
			ID BIGINT
		);

		MERGE Core.UserRole AS TARGET
		USING (SELECT * FROM @UR WHERE RoleID IS NOT NULL) AS SOURCE
			ON SOURCE.RoleID = TARGET.RoleID
			AND TARGET.UserID = @UserID
		WHEN NOT MATCHED BY TARGET THEN
			INSERT
			(
				UserID,
				RoleID,
				ModifiedOn,
				ModifiedBy,
				IsActive,
				CreatedBy,
				CreatedOn
			)
			VALUES
			(
				@UserID,
				SOURCE.RoleID,
				@ModifiedOn,
				@ModifiedBy,
				1,
				@ModifiedBy,
				@ModifiedOn
			)
		WHEN NOT MATCHED BY SOURCE AND TARGET.UserID = @UserID THEN
			UPDATE
			SET TARGET.IsActive = 0,
				TARGET.ModifiedBy = @ModifiedBy,
				TARGET.ModifiedOn = @ModifiedOn,
				TARGET.SystemModifiedOn = GETUTCDATE()
		WHEN MATCHED THEN
			UPDATE
			SET TARGET.IsActive = 1,
				TARGET.ModifiedBy = @ModifiedBy,
				TARGET.ModifiedOn = @ModifiedOn,
				TARGET.SystemModifiedOn = GETUTCDATE()
		OUTPUT
			$action,
			inserted.UserRoleID
		INTO
			@UR_ID;

		IF EXISTS (SELECT TOP 1 * FROM @UR_ID WHERE Operation = 'Insert')
			BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT ID FROM @UR_ID;    

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @ID

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'UserRole', @ID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'UserRole', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			FETCH NEXT FROM @AuditCursor 
			INTO @ID
			END; 

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;
			END

		IF EXISTS (SELECT TOP 1 * FROM @UR WHERE UserRoleID IS NOT NULL)
			BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT UserRoleID, RoleID, AuditDetailID FROM @UR WHERE UserRoleID IS NOT NULL;    

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @ID, @RoleID, @AuditDetailID

			WHILE @@FETCH_STATUS = 0
			BEGIN
			IF @RoleID IS NOT NULL
				BEGIN
				EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'UserRole', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
				END
			ELSE
				BEGIN
				EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Core', 'UserRole', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
				END		

			FETCH NEXT FROM @AuditCursor 
			INTO @ID, @RoleID, @AuditDetailID
			END; 

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;
			END;
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END