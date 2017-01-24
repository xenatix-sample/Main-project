-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_UpdateMedicalHistoryCondition]
-- Author:		Scott Martin
-- Date:		11/20/2015
--
-- Purpose:		Update Medical History Condition
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/20/2015	Scott Martin	Initial creation.
-- 12/2/2015	Scott Martin	Refactored code to accommodate schema change (Table name/columns)
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn to Update statement
-- 02/17/2016	Scott Martin	Refactored audit logging
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_UpdateMedicalHistoryCondition]
	@MedicalHistoryXML XML,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ID BIGINT;

	BEGIN TRY
	SELECT @ResultCode = 0,
			@ResultMessage = 'Executed successfully'

	DECLARE @MedicalHistoryCondition TABLE
	(
		MedicalHistoryConditionID BIGINT,
		MedicalHistoryID BIGINT,
		MedicalConditionID INT,
		AuditDetailID BIGINT
	);

	INSERT INTO @MedicalHistoryCondition
	SELECT
		T.C.value('MedicalHistoryConditionID[1]','BIGINT'),
		T.C.value('MedicalHistoryID[1]','BIGINT'),
		T.C.value('MedicalConditionID[1]','INT'),
		NULL
	FROM
		@MedicalHistoryXML.nodes('MedicalHistory/MedicalHistoryConditions') as T(C);

	DECLARE @AuditCursor CURSOR;
	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT MedicalHistoryConditionID FROM @MedicalHistoryCondition;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Clinical', 'MedicalHistoryCondition', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE @MedicalHistoryCondition
		SET AuditDetailID = @AuditDetailID
		WHERE
			MedicalHistoryConditionID = @ID;
		
		FETCH NEXT FROM @AuditCursor 
		INTO @ID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
	END;

	UPDATE MHC
	SET MedicalHistoryID = tMHC.MedicalHistoryID,
		MedicalConditionID = tMHC.MedicalConditionID,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM
		Clinical.MedicalHistoryCondition MHC
		JOIN @MedicalHistoryCondition tMHC
			ON MHC.MedicalHistoryConditionID = tMHC.MedicalHistoryConditionID
	WHERE
		MHC.MedicalHistoryConditionID = tMHC.MedicalHistoryConditionID;

	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT AuditDetailID, MedicalHistoryConditionID FROM @MedicalHistoryCondition;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @AuditDetailID, @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Clinical', 'MedicalHistoryCondition', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @AuditDetailID, @ID
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
GO