-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateUserAddresses]
-- Author:		Justin Spalti
-- Date:		10/01/2015
--
-- Purpose:		Update user addresses
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		Core.Address, Core.UserAddress 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/01/2015	Justin Spalti - Initial creation
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
-- 03/04/2016   Justin Spalti - Updated temp table name to prevent same session table conflicts
-- 03/07/2016	Kyle Campbell	TFS #5793 Modifed field size for ComplexName and GateCode
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateUserAddresses]
	@AddressesXML XML,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @IsPrimary BIT=0,
		@UserAddressID BIGINT = 0,
		@OtherUserAddressID BIGINT = 0,
		@UserID BIGINT,
		@AddressID BIGINT,
		@ModifiedOn DATETIME,
		@AuditDetailID BIGINT,
		@AddressCount INT,
		@AdditionalResultXML XML,
		@AddressXML XML,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	IF OBJECT_ID('tempdb..#tmpAddresses') IS NOT NULL
		DROP TABLE #tmpAddresses

	CREATE TABLE #tmpAddresses
	(
		UserAddressID BIGINT,
		UserID BIGINT,
		AddressID BIGINT, 
		AddressTypeID INT, 
		Line1 NVARCHAR(200), 
		Line2 NVARCHAR(200), 
		City NVARCHAR(200), 
		StateProvince INT, 
		County INT, 
		Zip NVARCHAR(10), 
		ComplexName NVARCHAR(255), 
		GateCode NVARCHAR(50), 
		MailPermissionID INT, 				
		IsPrimary bit,
		ModifiedOn DATETIME
	)

	INSERT INTO #tmpAddresses
	(
		UserAddressID,
		AddressID, 
		AddressTypeID, 
		Line1, 
		Line2, 
		City, 
		StateProvince, 
		County, 
		Zip, 
		ComplexName, 
		GateCode, 
		MailPermissionID,				
		IsPrimary,
		ModifiedOn
	)
	SELECT 
		T.C.value('UserAddressID[1]', 'BIGINT'), 
		T.C.value('AddressID[1]', 'BIGINT'), 
		T.C.value('(./AddressTypeID/text())[1]', 'INT'), 
		T.C.value('Line1[1]', 'NVARCHAR(200)'),
		T.C.value('Line2[1]', 'NVARCHAR(200)'),
		T.C.value('City[1]', 'NVARCHAR(200)'),
		T.C.value('(./StateProvince/text())[1]', 'INT'),
		T.C.value('(./County/text())[1]', 'INT'),
		T.C.value('Zip[1]', 'NVARCHAR(10)'),
		T.C.value('ComplexName[1]', 'NVARCHAR(255)'),
		T.C.value('GateCode[1]', 'NVARCHAR(50)'),
		T.C.value('(./MailPermissionID/text())[1]', 'INT'),
		T.C.value('IsPrimary[1]', 'BIT'),
		T.C.value('ModifiedOn[1]', 'DATETIME')
	FROM 
		@AddressesXML.nodes('RequestXMLValue/Address') AS T(C);

	UPDATE #tmpAddresses
	SET UserID = CE.UserID
	FROM
		#tmpAddresses
		INNER JOIN Core.UserAddress CE
			ON #tmpAddresses.UserAddressID = CE.UserAddressID
	WHERE
		#tmpAddresses.UserAddressID = CE.UserAddressID;
	
	DECLARE @UpdateCursor CURSOR;
	SET @UpdateCursor = CURSOR FOR
	SELECT UserAddressID, UserID, AddressID, IsPrimary, ModifiedOn FROM #tmpAddresses WHERE UserAddressID IS NOT NULL;    

	OPEN @UpdateCursor 
	FETCH NEXT FROM @UpdateCursor 
	INTO @UserAddressID, @UserID, @AddressID, @IsPrimary, @ModifiedOn

	WHILE @@FETCH_STATUS = 0
	BEGIN
	SELECT @OtherUserAddressID = UserAddressID FROM Core.UserAddress WHERE UserID = @UserID AND IsPrimary = 1 AND UserAddressID <> @UserAddressID;

	IF ISNULL(@OtherUserAddressID, 0) > 0 AND @IsPrimary = 1
		BEGIN
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'UserAddress', @OtherUserAddressID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE CE
		SET IsPrimary = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		FROM
			Core.UserAddress CE 
		WHERE 
			CE.UserAddressID = @OtherUserAddressID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'UserAddress', @AuditDetailID, @OtherUserAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 
		END

	SET @AddressCount = (SELECT COUNT(*) FROM Core.UserAddress WHERE AddressID = @AddressID AND UserID <> @UserID);

	SET @AddressXML = (SELECT * FROM #tmpAddresses WHERE AddressID = @AddressID FOR XML PATH ('Address'), ROOT ('RequestXMLValue'));

	IF ISNULL(@AddressCount, 0) > 0
		BEGIN
		EXEC Core.usp_AddAddress @AddressXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AdditionalResultXML OUTPUT;

		SET @AddressID = (SELECT T.C.value('Identifier[1]', 'BIGINT') FROM @AdditionalResultXML.nodes('OutParameters') AS T (C));
		END
	ELSE
		BEGIN
		EXEC Core.usp_UpdateAddress @AddressXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;
		END

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'UserAddress', @UserAddressID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE ce
	SET ce.AddressID = te.AddressID,
		ce.MailPermissionID = te.MailPermissionID,
		IsPrimary = te.IsPrimary,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = te.ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM 
		Core.UserAddress ce 
		INNER JOIN #tmpAddresses te
			ON te.UserAddressID = ce.UserAddressID
	WHERE
		ce.UserAddressID = @UserAddressID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'UserAddress', @AuditDetailID, @UserAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 

	FETCH NEXT FROM @UpdateCursor 
	INTO @UserAddressID, @UserID, @AddressID, @IsPrimary, @ModifiedOn
	END; 

	CLOSE @UpdateCursor;
	DEALLOCATE @UpdateCursor;
			
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END