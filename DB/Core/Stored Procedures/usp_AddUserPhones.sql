-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_AddUserPhones]
-- Author:		Justin Spalti
-- Date:		10/01/2015
--
-- Purpose:		Add User Phone Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		Core.Phone, Core.UserPhone
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/01/2015	Justin Spalti - Initial creation
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedOn and CreatedBy fields
-- 08/26/2016	Arun Choudhary		Increased length of extension field
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddUserPhones]
	@UserID BIGINT,
	@PhonesXML XML,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@AuditCursor CURSOR,
		@ModifiedOn DATETIME,
		@AdditionalResultXML XML,
		@IsPrimary BIT = 0,
		@UserPhoneID BIGINT,
		@PhoneID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);
	 
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY

	-- iterate through the addresses in the passed in xml, and each time an address is created, also associate the address w/ the user
	IF OBJECT_ID('tempdb..#tmpUserPhone') IS NOT NULL
		DROP TABLE #tmpUserPhone

	CREATE TABLE #tmpUserPhone
	(
		UserPhoneID BIGINT,
		PhoneID BIGINT,
		PhoneTypeID INT, 
		Number NVARCHAR(50), 
		Extension NVARCHAR(10),
		PhonePermissionID int,
		IsPrimary BIT,
		ModifiedOn DATETIME,
		AuditDetailID BIGINT
	);

	INSERT INTO #tmpUserPhone
	(
		PhoneTypeID,
		Number,
		Extension,
		PhonePermissionID,
		IsPrimary,
		ModifiedOn
	)
	SELECT 
		T.C.value('(./PhoneTypeID/text())[1]', 'INT'), 
		T.C.value('Number[1]', 'NVARCHAR(50)'),
		T.C.value('Extension[1]', 'NVARCHAR(10)'),
		T.C.value('(./PhonePermissionID/text())[1]', 'int'),
		T.C.value('IsPrimary[1]', 'bit'),
		T.C.value('ModifiedOn[1]', 'DATETIME')
	FROM 
		@PhonesXML.nodes('RequestXMLValue/Phone') AS T(C);

	EXEC Core.usp_AddPhone @PhonesXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AdditionalResultXML OUTPUT;

	UPDATE #tmpUserPhone
	SET PhoneID = P.PhoneID
	FROM
		#tmpUserPhone tCP
		INNER JOIN Core.Phone P
			ON ISNULL(tCP.Number, '') = ISNULL(P.Number, '')
			AND ISNULL(tCP.PhoneTypeID, 0) = ISNULL(P.PhoneTypeID, 0)
			AND ISNULL(tCP.Extension, '') = ISNULL(P.Extension, '')
			AND P.PhoneID IN (SELECT T.C.value('Identifier[1]', 'BIGINT') FROM @AdditionalResultXML.nodes('OutParameters') AS T (C))
	WHERE
		ISNULL(tCP.Number, '') = ISNULL(P.Number, '')
		AND ISNULL(tCP.PhoneTypeID, 0) = ISNULL(P.PhoneTypeID, 0)
		AND ISNULL(tCP.Extension, '') = ISNULL(P.Extension, '');

	UPDATE #tmpUserPhone
	SET UserPhoneID = CP.UserPhoneID
	FROM
		#tmpUserPhone tCP
		INNER JOIN Core.UserPhone CP
			ON tCp.PhoneID = CP.PhoneID
			AND CP.UserID = @UserID
	WHERE
		tCp.PhoneID = CP.PhoneID
		AND CP.UserID = @UserID;

	--Select statements purposely set this way so the procedure will fail if more than 1 primary email is found
	SET @IsPrimary = (SELECT IsPrimary FROM #tmpUserPhone WHERE IsPrimary = 1);

	SELECT @PhoneID = PhoneID, @ModifiedOn = ModifiedOn FROM #tmpUserPhone WHERE IsPrimary = 1;

	SET @UserPhoneID = (SELECT UserPhoneID FROM Core.UserPhone WHERE UserID = @UserID AND PhoneID <> @PhoneID AND IsPrimary = 1);

	--If the email to be inserted IsPrimary = 1 and there is an existing primary, set the existing primary to 0
	IF @UserPhoneID IS NOT NULL AND @IsPrimary = 1
		BEGIN
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'UserPhone', @UserPhoneID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Core.UserPhone
		SET IsPrimary = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE	
			UserPhoneID = @UserPhoneID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'UserPhone', @AuditDetailID, @UserPhoneID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END

	IF EXISTS (SELECT TOP 1 * FROM #tmpUserPhone WHERE UserPhoneID IS NOT NULL)
		BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT UserPhoneID, ModifiedOn FROM #tmpUserPhone WHERE UserPhoneID IS NOT NULL;    

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @UserPhoneID, @ModifiedOn

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'UserPhone', @UserPhoneID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
			UPDATE #tmpUserPhone
			SET AuditDetailID = @AuditDetailID
			WHERE
				UserPhoneID = @UserPhoneID;

			FETCH NEXT FROM @AuditCursor 
			INTO @UserPhoneID, @ModifiedOn
			END; 

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;
		END;

		-- Merge statement for Core.UserPhone
		DECLARE	@tmpUserPhone TABLE
		(
			Operation varchar(10),
			UserPhoneID INT,
			ModifiedOn DATETIME
		);

	MERGE INTO Core.UserPhone AS TARGET
	USING (SELECT * FROM #tmpUserPhone t)  AS SOURCE
		ON ISNULL(SOURCE.UserPhoneID, 0) = TARGET.UserPhoneID
	WHEN NOT MATCHED THEN
		INSERT
		(
			UserID,
			PhoneID,
			PhonePermissionID,
			IsPrimary,
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			@UserID,
			SOURCE.PhoneID,
			SOURCE.PhonePermissionID,
			SOURCE.IsPrimary,
			1,
			@ModifiedBy,
			SOURCE.ModifiedOn,
			@ModifiedBy,
			SOURCE.ModifiedOn
		)
	WHEN MATCHED THEN
		UPDATE
		SET TARGET.IsPrimary = SOURCE.IsPrimary,
			TARGET.PhonePermissionID = SOURCE.PhonePermissionID,
			TARGET.IsActive = 1,
			TARGET.ModifiedBy = @ModifiedBy,
			TARGET.ModifiedOn = SOURCE.ModifiedOn,
			TARGET.SystemModifiedOn = GETUTCDATE()
	OUTPUT
		$action,
		inserted.UserPhoneID,
		inserted.ModifiedOn
	INTO
		@tmpUserPhone;

	IF EXISTS (SELECT TOP 1 * FROM @tmpUserPhone WHERE Operation = 'Insert')
		BEGIN
		BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT UserPhoneID, ModifiedOn FROM @tmpUserPhone WHERE Operation = 'Insert';    

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @UserPhoneID, @ModifiedOn

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'UserPhone', @UserPhoneID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'UserPhone', @AuditDetailID, @UserPhoneID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			FETCH NEXT FROM @AuditCursor 
			INTO @UserPhoneID, @ModifiedOn
			END; 

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;
		END;
		END

		IF EXISTS (SELECT TOP 1 * FROM #tmpUserPhone WHERE UserPhoneID IS NOT NULL)
		BEGIN
		BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT UserPhoneID, ModifiedOn, AuditDetailID FROM #tmpUserPhone WHERE UserPhoneID IS NOT NULL;   

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @UserPhoneID, @ModifiedOn, @AuditDetailID

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'UserPhone', @AuditDetailID, @UserPhoneID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			FETCH NEXT FROM @AuditCursor 
			INTO @UserPhoneID, @ModifiedOn, @AuditDetailID
			END; 

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;
		END;
		END

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END