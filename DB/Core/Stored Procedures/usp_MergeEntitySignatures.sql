-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_MergeEntitySignatures
-- Author:		Scott Martin
-- Date:		09/21/2016
--
-- Purpose:		Main procedure for Client Eligibility
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/21/2016	Scott Martin	Initial procedure creation
-- 10/10/2016	Scott Martin	Changed schema reference on audit proc
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Core].[usp_MergeEntitySignatures]
(
	@MergedContactsMappingID BIGINT,
	@ParentID BIGINT,
	@ChildID BIGINT,
	@TotalRecords INT OUTPUT,
	@TotalRecordsMerged INT OUTPUT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
)
AS
BEGIN
	SELECT @ResultCode = 0,
	
	@ResultMessage = 'Data saved successfully'

	BEGIN TRY	

	DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID)

	-- for audit purposes, getting a list of IDs to update and looping through them.   Need an audit solution that can handle a simple update table set ContactID=someotherid with the audit.
	CREATE TABLE #IDstoUpdate (PKID bigint, completed tinyint DEFAULT(0))

	INSERT INTO #IDstoUpdate (PKID, completed)
	SELECT
		EntitySignatureID,
		0
	FROM
		ESignature.EntitySignatures
	WHERE
		EntityID = @ChildID
		AND EntityTypeID = 2;

	DECLARE @PKID BIGINT, @Loop INT

	SELECT @Loop = COUNT(*) FROM #IDstoUpdate;
	SET @TotalRecords = @Loop;
	SET @TotalRecordsMerged = @Loop;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = PKID FROM #IDstoUpdate WHERE completed = 0;
	SET ROWCOUNT 0
		
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'ESignature', 'EntitySignatures', @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE ESignature.EntitySignatures
	SET EntityID = @ParentID,
		ModifiedOn = @ModifiedOn,
		ModifiedBy = @ModifiedBy
	WHERE
		EntitySignatureID = @PKID;
		
	EXEC Auditing.usp_AddPostAuditLog 'Update', 'ESignature', 'EntitySignatures', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	INSERT INTO Core.MergedContactAuditDetail (MergedContactsMappingID, AuditDetailID) VALUES (@MergedContactsMappingID, @AuditDetailID);

	SELECT @Loop = @Loop - 1;
	UPDATE #IDstoUpdate
	SET completed = 1
	WHERE
		PKID = @PKID;
	END

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END