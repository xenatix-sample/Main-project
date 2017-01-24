-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_HPIDetail]
-- Author:		John Crossen
-- Date:		11/20/2015
--
-- Purpose:		Add HPI Detail Data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/20/2015	Scott Martin	Initial creation.
-- 12/8/2015	Scott Martin	Added 2 new columns
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
------------------------------------    -----------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_AddHPIDetail]
	@HPIXML XML,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

DECLARE @HPID_ID TABLE (ID BIGINT);

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	INSERT INTO [Clinical].[HPIDetail]
    (
		[HPIID],
		[Comment],
		[Location],
		[HPISeverityID],
		[Quality],
		[Duration],
		[Timing],
		[Context],
		[ModifyingFactors],
		[Symptoms],
		[Conditions],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		[CreatedBy],
		[CreatedOn]
	)
	OUTPUT inserted.HPIDetailID
	INTO @HPID_ID
	SELECT
		T.C.value('HPIID[1]','BIGINT') as HPIID,
		T.C.value('Comment[1]','NVARCHAR(2000)') as Comment,
		T.C.value('Location[1]','NVARCHAR(255)') as Location,
		T.C.value('HPISeverityID[1]','SMALLINT') as HPISeverityID,
		T.C.value('Quality[1]','NVARCHAR(255)') as Quality,
		T.C.value('Duration[1]','NVARCHAR(255)') as Duration,
		T.C.value('Timing[1]','NVARCHAR(255)') as Timing,
		T.C.value('Context[1]','NVARCHAR(255)') as Context,
		T.C.value('ModifyingFactors[1]','NVARCHAR(255)') as ModifyingFactors,
		T.C.value('Symptoms[1]','NVARCHAR(255)') as Symptoms,
		T.C.value('Conditions[1]','NVARCHAR(255)') as Conditions,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM 
		@HPIXML.nodes('HPI/HPIDetails') AS T(C)

	DECLARE @AuditCursor CURSOR;
	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ID FROM @HPID_ID;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Clinical', 'HPIDetail', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Clinical', 'HPIDetail', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

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


