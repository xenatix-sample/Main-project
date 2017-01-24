-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_RebuildIndexes
-- Author:		Scott Martin
-- Date:		09/22/2016
--
-- Purpose:		Used to rebuild database indexes
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/22/2016	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_RebuildIndexes]
(
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
)
AS
BEGIN
SELECT @ResultCode = 0,
		@ResultMessage = 'Indexes rebuilt successfully';

BEGIN TRY
	DECLARE @SQLCommand NVARCHAR(MAX)

	CREATE TABLE #TableCatalog
	(
		TableCatalogID INT,
		SchemaName VARCHAR(255),
		TableName VARCHAR(255),
		Complete BIT DEFAULT(0)
	);

	INSERT INTO #TableCatalog
	(
		TableCatalogID,
		SchemaName,
		TableName
	)
	SELECT
		TableCatalogID,
		SchemaName,
		TableName
	FROM
		Reference.TableCatalog TC
		INNER JOIN sys.objects O
			ON TC.TableName = O.name
		INNER JOIN sys.indexes I
			ON O.object_id = I.object_id
	WHERE
		I.is_primary_key <> 1
		AND TC.IsActive = 1
	GROUP BY
		TableCatalogID,
		SchemaName,
		TableName;

	DECLARE @PKID BIGINT, @Loop INT

	SELECT @Loop = COUNT(*) FROM #TableCatalog WHERE Complete = 0;

	WHILE @LOOP > 0

		BEGIN

		SET ROWCOUNT 1
		SELECT @PKID = TableCatalogID FROM #TableCatalog WHERE Complete = 0;
		SET ROWCOUNT 0

		SELECT
			@SQLCommand = CONCAT('ALTER INDEX ALL ON ', SchemaName, '.', TableName, ' REBUILD WITH (FILLFACTOR = 95, SORT_IN_TEMPDB = ON, STATISTICS_NORECOMPUTE = ON)')
		FROM
			#TableCatalog TC
		WHERE
			TableCatalogID = @PKID
			AND SchemaName NOT IN ('sys');

		EXEC sp_executesql @SQLCommand

		print @SQLCommand;

		SELECT @Loop = @Loop - 1;
		UPDATE #TableCatalog
		SET Complete = 1
		WHERE
			TableCatalogID = @PKID;
		END

	DROP TABLE #TableCatalog;

END TRY

BEGIN CATCH
	SELECT @ResultCode = ERROR_SEVERITY(),
			@ResultMessage = ERROR_MESSAGE()
END CATCH
END