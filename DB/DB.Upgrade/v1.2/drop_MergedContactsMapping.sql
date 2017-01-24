-----------------------------------------------------------------------------------------------------------------------
-- Table:		Core.MergedContactsMapping
-- Author:		Scott Martin
-- Date:		01/18/2017
--
-- Purpose:		Script used to migrated existing data to new table schema
--
-- Notes:		
--
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/18/2017	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS(
    SELECT 'X'
    FROM
		sys.columns 
    WHERE
		Name = 'IsUnMergeAllowed'
		AND Object_ID = Object_ID(N'Core.MergedContactsMapping'))
	BEGIN


	CREATE TABLE [dbo].[tempMergedContactsMapping](
		[MergedContactsMappingID] [BIGINT] NULL,
		[TransactionLogID] [BIGINT] NULL,
		[ContactID] [BIGINT] NULL,
		[ParentID] [BIGINT] NULL,
		[ChildID] [BIGINT] NULL,
		[IsParentPrimary] [BIT] NOT NULL DEFAULT(1),
		[MergeDate] datetime NOT NULL,
		[IsUnMergeAllowed] BIT NOT NULL DEFAULT(0),
		[IsActive] BIT NOT NULL DEFAULT(1),
		[ModifiedBy] INT NOT NULL,
		[ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
		[CreatedBy] INT NOT NULL DEFAULT(1),
		[CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
		[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
		[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),	
	);

	DECLARE @TransactionLogID BIGINT

	CREATE TABLE #TLog (TransactionLogID NVARCHAR(16));

	INSERT INTO #TLog EXEC Core.usp_GenerateTransactionLogID 1, NULL, NULL;

	SELECT @TransactionLogID = TransactionLogID FROM #TLog;

	DROP TABLE #TLog;

	INSERT INTO dbo.tempMergedContactsMapping
	(
		MergedContactsMappingID,
		TransactionLogID,
		ContactID,
		ParentID,
		ChildID,
		IsParentPrimary,
		MergeDate,
		IsUnMergeAllowed,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn,
		SystemCreatedOn,
		SystemModifiedOn
	)
	SELECT
		MergedContactsMappingID,
		@TransactionLogID,
		ParentID,
		ParentID,
		ChildID,
		IsParentPrimary,
		MergeDate,
		0,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn,
		SystemCreatedOn,
		SystemModifiedOn
	FROM
		Core.MergedContactsMapping;

	ALTER TABLE Core.MergedContactResult DROP CONSTRAINT FK_MergedContactResult_MergedContactsMappingID;

	DROP TABLE Core.MergedContactsMapping;
	END