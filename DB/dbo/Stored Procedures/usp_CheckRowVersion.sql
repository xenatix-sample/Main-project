-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[dbo].[usp_CheckRowVersion]
-- Author:		Gurpreet Singh
-- Date:		11/15/2016
--
-- Purpose:		Concurrency handling
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/15/2016	Gurpreet Singh	Added sproc
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[usp_CheckRowVersion] 
    @tsTimeMain DATETIME, -- time stamp which is to be Checked(For now it is being set on SystemModifiedOn, later need to change it to RowVersion)
    @tableName  VARCHAR(100), --Table name it has to Check.
    @FeildName  VARCHAR(100), --Primary key name for the table.
    @FeildValue INT --Primary key value.
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @strMain VARCHAR(2000);

	IF(@tsTimeMain IS NOT NULL)
		BEGIN
			DECLARE @SystemModifiedDate varchar(100) = CONVERT(VARCHAR(24),@tsTimeMain,113)
			SET @strMain = 'declare @time datetime
				select @time = SystemModifiedOn from '+@tableName+' Where '+@FeildName+' ='+CAST(@FeildValue AS VARCHAR(100))+'
				if(Convert(nvarchar(200),Cast('''+@SystemModifiedDate+''' as DATETIME),113)=Convert(nvarchar(200),Cast(@time as datetime),113))
					begin
						return 
					end
				else
					begin
						RAISERROR (''Data has been changed. Please reload the page.'', 15, 1);
					end'
                     EXEC (@strMain);
              END;
		ELSE
			BEGIN
				RAISERROR('Data has been changed. Please reload the page.', 15, 1);
			END;
END;

