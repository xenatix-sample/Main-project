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

GO
IF EXISTS(SELECT 'X' FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.tempMergedContactsMapping'))
	BEGIN
	SET IDENTITY_INSERT Core.MergedContactsMapping ON;

	INSERT INTO Core.MergedContactsMapping
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
	FROM
		dbo.tempMergedContactsMapping;

	SET IDENTITY_INSERT Core.MergedContactsMapping OFF;

	DROP TABLE dbo.tempMergedContactsMapping;
	END

GO