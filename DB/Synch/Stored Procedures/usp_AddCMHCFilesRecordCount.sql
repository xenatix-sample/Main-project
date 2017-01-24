-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_AddCMHCFilesRecordCount]
-- Author:		Sumana Sangapu
-- Date:		08/16/2016
--
-- Purpose:		CMHC File Record count  
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/16/2016	Sumana Sangapu		Initial creation
-----------------------------------------------------------------------------------------------------------------------

 
 CREATE PROCEDURE [Synch].[usp_AddCMHCFilesRecordCount]
 @BatchID INT,
 @BatchTypeID INT,
 @DestinationCount INT 

 AS 
 BEGIN 
 
 

			DECLARE @TableName nvarchar(100) 
			DECLARE @sql nvarchar(4000)
			
			IF @BatchTypeID = '3' -- CMHC Registration
				SET @TableName = '[Synch].[CMHCClientRegistration]'
			ELSE IF @BatchTypeID = '4' -- CMHC Addresses
				SET @TableName =  '[Synch].[CMHCClientAddress]'
			ELSE IF @BatchTypeID = '5' -- CMHC SelfPay
				SET @TableName =  '[Synch].[CMHCSelfPay]'
			ELSE IF @BatchTypeID = '6' -- CMHC ContactPayor 
				SET @TableName =  '[Synch].[CMHCContactPayorDetails]'
			ELSE IF @BatchTypeID = '7' -- CMHC RecordedServices 
				SET @TableName =  '[Synch].[CMHCClientRecordedServices]'

			SET @sql = ''
			
			SET @sql = @sql + 'INSERT INTO [Synch].[CMHCFilesRecordCount] (BatchID, BatchTypeID, SourceRecordCount, DestinationRecordCount, CreatedBy,CreatedOn) '
			
			SET @sql = @sql + ' SELECT ' + convert(nvarchar(10),@BatchID)+' , '+ convert(nvarchar(10),@BatchTypeID)+',  Convert(nvarchar(10),count(*)) ,' + convert(nvarchar(10),@DestinationCount)+', 1, GETUTCDATE() FROM ' + @TableName  
		
			Execute sp_executesql @sql
 
 

END

  