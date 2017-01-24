-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_GetLastBatch]
-- Author:		Chad Roberts
-- Date:		1/26/2016
--
-- Purpose:		Get a configuration for a service
--
-- Notes:		n/a
--
-- Depends:		n/a
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/26/2016	Chad Roberts	Initial creation.
-- 09/03/2016	Rahul Vats		Review the proc
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Synch].[usp_GetLastBatch]
	@ConfigID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
		SELECT @ResultCode = 0,
			@ResultMessage = 'executed successfully'
		
		SELECT top 1 BatchID, BatchStatusID, BatchTypeID, ConfigID, USN, ModifiedBy, ModifiedOn
		FROM Synch.Batch
		WHERE IsActive=1 AND ConfigID=@ConfigID
		order by BatchID desc
	  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO