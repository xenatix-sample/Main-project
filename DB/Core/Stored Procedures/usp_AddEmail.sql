-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddEmail]
-- Author:		Saurabh Sahu
-- Date:		08/10/2015
--
-- Purpose:		Add Email Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		 (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/10/2015	Saurabh Sahu		Initial draft .
-- 09/21/2015   John Crossen        Add Audit
-- 01/12/2016	Scott Martin		Changed Insert to a Merge statement to prevent exact duplicates
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedOn and CreatedBy fields
-- 11/04/2016	Scott Martin		Added a TOP 1 to the return xml query
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddEmail]
	@EmailsXML XML,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@AdditionalResult XML OUTPUT
	
AS
BEGIN
DECLARE @EmailId int,
		@ModifiedOn DATETIME,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	DECLARE @tmpEmailsCreated TABLE
	(
		Operation varchar(10),
		EmailID INT,
		ModifiedOn DATETIME
	);

	MERGE INTO Core.Email e
	USING (SELECT DISTINCT T.C.value('Email[1]', 'NVARCHAR(255)') AS Email, T.C.value('ModifiedOn[1]', 'DATETIME') AS ModifiedOn FROM @EmailsXML.nodes('RequestXMLValue/Email') AS T(C)) ce
		ON ce.Email = e.Email
	WHEN NOT MATCHED THEN
		INSERT
		(
			Email, 
			IsActive, 
			ModifiedBy, 
			ModifiedOn,
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			ce.Email, 
			1, 
			@ModifiedBy, 
			ce.ModifiedOn,
			@ModifiedBy,
			ce.ModifiedOn
		)
	WHEN MATCHED THEN
		UPDATE
		SET @EmailID = e.EmailID,
			ModifiedOn = ce.ModifiedOn
	OUTPUT
		$action,
		inserted.EmailID,
		inserted.ModifiedOn
	INTO
		@tmpEmailsCreated;
			
	SELECT @AdditionalResult = (SELECT TOP 1 EmailID AS Identifier FROM @tmpEmailsCreated FOR XML PATH('OutParameters'), TYPE);

	IF EXISTS (SELECT TOP 1 * FROM @tmpEmailsCreated WHERE Operation = 'Insert')
		BEGIN
		DECLARE @AuditCursor CURSOR;
		DECLARE @AuditDetailID BIGINT;
		BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT EmailID, ModifiedOn FROM @tmpEmailsCreated WHERE Operation = 'Insert';    

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @EmailID, @ModifiedOn

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'Email', @EmailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'Email', @AuditDetailID, @EmailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			FETCH NEXT FROM @AuditCursor 
			INTO @EmailID, @ModifiedOn
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