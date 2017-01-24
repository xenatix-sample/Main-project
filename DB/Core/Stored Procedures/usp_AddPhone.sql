-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddPhones]
-- Author:		Saurabh Sahu
-- Date:		08/10/2015
--
-- Purpose:		Add Phone Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		(or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/10/2015	Saurabh Sahu		Created .
-- 09/21/2015   John Crossen        Add Audit
-- 01/12/2016	Scott Martin		Changed Insert to a Merge statement to prevent exact duplicates
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedOn and CreatedBy fields
-- 07/12/2016	Scott Martin		Increased length of extension field
-- 07/14/2016	Gurmant Singh		Increased length of temporary table #tmpPhone from 5 to 10 characters
-- 11/04/2016	Scott Martin		Added a TOP 1 to the return xml query
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddPhone]
	@PhonesXML xml,
	@ModifiedBy INT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT,
	@AdditionalResult XML OUTPUT	
AS
BEGIN
DECLARE @PhoneId INT,
		@ModifiedOn DATETIME,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	-- iterate through the addresses in the passed in xml, and each time an address is created, also associate the address w/ the contact
	IF OBJECT_ID('tempdb..#tmpPhone') IS NOT NULL
		DROP TABLE #tmpPhone		

	CREATE TABLE #tmpPhone
	(
		PhoneTypeID INT, 
		Number NVARCHAR(50), 
		Extension NVARCHAR(10),
		ModifiedOn DATETIME
	);

	INSERT INTO #tmpPhone
	(
		PhoneTypeID,
		Number,
		Extension,
		ModifiedOn
	)
	SELECT 
		T.C.value('(./PhoneTypeID/text())[1]', 'INT'), 
		T.C.value('Number[1]', 'NVARCHAR(50)'),
		T.C.value('Extension[1]', 'NVARCHAR(10)'),
		T.C.value('ModifiedOn[1]', 'DATETIME')
	FROM 
		@PhonesXML.nodes('RequestXMLValue/Phone') AS T(C);

	DECLARE @tmpPhonesCreated TABLE
	(
		Operation varchar(10),
		PhoneID INT,
		ModifiedOn DATETIME
	);

	MERGE INTO Core.Phone p
	USING (SELECT Number, PhoneTypeID, Extension, ModifiedOn FROM #tmpPhone) cp
		ON ISNULL(cp.Number, '') = ISNULL(p.Number, '')
		AND ISNULL(cp.PhoneTypeID, 0) = ISNULL(p.PhoneTypeID, 0)
		AND ISNULL(cp.Extension, '') = ISNULL(p.Extension, '')
	WHEN NOT MATCHED THEN
		INSERT
		(
			PhoneTypeID, 
			Number, 
			Extension, 
			IsActive, 
			ModifiedBy, 
			ModifiedOn,
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			cp.PhoneTypeID, 
			cp.Number, 
			cp.Extension, 
			1, 
			@ModifiedBy, 
			cp.ModifiedOn,
			@ModifiedBy,
			cp.ModifiedOn
		)
	WHEN MATCHED THEN
		UPDATE
		SET @PhoneID = p.PhoneID
	OUTPUT
		$action,
		inserted.PhoneID,
		inserted.ModifiedOn
	INTO
		@tmpPhonesCreated;

	SELECT @AdditionalResult = (SELECT TOP 1 PhoneID AS Identifier FROM @tmpPhonesCreated FOR XML PATH('OutParameters'), TYPE);

	IF EXISTS (SELECT TOP 1 * FROM @tmpPhonesCreated WHERE Operation = 'Insert')
		BEGIN
		DECLARE @AuditCursor CURSOR;
		DECLARE @AuditDetailID BIGINT;
		BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT PhoneID, ModifiedOn FROM @tmpPhonesCreated WHERE Operation = 'Insert';    

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @PhoneId, @ModifiedOn

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'Phone', @PhoneId, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'Phone', @AuditDetailID, @PhoneId, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			FETCH NEXT FROM @AuditCursor 
			INTO @PhoneId, @ModifiedOn
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