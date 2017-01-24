----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddAuditLog]
-- Author:		Scott Martin
-- Date:		1/26/2016
--
-- Purpose:		Logs the values of a record 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/26/2016	Scott Martin		Initial creation.
-- 02/29/2016	Rajiv Ranjan		Adedd SET NOCOUNT ON for validating unit test case.
-- 09/15/2016	Scott Martin		Added parameters for ModuleComponentID, EntityID, and EntityTypeID
-- 09/16/2016	Scott Martin		Moved to Auditing Schema
-- 09/19/2016	Scott Martin		Mapped TransactionLogID, ModuleComponentID, EntityID, EntityTypeID, and TableCatalogID to appropriate table values
-- 09/21/2016	Scott Martin		Fixed a bug where audit log wouldn't save if TransactionID/ModuleComponent ID were 0
-- 10/26/2016	Scott Martin		Changed how XML data was being cast
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Auditing].[usp_AddAuditLog]
	@PreOrPost NVARCHAR(4),
	@AuditSource NVARCHAR(255) = NULL,
	@AuditType NVARCHAR(50),
	@TableSchema NVARCHAR(255),
	@TableName NVARCHAR(255),
	@AuditDetailID BIGINT = 0,
	@PrimaryKeyValue BIGINT = 0,
	@ReasonText NVARCHAR(MAX) = NULL,
	@SelectQuery NVARCHAR(MAX) = NULL,
	@TransactionLogID BIGINT = NULL,
	@ModuleComponentID BIGINT = NULL,
	@EntityID BIGINT = NULL,
	@EntityTypeID INT = NULL,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
SET NOCOUNT ON

DECLARE @PrimaryKeyColumnName NVARCHAR(255),
		@ColumnList NVARCHAR(MAX),
		@SQLCommand NVARCHAR(MAX),
		@Node XML,
		@AuditXMLString NVARCHAR(MAX),
		@AuditXML XML,
		@AuditPre XML,
		@AuditPost XML,
		@AuditID BIGINT,
		@TableCatalogID INT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
		SELECT @TableCatalogID = TableCatalogID FROM Reference.TableCatalog TC WHERE TC.SchemaName = @TableSchema AND TC.TableName = @TableName;

		IF [Core].[fn_GetAuditType](@AuditType) IN (1)
			BEGIN
			SET @Node = CONCAT('<', @TableSchema, '.', @TableName, ' SelectQuery="', @SelectQuery,'" />');
			SET @SelectQuery = CONCAT('SET @AuditXMLString = CONVERT(NVARCHAR(MAX), (', @SelectQuery, ' FOR XML AUTO))');

			EXEC sp_executesql @SelectQuery, N'@AuditXMLString NVARCHAR(MAX) OUTPUT', @AuditXML OUTPUT;

			SET @AuditXML = @AuditXMLString;
			
			SET @AuditXML.modify('insert sql:variable("@Node") as first into (/)[1]');
			END

		IF ([Core].[fn_GetAuditType](@AuditType) IN (2, 3) OR ([Core].[fn_GetAuditType](@AuditType) = 4 AND @PreOrPost = 'Post'))
			BEGIN
			SELECT DISTINCT
				@PrimaryKeyColumnName = sc.COLUMN_NAME
			FROM
				INFORMATION_SCHEMA.COLUMNS sc 
				LEFT JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu
					ON	sc.COLUMN_NAME = ccu.COLUMN_NAME
					AND	sc.TABLE_SCHEMA = ccu.TABLE_SCHEMA
					AND sc.TABLE_NAME = ccu.TABLE_NAME
				FULL OUTER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
					ON tc.CONSTRAINT_NAME = ccu.CONSTRAINT_NAME
			WHERE
				tc.CONSTRAINT_TYPE = 'PRIMARY KEY'
				AND sc.TABLE_SCHEMA = @TableSchema
				AND sc.TABLE_NAME = @TableName;

			SET @ColumnList =
			(SELECT
				', ' + CASE WHEN C.COLUMN_NAME IN ('Password') THEN CONCAT('CAST(', C.COLUMN_NAME, ' AS VARBINARY) AS ', C.COLUMN_NAME) ELSE C.COLUMN_NAME END
			FROM
				INFORMATION_SCHEMA.COLUMNS As C
			WHERE
				C.TABLE_SCHEMA = @TableSchema
				AND C.TABLE_NAME = @TableName
			ORDER BY
				C.ORDINAL_POSITION
			FOR XML PATH (''))

			SET @ColumnList = RIGHT(@ColumnList, LEN(@ColumnList) - 2)

			SET @SQLCommand = CONCAT('SET @AuditXMLString = CONVERT(NVARCHAR(MAX), (SELECT ', @ColumnList, ' FROM ', @TableSchema, '.', @TableName, ' WHERE ', @PrimaryKeyColumnName, ' = ', @PrimaryKeyValue, ' FOR XML AUTO, BINARY BASE64))');

			EXEC sp_executesql @SQLCommand, N'@AuditXMLString NVARCHAR(MAX) OUTPUT', @AuditXMLString OUTPUT;

			SET @AuditXML = @AuditXMLString;
			END

		IF @PreOrPost = 'Pre'
			BEGIN
			SET @AuditPre = @AuditXML;
			SET @AuditPost = NULL;
			END

		IF @PreOrPost = 'Post'
			BEGIN
			SET @AuditPre = NULL;
			SET @AuditPost = @AuditXML;
			END
		
		IF ISNULL(@AuditDetailID, 0) = 0
			BEGIN
			INSERT INTO Auditing.Audits
			(
				TransactionLogID,
				ModuleComponentID,
				AuditTypeID ,
				UserID,
				CreatedOn,
				AuditTimeStamp ,
				IsArchivable
			)
			VALUES
			(
				CASE WHEN @TransactionLogID > 0 THEN @TransactionLogID ELSE NULL END,
				CASE WHEN @ModuleComponentID > 0 THEN @ModuleComponentID ELSE NULL END, 
				[Core].[fn_GetAuditType](@AuditType),
				@ModifiedBy ,
				@ModifiedOn, 
				GETUTCDATE() , 
				0  
			);
			END
			
		SELECT @AuditID = SCOPE_IDENTITY();

		IF @ReasonText IS NOT NULL
			BEGIN
			IF ISNULL(@AuditID, 0) = 0
				BEGIN
				SET @AuditID = (SELECT AuditID FROM Core.AuditDetail WHERE AuditDetailID = @AuditDetailID)
				END

			INSERT INTO Auditing.AuditReason VALUES(@AuditID, @ReasonText);
			END

		DECLARE @AD_ID TABLE (AuditDetailID BIGINT);

		MERGE Auditing.AuditDetail AS TARGET
		USING (SELECT ISNULL(@AuditDetailID, 0) AS AuditDetailID) AS SOURCE
			ON TARGET.AuditDetailID = SOURCE.AuditDetailID
		WHEN NOT MATCHED THEN
			INSERT
			(
				AuditID,
				AuditPre,
				AuditPost,
				AuditPrimaryKeyValue,
				AuditSource,
				EntityID,
				EntityTypeID,
				TableCatalogID
			)
			VALUES
			(
				@AuditID,
				@AuditPre,
				@AuditPost,
				ISNULL(@PrimaryKeyValue, 0),
				@AuditSource,
				@EntityID,
				@EntityTypeID,
				@TableCatalogID
			)
		WHEN MATCHED THEN
			UPDATE
			SET TARGET.AuditPrimaryKeyValue = ISNULL(@PrimaryKeyValue, 0),
				TARGET.AuditPost = @AuditPost
		OUTPUT inserted.AuditDetailID
		INTO @AD_ID;

		SELECT TOP 1 @ID = AuditDetailID FROM @AD_ID;
			
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE();
	END CATCH
END