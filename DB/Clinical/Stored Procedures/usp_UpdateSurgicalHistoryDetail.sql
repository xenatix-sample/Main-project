-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_UpdateSurgicalHistoryDetail]
-- Author:		Scott Martin
-- Date:		11/30/2015
--
-- Purpose:		Update Surgical History Detail
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/30/2015	Scott Martin	Initial creation.
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn to Update statement
-- 02/17/2016	Scott Martin	Refactored audit logging
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_UpdateSurgicalHistoryDetail]
	@SurgicalHistoryXML XML,
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

	DECLARE @SurgicalHistoryDetail TABLE
	(
		SurgicalHistoryDetailID BIGINT,
		SurgicalHistoryID BIGINT,
		Surgery NVARCHAR(255),
		Date DATE,
		Comments NVARCHAR(2000),
		AuditDetailID BIGINT
	);

	INSERT INTO @SurgicalHistoryDetail
	SELECT
		T.C.value('SurgicalHistoryDetailID[1]','BIGINT'),
		T.C.value('SurgicalHistoryID[1]','BIGINT'),
		T.C.value('Surgery[1]','NVARCHAR(255)'),
		T.C.value('Date[1]','DATE'),
		T.C.value('Comments[1]','NVARCHAR(2000)'),
		NULL
	FROM
		@SurgicalHistoryXML.nodes('SurgicalHistory/SurgicalHistoryDetails') as T(C);

	DECLARE @AuditCursor CURSOR;
	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT SurgicalHistoryDetailID FROM @SurgicalHistoryDetail;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Clinical', 'SurgicalHistoryDetail', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE @SurgicalHistoryDetail
		SET AuditDetailID = @AuditDetailID
		WHERE
			SurgicalHistoryDetailID = @ID;
		
		FETCH NEXT FROM @AuditCursor 
		INTO @ID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
	END;

	UPDATE SHD
	SET SurgicalHistoryID = tSHD.SurgicalHistoryID,
		Surgery = tSHD.Surgery,
		Date = tSHD.Date,
		Comments = tSHD.Comments,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM
		Clinical.SurgicalHistoryDetail SHD
		JOIN @SurgicalHistoryDetail tSHD
			ON SHD.SurgicalHistoryDetailID = tSHD.SurgicalHistoryDetailID
	WHERE
		SHD.SurgicalHistoryDetailID = tSHD.SurgicalHistoryDetailID;

	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT AuditDetailID, SurgicalHistoryDetailID FROM @SurgicalHistoryDetail;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @AuditDetailID, @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Clinical', 'SurgicalHistoryDetail', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

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


