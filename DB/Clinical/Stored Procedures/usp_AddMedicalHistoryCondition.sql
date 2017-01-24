-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_AddMedicalHistoryCondition]
-- Author:		Scott Martin
-- Date:		11/20/2015
--
-- Purpose:		Add Medical History Condition Data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/20/2015	Scott Martin	Initial creation.
-- 12/2/2015	Scott Martin	Refactored code to accommodate schema change (Table name/columns)
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-- 02/17/2016	Scott Martin	Added audit logging
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_AddMedicalHistoryCondition]
	@MedicalHistoryXML XML,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @ID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

DECLARE @MHC_ID TABLE (ID BIGINT);

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	INSERT INTO [Clinical].[MedicalHistoryCondition]
    (
		[MedicalHistoryID],
		[MedicalConditionID],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		[CreatedBy],
		[CreatedOn]
	)
	OUTPUT inserted.MedicalHistoryConditionID
	INTO @MHC_ID
	SELECT
		T.C.value('MedicalHistoryID[1]','BIGINT'),
		T.C.value('MedicalConditionID[1]','BIGINT'),
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM 
		@MedicalHistoryXML.nodes('MedicalHistory/MedicalHistoryConditions') AS T(C);

	DECLARE @AuditCursor CURSOR;
	DECLARE @AuditDetailID BIGINT;
	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ID FROM @MHC_ID;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Clinical', 'MedicalHistoryCondition', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Clinical', 'MedicalHistoryCondition', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
	END;	
  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE();
	END CATCH
END
GO