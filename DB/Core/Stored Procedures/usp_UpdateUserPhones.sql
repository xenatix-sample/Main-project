-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_UpdateUserPhones]
-- Author:		Justin Spalti
-- Date:		10/01/2015
--
-- Purpose:		Update User Phone Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		Core.Phone, Core.UserPhone
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/01/2015	Justin Spalti - Initial creation
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn fields
-- 02/15/2016	Rajiv Ranjan		Move Core.usp_UpdatePhone to Registration.usp_UpdatePhone
-- 08/26/2016	Arun Choudhary		Increased length of extension field
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE  PROCEDURE [Core].[usp_UpdateUserPhones]
	@PhoneXML XML,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @IsPrimary BIT=0,
		@UserPhoneID BIGINT = 0,
		@OtherUserPhoneID BIGINT = 0,
		@UserID BIGINT,
		@PhoneID BIGINT,
		@ModifiedOn DATETIME,
		@AuditDetailID BIGINT,
		@PhoneCount INT,
		@AdditionalResultXML XML,
		@PhonesXML XML,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	IF OBJECT_ID('tempdb..#tmpUserPhone') IS NOT NULL
		DROP TABLE #tmpUserPhone

	CREATE TABLE #tmpUserPhone
	(
		UserPhoneID BIGINT,
		UserID BIGINT,
		PhoneID BIGINT, 
		PhoneTypeID INT, 
		Number NVARCHAR(50), 
		Extension NVARCHAR(10), 
		PhonePermissionID INT, 				
		IsPrimary BIT,
		IsActive BIT,
		ModifiedOn DATETIME
	);

	INSERT INTO #tmpUserPhone
	(
		UserPhoneID,
		PhoneID, 
		PhoneTypeID, 
		Number, 
		Extension, 
		PhonePermissionID, 
		IsPrimary,
		IsActive,
		ModifiedOn
	)
	SELECT 
		T.C.value('UserPhoneID[1]', 'BIGINT'), 
		T.C.value('PhoneID[1]', 'BIGINT'), 
		T.C.value('(./PhoneTypeID/text())[1]', 'INT'), 
		T.C.value('Number[1]', 'NVARCHAR(50)'), 
		T.C.value('Extension[1]', 'NVARCHAR(10)'), 
		T.C.value('(./PhonePermissionID/text())[1]', 'INT'),
		T.C.value('IsPrimary[1]', 'BIT'),
		T.C.value('IsActive[1]', 'BIT'),
		T.C.value('ModifiedOn[1]', 'DATETIME')
	FROM 
		@PhoneXML.nodes('RequestXMLValue/Phone') AS T(C);

	UPDATE #tmpUserPhone
	SET UserID = CE.UserID
	FROM
		#tmpUserPhone
		INNER JOIN Core.UserPhone CE
			ON #tmpUserPhone.UserPhoneID = CE.UserPhoneID
	WHERE
		#tmpUserPhone.UserPhoneID = CE.UserPhoneID;
	
	DECLARE @UpdateCursor CURSOR;
	SET @UpdateCursor = CURSOR FOR
	SELECT UserPhoneID, UserID, PhoneID, IsPrimary, ModifiedOn FROM #tmpUserPhone WHERE UserPhoneID IS NOT NULL;    

	OPEN @UpdateCursor 
	FETCH NEXT FROM @UpdateCursor 
	INTO @UserPhoneID, @UserID, @PhoneID, @IsPrimary, @ModifiedOn

	WHILE @@FETCH_STATUS = 0
	BEGIN
	SELECT @OtherUserPhoneID = UserPhoneID FROM Core.UserPhone WHERE UserID = @UserID AND IsPrimary = 1 AND UserPhoneID <> @UserPhoneID;

	IF ISNULL(@OtherUserPhoneID, 0) > 0 AND @IsPrimary = 1
		BEGIN
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'UserPhone', @OtherUserPhoneID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE CE
		SET IsPrimary = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		FROM
			Core.UserPhone CE 
		WHERE 
			CE.UserPhoneID = @OtherUserPhoneID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'UserPhone', @AuditDetailID, @OtherUserPhoneID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 
		END

	SET @PhoneCount = (SELECT COUNT(*) FROM Core.UserPhone WHERE PhoneID = @PhoneID AND UserID <> @UserID);

	SET @PhonesXML = (SELECT * FROM #tmpUserPhone WHERE PhoneID = @PhoneID FOR XML PATH ('Phone'), ROOT ('RequestXMLValue'));

	IF ISNULL(@PhoneCount, 0) > 0
		BEGIN
		EXEC Core.usp_AddPhone @PhonesXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AdditionalResultXML OUTPUT;

		SET @PhoneID = (SELECT T.C.value('Identifier[1]', 'BIGINT') FROM @AdditionalResultXML.nodes('OutParameters') AS T (C));
		END
	ELSE
		BEGIN
		EXEC Core.usp_UpdatePhone @PhonesXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;
		END

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'UserPhone', @UserPhoneID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE ce
	SET ce.PhoneID = te.PhoneID,
		ce.PhonePermissionID = te.PhonePermissionID,
		ce.IsActive = ISNULL(te.IsActive,1),
		IsPrimary = te.IsPrimary,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = te.ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM 
		Core.UserPhone ce 
		INNER JOIN #tmpUserPhone te
			ON te.UserPhoneID = ce.UserPhoneID
	WHERE
		ce.UserPhoneID = @UserPhoneID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'UserPhone', @AuditDetailID, @UserPhoneID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 

	FETCH NEXT FROM @UpdateCursor 
	INTO @UserPhoneID, @UserID, @PhoneID, @IsPrimary, @ModifiedOn
	END; 

	CLOSE @UpdateCursor;
	DEALLOCATE @UpdateCursor;
			
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END