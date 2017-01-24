		
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_InsertGenerator
-- Author:		John Crossen
-- Date:		07/28/2015
--
-- Purpose:		Auto Generate seed data scripts with if not exists checks
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/28/2015	John Crossen		TFS#  - Initial creation.
-- 09/14/2015	Sumana Sangapu		Refactored the insert script generator to use MERGE statement and handle existing data
-- 1/15/2016    John Crossen        Modified proc so the new date fields will return GETUTCDATE() value
-----------------------------------------------------------------------------------------------------------------------



IF EXISTS (SELECT * FROM dbo.sysobjects 
WHERE id = OBJECT_ID(N'[dbo].[usp_InsertGenerator]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[usp_InsertGenerator]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
SET NOCOUNT ON
GO

-- execute  [usp_InsertGenerator] @tableName='ContactTypeHeirarchy', @KeyColumn1='ContactTypeHeirarchyID',@TableSchema='Reference'

CREATE PROC [dbo].[usp_InsertGenerator]
(
@tableName varchar(100),
@TableSchema varchar(100),
@KeyColumn1 varchar(100)='',
@KeyColumn2 varchar(100)=''
)
AS
BEGIN
      
 
		DECLARE @vsSQL nvarchar(1000) -- used for Create temp table query 
		DECLARE @string nvarchar(max) --for storing the first half of INSERT statement
		DECLARE @Query nvarchar(max)
		DECLARE @stringData nvarchar(max) --for storing the data (VALUES) related statement
		DECLARE @stringUpdate nvarchar(max) -- for generating the update statement
		DECLARE @StringInsertRef nvarchar(max) -- for generating the Insert statement into main reference table
		DECLARE @StringInsertRefValues nvarchar(max) -- for generating the VALUES statement into main reference table
		DECLARE @StringMerge nvarchar(max) -- for generating the MERGE statement
		DECLARE @StringPK nvarchar(max) -- for generating PK joins
		DECLARE @dataType nvarchar(1000) --data types returned for respective columns
		DECLARE @FieldVal nvarchar(1000) -- save value for the current field
		DECLARE @KeyVal nvarchar(1000) -- save value for the current field
		DECLARE @KeyTest0 nvarchar(1000) -- used to test if key exists
		DECLARE @KeyTest1 nvarchar(1000) -- used to test if key exists
		DECLARE @KeyTest2 nvarchar(1000) -- used to test if key exists
		DECLARE @ConstraintType nvarchar(20)
		DECLARE @OrdinalPosition int
		DECLARE @colName nvarchar(50) 

		----------------------------------------------------------------------
		PRINT  'SET IDENTITY_INSERT ['+@TableSchema +'].['+@tableName+'] ON'
		----------------------------------------------------------------------

		/******************* Generate the temp table *****************************************/
		select @vsSQL = 'DECLARE @' + @TableName + char(10) + ' TABLE (' + char(10)
		select @vsSQL = @vsSQL + ' ' + sc.COLUMN_NAME + ' ' +
		sc.DATA_TYPE +
		case when DATA_TYPE in ('varchar','nvarchar','char','nchar') then '(' + cast(sc.CHARACTER_MAXIMUM_LENGTH as varchar) + ') ' else ' ' end +
		case when sc.IS_NULLABLE = 'YES' then 'NULL' else 'NOT NULL' end + ',' + char(10)
		from information_schema.columns sc 
		where  sc.TABLE_SCHEMA = @TableSchema 
		and sc.table_name = @TableName
		order by sc.ORDINAL_POSITION
 
		select @vsSQL = substring(@vsSQL,1,len(@vsSQL) - 2) + char(10) + ')'

		SET @string='INSERT INTO @'+@tableName+'('

		/***************** Declare a cursor to retrieve column specific information for the specified table ********************************/
		DECLARE cursCol CURSOR FAST_FORWARD FOR 
		SELECT DISTINCT column_name, DATA_TYPE , CONSTRAINT_TYPE, ORDINAL_POSITION FROM 
		(
				SELECT sc.COLUMN_NAME as column_name, sc.DATA_TYPE as DATA_TYPE 
				, CASE WHEN tc.CONSTRAINT_TYPE IS NULL OR tc.CONSTRAINT_TYPE <> 'PRIMARY KEY' THEN 'Not Key' ELSE tc.CONSTRAINT_TYPE END AS CONSTRAINT_TYPE, sc.ORDINAL_POSITION
				from  information_schema.columns sc 
				left JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu
				ON	sc.COLUMN_NAME = ccu.COLUMN_NAME
				AND	sc.TABLE_SCHEMA = ccu.TABLE_SCHEMA
				AND sc.TABLE_NAME = ccu.TABLE_NAME
				full  join INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
				ON tc.CONSTRAINT_NAME = ccu.Constraint_name
				WHERE sc.TABLE_SCHEMA = @TableSchema
				AND sc.TABLE_NAME = @tableName
		)a
		order by ORDINAL_POSITION asc , CONSTRAINT_TYPE desc

		OPEN cursCol

		SET @KeyTest0=''
		SET @KeyTest1=''
		SET @KeyTest2=''

		-----------------------------------
		PRINT @vsSQL
		-----------------------------------

		SET @string='INSERT INTO @'+@tableName +' ( '
		SET @stringUpdate='UPDATE SET '
		SET @StringInsertRef = 'INSERT ('
		SET @StringInsertRefValues = 'VALUES ('


		SET @stringData=''
		SET @FieldVal=''
		SET @KeyVal=''

		FETCH NEXT FROM cursCol INTO @colName,@dataType,@ConstraintType, @OrdinalPosition

		IF @@fetch_status<>0
		begin
			print 'Table '+@tableName+' not found, processing skipped.'
			close curscol
			deallocate curscol
			return
		END

		WHILE @@FETCH_STATUS=0
		BEGIN
						
						-- Generate the column list 
						IF @dataType in ('varchar','char','nchar','nvarchar')
						BEGIN
							SET @FieldVal=''''+'''+isnull('''''+'''''+'+@colName+'+'''''+''''',''NULL'')+'',''+'
							SET @KeyVal='''+isnull('''''+'''''+'+@colName+'+'''''+''''',''NULL'')+'',''+'
							SET @stringData=@stringData+@FieldVal
						END

						ELSE

						if @dataType in ('text','ntext','xml') --if the datatype is text or something else 
						BEGIN
							SET @FieldVal='''''''''+isnull(cast('+@colName+' as varchar(max)),'''')+'''''',''+'
							SET @stringData=@stringData+@FieldVal
						END
						ELSE
						IF @dataType = 'money' --because money doesn't get converted from varchar implicitly
						BEGIN
							SET @FieldVal='''convert(money,''''''+isnull(cast('+@colName+' as varchar(200)),''0.0000'')+''''''),''+'
							SET @stringData=@stringData+@FieldVal
						END
						ELSE 
						IF @dataType='datetime'
						BEGIN

								IF @colName IN ('ModifiedOn','SystemCreatedOn','SystemModifiedOn','CreatedOn')
								BEGIN
										SET @FieldVal='''GETUTCDATE(),''+'
								END
								IF @colName NOT IN  ('ModifiedOn','SystemCreatedOn','SystemModifiedOn','CreatedOn')
								BEGIN
										SET @FieldVal='''convert(datetime,'+'''+isnull('''''+'''''+convert(varchar(200),'+@colName+',121)+'''''+''''',''NULL'')+'',121),''+'
								END
								SET @stringData=@stringData+@FieldVal
						END
						ELSE 
						IF @dataType='image' 
						BEGIN
							SET @FieldVal='''''''''+isnull(cast(convert(varbinary,'+@colName+') as varchar(6)),''0'')+'''''',''+'
							SET @stringData=@stringData+@FieldVal
						END
						ELSE --presuming the data type is int,bit,numeric,decimal 
						BEGIN
							SET @FieldVal=''''+'''+isnull('''''+'''''+convert(varchar(200),'+@colName+')+'''''+''''',''NULL'')+'',''+'
							SET @KeyVal='''+isnull('''''+'''''+convert(varchar(200),'+@colName+')+'''''+''''',''NULL'')+'',''+'
							SET @stringData=@stringData+@FieldVal
						END

						--Build key test
						IF @KeyColumn1=@colName
						begin
							SET @KeyTest1 = ' WHERE [' + @KeyColumn1 + ']='
							SET @KeyTest1 = @KeyTest1+@KeyVal+']'
						end
						IF @KeyColumn2=@colName
						begin
							SET @KeyTest2 = ' AND [' + @KeyColumn2 + ']='
							SET @KeyTest2 = @KeyTest2+@KeyVal+']'
						end

						-- Generate the Insert into Temp table statement
						SET @string=@string+'['+@colName+'],'

						-- Generate the Update statement 
						IF @ConstraintType = 'Not Key' -- Ignore the PrimaryKey for the Update statement
							SET @stringUpdate= @stringUpdate+ '['+@colName +']' +'= source.['+@colName+'],'
						
						-- Generate the Insert statement into Reference table
						SET @StringInsertRef= @StringInsertRef+ '['+@colName +'], ' 
						SET @StringInsertRefValues = @StringInsertRefValues + ' source.['+@colName+'],'

			FETCH NEXT FROM cursCol INTO @colName,@dataType,@ConstraintType, @OrdinalPosition
			END -- End of Cursor
						

			/********************************************* Remove the last comma ********************************************************/
			SET @stringUpdate = SUBSTRING(@stringUpdate,1,len(@stringUpdate)-1)
			SET @StringInsertRef = SUBSTRING(@StringInsertRef,1,len(@StringInsertRef)-1) + ' )'
			SET @StringInsertRefValues = SUBSTRING(@StringInsertRefValues,1,len(@StringInsertRefValues)-1)  + ')' 
					
			-- Build Insert into table statements
			if @KeyTest0<>''
			begin
				if @Keycolumn1<>''
					SET @KeyTest0 = @KeyTest0 + substring(@KeyTest1,0,len(@KeyTest1)-4)
					
					if @Keycolumn2<>''
					begin
						SET @KeyTest0 = @KeyTest0 + ''''
						SET @KeyTest0 = @KeyTest0 + substring(@KeyTest2,0,len(@KeyTest2)-4)
					end
					SET @KeyTest0 = @KeyTest0 + ''')'

					SET @query = 'SELECT '''+substring(@KeyTest0,0,len(@KeyTest0)) + ') ' 
			end
			else
			    SET @query ='SELECT '''+substring(@KeyTest0,0,len(@KeyTest0))
	
			SET @query = @query + '(''+ ' + substring(@stringData,0,len(@stringData)-2)+'''+''), '' FROM ['+@TableSchema +'].['+@tableName+']'

			-------------------------------------------------------------------------
			PRINT SUBSTRING(@string,1,len(@string)-1) + ' ) VALUES '

			exec sp_executesql  @query 
			-------------------------------------------------------------------------

			CLOSE cursCol
			DEALLOCATE cursCol

			/********************************** Determine the Primary Keys on the table ****************************************/

			DECLARE cursPK CURSOR FAST_FORWARD FOR 
			SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
			JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu ON tc.CONSTRAINT_NAME = ccu.Constraint_name
			WHERE tc.CONSTRAINT_TYPE = 'Primary Key'
			AND tc.TABLE_SCHEMA = @TableSchema
			AND tc.TABLE_NAME = @TableName

			OPEN cursPK

			FETCH NEXT FROM cursPK INTO @colName 

			IF @@fetch_status<>0
			begin
				print 'Table '+@tableName+' not found, processing skipped.'
				close curscol
				deallocate curscol
				return
			END

			WHILE @@FETCH_STATUS=0
			BEGIN
				SET @StringPK = ' SOURCE.['+@Colname+'] = TARGET.['+@Colname+'] AND'

				FETCH NEXT FROM cursPK INTO @colName 
			END
			
			/***************************************** Build the Merge statement **************************************************/
			SET @query = SUBSTRING(@query,1,len(@query)-1) + substring(@string,0,len(@string)) + ') ' 
			SET @stringMerge = 'MERGE INTO ['+@TableSchema +'].['+@tableName+'] AS TARGET USING ( SELECT * FROM @'+@tableName +') AS SOURCE ON '
			SET @StringMerge = @StringMerge + SUBSTRING(@StringPK,1,len(@StringPK)-3) + ' WHEN MATCHED THEN ' + @stringUpdate 
			SET @stringMerge = @stringMerge + ' WHEN NOT MATCHED THEN '+@StringInsertRef + @StringInsertRefValues
			SET @stringMerge = @stringMerge + ' WHEN NOT MATCHED BY SOURCE THEN DELETE ;'

			-------------------------------------------------------------------------------------
			PRINT @StringMerge

			PRINT  'SET IDENTITY_INSERT ['+@TableSchema +'].['+@tableName+'] OFF'
			-------------------------------------------------------------------------------------
			
			CLOSE cursPK
			DEALLOCATE cursPK
END
