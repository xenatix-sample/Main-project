-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_AddBatch]
-- Author:		Chad Roberts
-- Date:		1/26/2016
--
-- Purpose:		Add a Batch
--
-- Notes:		n/a
--
-- Depends:		n/a
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/26/2016	Chad Roberts	 Initial creation.
-- 09/03/2016	Rahul Vats		Review the proc
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Synch].[usp_AddBatch]
	@BatchStatusID INT,
	@BatchTypeID INT,
	@ConfigID INT,
	@USN BIGINT,
	@IsActive bit = 1,
	@ModifiedOn DateTime,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
	DECLARE @AuditPost XML,
			@AuditID BIGINT;
	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	if (@ModifiedBy = 0)
	Begin
		select @ModifiedBy = 1
	End

	INSERT INTO Synch.Batch
	(
		BatchStatusID,
		BatchTypeID,
		ConfigID,
		USN,
		IsActive,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@BatchStatusID,
		@BatchTypeID,
		@ConfigID,
		@USN,
		@IsActive,
		@ModifiedBy,
		@ModifiedOn
	);
		
	SELECT @ID = SCOPE_IDENTITY();

	-- NOTE: Module record needs to be created
	DECLARE @TableCatalogID INT;
	SELECT @TableCatalogID = TableCatalogID FROM Reference.TableCatalog TC WHERE TC.SchemaName = 'Synch' AND TC.TableName = 'Batch';

	INSERT INTO Auditing.Audits
	(
	    AuditTypeID ,
	    UserID ,
		CreatedOn,
	    AuditTimeStamp ,
	    IsArchivable
	)
	VALUES
	(
		[Core].[fn_GetAuditType]('Insert'),
	    @ModifiedBy ,
		@ModifiedOn,
	    GETUTCDATE() , 
	    0  
	);
			
	SELECT @AuditID = SCOPE_IDENTITY();

	INSERT INTO Auditing.AuditDetail
	(
		AuditID,
		AuditPost,
		AuditPrimaryKeyValue,
		TableCatalogID
	)
	SELECT
		@AuditID,
		(SELECT * FROM Synch.Batch WHERE BatchID = tbl.BatchID FOR XML AUTO),
		tbl.BatchID,
		@TableCatalogID
	FROM
		Synch.Batch tbl
	WHERE
		tbl.BatchID = @ID;

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO