
----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_PurgeBatchLogData]
-- Author:		Sumana Sangapu
-- Date:		09/16/2016
--
-- Purpose:		Purge Batch Log Data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/16/2016	Sumana Sangapu		Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Synch].[usp_PurgeBatchLogData]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
			SELECT  @ResultCode = 0,
					@ResultMessage = 'executed successfully'

			BEGIN TRY 

				DECLARE @days INT

				DECLARE @TmpBatchIDSinUse TABLE ( BatchID int )
				DECLARE @TmpBatchIDs TABLE (BatchID int)

				SELECT @days = convert(int,ConfigXML) FROM Synch.Config WHERE ConfigName = 'NumberOfDaysforLogging'
				
				-- This logic will handle situations when either of the packages has failed and Batch details fall within the criteria of purging.
				-- Ex: BatchID 236 is stuck in any of the below tables, and it needs to be purged as per the 'NumberofDaysForLogging' criteria, 
				-- the DELETE statement will not be completed due to foreign key. Ideally this situation should be handled by correcting the package and 
				-- have a successful run. The logic below will avoid the Foreign key constraint error to be thrown. In a happy path the BatchIDs in the Staging tables will be from the latest runs.
				INSERT INTO @TmpBatchIDSinUse
				SELECT DISTINCT BatchID FROM [Synch].[CMHCClientAddress]
				UNION
				SELECT DISTINCT BatchID FROM [Synch].[CMHCClientRecordedServices]
				UNION
				SELECT DISTINCT BatchID FROM [Synch].[CMHCClientRegistration]
				UNION
				SELECT DISTINCT BatchID FROM [Synch].[CMHCContactPayorDetails]
				UNION
				SELECT DISTINCT BatchID FROM [Synch].[CMHCSelfPay]

				INSERT INTO @TmpBatchIDs
				SELECT	BatchID 
				FROM	Synch.BATCH b
				WHERE	CONVERT(date,CreatedOn) <  CONVERT(date,DATEADD(dd, -@days, getdate()-1))
				AND		BatchID NOT IN (SELECT BatchID FROM @TmpBatchIDSinUse)
			
				-- Synch.BatchErrorDetails
				DELETE FROM Synch.BatchErrorDetails WHERE BatchID IN ( SELECT BatchID FROM @TmpBatchIDs  )

				-- Synch.CMHCFilesRecordCount
				DELETE FROM Synch.CMHCFilesRecordCount WHERE BatchID  IN (SELECT BatchID FROM @TmpBatchIDs)

				-- Synch.Batch
				DELETE FROM Synch.Batch WHERE BatchID IN (SELECT BatchID FROM @TmpBatchIDs)



			END TRY
			BEGIN CATCH
				SELECT @ResultCode = ERROR_SEVERITY(),
					   @ResultMessage = ERROR_MESSAGE()
			END CATCH
END