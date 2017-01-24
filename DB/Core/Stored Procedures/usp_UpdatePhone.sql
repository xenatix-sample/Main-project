-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdatePhone]
-- Author:		Saurabh Sahu	
-- Date:		08/10/2015
--
-- Purpose:		Update modifiable Phone Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/10/2015 - Saurabh Sahu - Initial draft
-- 01/12/2016	Scott Martin		Added audit logging
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
-- 02/15/2016	Scott Martin		Refactored the code for new auditing method
-- 03/02/2016 - Justin Spalti - Renamed the temp table to #tmpPhones to prevent same session conflicts
-- 07/14/2016	Gurmant Singh	Increased length of Extension column from 5 to 10 characters
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdatePhone]
	@PhoneXML XML,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @PhoneID INT,
		@ModifiedOn DATETIME,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'Executed Successfully';

	BEGIN TRY
	IF OBJECT_ID('tempdb..#tmpPhones') IS NOT NULL
		DROP TABLE #tmpPhones

	CREATE TABLE #tmpPhones
	(	
		PhoneID BIGINT, 
		PhoneTypeID INT, 
		Number NVARCHAR(50), 
		Extension NVARCHAR(10), 
		IsActive BIT,
		ModifiedOn DATETIME,
		AuditDetailID BIGINT
	);

	INSERT INTO #tmpPhones
	(
		PhoneID, 
		PhoneTypeID, 
		Number, 
		Extension, 
		IsActive,
		ModifiedOn,
		AuditDetailID
	)
	SELECT 
		T.C.value('PhoneID[1]', 'BIGINT'), 
		T.C.value('(./PhoneTypeID/text())[1]', 'INT'), 
		T.C.value('Number[1]', 'NVARCHAR(50)'), 
		T.C.value('Extension[1]', 'NVARCHAR(10)'), 
		T.C.value('IsActive[1]', 'BIT'),
		T.C.value('ModifiedOn[1]', 'DATETIME'),
		NULL
	FROM 
		@PhoneXML.nodes('RequestXMLValue/Phone') AS T(C);

	DECLARE @AuditCursor CURSOR;
	DECLARE @AuditDetailID BIGINT;
	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT PhoneID, ModifiedOn FROM #tmpPhones;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @PhoneID, @ModifiedOn

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'Phone', @PhoneID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE #tmpPhones
		SET AuditDetailID = @AuditDetailID
		WHERE
			PhoneID = @PhoneID;

		FETCH NEXT FROM @AuditCursor 
		INTO @PhoneID, @ModifiedOn
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
	END;

	UPDATE p
	SET PhoneTypeID = tp.PhoneTypeID,
		Number = tp.Number,
		Extension = tp.Extension,
		IsActive = COALESCE(tp.IsActive, p.IsActive),
		ModifiedBy = @ModifiedBy,
		ModifiedOn = tp.ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM
		Core.Phone p
		INNER JOIN #tmpPhones tp 
			ON tp.PhoneID = p.PhoneID;

	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT PhoneID, ModifiedOn, AuditDetailID FROM #tmpPhones;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @PhoneID, @ModifiedOn, @AuditDetailID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'Phone', @AuditDetailID, @PhoneID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @PhoneID, @ModifiedOn, @AuditDetailID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
	END;
		
	END TRY

	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
