-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_UpdateMedicalHistoryConditionDetail]
-- Author:		Scott Martin
-- Date:		12/3/2015
--
-- Purpose:		Update Medical History Condition Detail
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/3/2015	Scott Martin	Initial creation.
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn to Update statement
-- 02/17/2016	Scott Martin	Refactored audit logging
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_UpdateMedicalHistoryConditionDetail]
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

	DECLARE @MedicalHistoryConditionDetail TABLE
	(
		MedicalHistoryConditionDetailID BIGINT,
		MedicalHistoryConditionID BIGINT,
		IsSelf BIT,
		FamilyRelationshipID BIGINT,
		Comments NVARCHAR(2000),
		AuditDetailID BIGINT
	);

	INSERT INTO @MedicalHistoryConditionDetail
	SELECT
		T.C.value('MedicalHistoryConditionDetailID[1]','BIGINT'),
		T.C.value('MedicalHistoryConditionID[1]','BIGINT'),
		T.C.value('IsSelf[1]','BIT'),
		T.C.value('FamilyRelationshipID[1]','BIGINT'),
		T.C.value('Comments[1]','NVARCHAR(2000)'),
		NULL
	FROM
		@MedicalHistoryXML.nodes('MedicalHistory/MedicalHistoryConditions/MedicalHistoryConditionDetails') as T(C)

	DECLARE @AuditCursor CURSOR;
	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT MedicalHistoryConditionDetailID FROM @MedicalHistoryConditionDetail;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Clinical', 'MedicalHistoryConditionDetailID', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE @MedicalHistoryConditionDetail
		SET AuditDetailID = @AuditDetailID
		WHERE
			MedicalHistoryConditionDetailID = @ID;
		
		FETCH NEXT FROM @AuditCursor 
		INTO @ID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
	END;

	UPDATE MHD
	SET MedicalHistoryConditionID = tMHD.MedicalHistoryConditionID,
		IsSelf = tMHD.IsSelf,
		FamilyRelationshipID = tMHD.FamilyRelationshipID,
		Comments = tMHD.Comments,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM
		Clinical.MedicalHistoryConditionDetail MHD
		JOIN @MedicalHistoryConditionDetail tMHD
			ON MHD.MedicalHistoryConditionDetailID = tMHD.MedicalHistoryConditionDetailID
	WHERE
		MHD.MedicalHistoryConditionDetailID = tMHD.MedicalHistoryConditionDetailID;

	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT AuditDetailID, MedicalHistoryConditionDetailID FROM @MedicalHistoryConditionDetail;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @AuditDetailID, @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Clinical', 'MedicalHistoryConditionDetailID', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

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