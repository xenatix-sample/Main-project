-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_SaveUserCredentials
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
-- 03/29/2016 - Justin Spalti - Added StateIssuedByID to the xml output
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_SaveUserCredentials]
	@UserID INT,
	@CredentialsXMLValue XML,	
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT

AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'User updated successfully'
		   --<CredentialsXMLValue><Credential><CredentialID>1</CredentialID></Credential></CredentialsXMLValue>

	BEGIN TRY			
		DECLARE @ID BIGINT,
				@CredentialID BIGINT,
				@AuditDetailID BIGINT,
				@AuditCursor CURSOR,
				@EmailXML XML,
				@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

		DECLARE @UC TABLE
		(
			UserCredentialID BIGINT,
			CredentialID BIGINT,
			LicenseNbr NVARCHAR(100),
			StateIssuedByID INT,
			EffectiveDate DATETIME,
			ExpirationDate DATETIME,
			UserID BIGINT,
			AuditDetailID BIGINT
		);

		INSERT INTO @UC(UserCredentialID, CredentialID, LicenseNbr, StateIssuedByID, EffectiveDate, ExpirationDate, UserID, AuditDetailID)
		SELECT
			UC.UserCredentialID,
			T.C.value('CredentialID[1]','BIGINT') AS CredentialID,
			T.C.value('LicenseNbr[1]', 'NVARCHAR(100)') AS LicenseNbr,
			T.C.value('StateIssuedByID[1]', 'INT') AS StateIssuedByID,
			T.C.value('EffectiveDate[1]', 'DATETIME') AS EffectiveDate,
			T.C.value('ExpirationDate[1]', 'DATETIME') AS ExpirationDate,
			UC.UserID,
			NULL
		FROM
			@CredentialsXMLValue.nodes('CredentialsXMLValue/Credential') AS T(C)
			FULL OUTER JOIN (SELECT * FROM Core.UserCredential WHERE UserID = @UserID) AS UC
				ON T.C.value('CredentialID[1]','BIGINT') = UC.CredentialID;

		IF EXISTS (SELECT TOP 1 * FROM @UC WHERE UserCredentialID IS NOT NULL)
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT UserCredentialID, CredentialID FROM @UC WHERE UserCredentialID IS NOT NULL;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @CredentialID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		IF @CredentialID IS NOT NULL
			BEGIN
			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'UserCredential', @ID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END
		ELSE
			BEGIN
			EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Core', 'UserCredential', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END

		UPDATE @UC
		SET AuditDetailID = @AuditDetailID
		WHERE
			UserCredentialID = @ID;
		
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @CredentialID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END;

		DECLARE @UC_ID TABLE
		(
			Operation NVARCHAR(12),
			ID BIGINT
		);

		MERGE Core.UserCredential AS TARGET
		USING (SELECT * FROM @UC WHERE CredentialID IS NOT NULL) AS SOURCE
			ON SOURCE.CredentialID = TARGET.CredentialID
			AND TARGET.UserID = @UserID
		WHEN NOT MATCHED BY TARGET THEN
			INSERT
			(
				UserID, CredentialID, LicenseNbr, StateIssuedByID, LicenseIssueDate, LicenseExpirationDate, IsActive, ModifiedBy, ModifiedOn, CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
			)
			VALUES
			(
				@UserID, SOURCE.CredentialID, SOURCE.LicenseNbr, SOURCE.StateIssuedByID, SOURCE.EffectiveDate, SOURCE.ExpirationDate, 1, @ModifiedBy, @ModifiedOn, @ModifiedBy, @ModifiedOn, GETUTCDATE(), GETUTCDATE()
			)
		WHEN NOT MATCHED BY SOURCE AND TARGET.UserID = @UserID THEN
			UPDATE
			SET TARGET.IsActive = 0,
				TARGET.ModifiedBy = @ModifiedBy,
				TARGET.ModifiedOn = @ModifiedOn,
				TARGET.SystemModifiedOn = GETUTCDATE()
		WHEN MATCHED THEN
			UPDATE
			SET TARGET.LicenseNbr = SOURCE.LicenseNbr, 
				TARGET.StateIssuedByID = SOURCE.StateIssuedByID,
				TARGET.LicenseIssueDate = SOURCE.EffectiveDate, 
				TARGET.LicenseExpirationDate = SOURCE.ExpirationDate,
				TARGET.IsActive = 1,
				TARGET.ModifiedBy = @ModifiedBy,
				TARGET.ModifiedOn = @ModifiedOn,
				TARGET.SystemModifiedOn = GETUTCDATE()
		OUTPUT
			$action,
			inserted.UserCredentialID
		INTO
			@UC_ID;


		--in progress
		IF EXISTS (SELECT TOP 1 * FROM @UC_ID WHERE Operation = 'Insert')
			BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT ID FROM @UC_ID;    

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @ID

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'UserCredential', @ID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'UserCredential', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			FETCH NEXT FROM @AuditCursor 
			INTO @ID
			END; 

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;
			END

		IF EXISTS (SELECT TOP 1 * FROM @UC WHERE UserCredentialID IS NOT NULL)
			BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT UserCredentialID, CredentialID, AuditDetailID FROM @UC WHERE UserCredentialID IS NOT NULL;    

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @ID, @CredentialID, @AuditDetailID

			WHILE @@FETCH_STATUS = 0
			BEGIN
			IF @CredentialID IS NOT NULL
				BEGIN
				EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'UserCredential', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
				END
			ELSE
				BEGIN
				EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Core', 'UserCredential', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
				END		

			FETCH NEXT FROM @AuditCursor 
			INTO @ID, @CredentialID, @AuditDetailID
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