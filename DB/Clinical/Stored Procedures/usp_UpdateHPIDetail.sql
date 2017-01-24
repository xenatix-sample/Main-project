-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_UpdateHPIDetail]
-- Author:		Scott Martin
-- Date:		11/20/2015
--
-- Purpose:		Update HPI Detail
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/20/2015	Scott Martin	Initial creation.
-- 12/8/2015	Scott Martin	Added 2 new columns
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn to Update statement
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_UpdateHPIDetail]
	@HPIXML XML,
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

	DECLARE @HPIDetail TABLE
	(
		HPIDetailID BIGINT,
		Comment NVARCHAR(2000),
		Location NVARCHAR(255),
		HPISeverityID SMALLINT,
		Quality NVARCHAR(255),
		Duration NVARCHAR(255),
		Timing NVARCHAR(255),
		Context NVARCHAR(255),
		ModifyingFactors NVARCHAR(255),
		Symptoms NVARCHAR(255),
		Conditions NVARCHAR(255),
		AuditDetailID BIGINT
	)

	INSERT INTO @HPIDetail
	SELECT
		T.C.value('HPIDetailID[1]','BIGINT'),
		T.C.value('Comment[1]','NVARCHAR(2000)'),
		T.C.value('Location[1]','NVARCHAR(255)'),
		T.C.value('HPISeverityID[1]','SMALLINT'),
		T.C.value('Quality[1]','NVARCHAR(255)'),
		T.C.value('Duration[1]','NVARCHAR(255)'),
		T.C.value('Timing[1]','NVARCHAR(255)'),
		T.C.value('Context[1]','NVARCHAR(255)'),
		T.C.value('ModifyingFactors[1]','NVARCHAR(255)'),
		T.C.value('Symptoms[1]','NVARCHAR(255)'),
		T.C.value('Conditions[1]','NVARCHAR(255)'),
		NULL
	FROM
		@HPIXML.nodes('HPI/HPIDetails') as T(C);

	DECLARE @AuditCursor CURSOR;
	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT HPIDetailID FROM @HPIDetail;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Clinical', 'HPIDetail', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE @HPIDetail
		SET AuditDetailID = @AuditDetailID
		WHERE
			HPIDetailID = @ID;
		
		FETCH NEXT FROM @AuditCursor 
		INTO @ID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
	END;

	UPDATE HPID
	SET Comment = tHPID.Comment,
		Location = tHPID.Location,
		HPISeverityID = tHPID.HPISeverityID,
		Quality = tHPID.Quality,
		Duration = tHPID.Duration,
		Timing = tHPID.Timing,
		Context = tHPID.Context,
		ModifyingFactors = tHPID.ModifyingFactors,
		Symptoms = tHPID.Symptoms,
		Conditions = tHPID.Conditions,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM
		Clinical.HPIDetail HPID
		JOIN @HPIDetail tHPID
			ON HPID.HPIDetailID = tHPID.HPIDetailID
	WHERE
		HPID.HPIDetailID = tHPID.HPIDetailID;

	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT AuditDetailID, HPIDetailID FROM @HPIDetail;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @AuditDetailID, @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Clinical', 'HPIDetail', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

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