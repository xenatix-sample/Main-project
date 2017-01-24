-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetClientMergeCounts]
-- Author:		Scott Martin
-- Date:		11/15/2016
--
-- Purpose:		Get counts for potential merge contacts and contacts that have been merged
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/15/2016	Scott Martin	Initial creation.
-- 12/21/2016	Scott Martin	Proc will now populate Potential Match table 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetClientMergeCounts]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN	
	BEGIN TRY
		SELECT
			@ResultCode = 0,
			@ResultMessage = 'Executed successfully' 

	DECLARE @PCMDate DATE,
			@ModifiedBy INT = 1,
			@ModifiedOn DATETIME = GETDATE();

	SELECT @PCMDate = MAX(PCM.CreatedOn) FROM Core.PotentialContactMatches PCM;

	IF DATEDIFF(DAY, GETDATE(), @PCMDate) <> 0 OR @PCMDate IS NULL
		BEGIN
		EXEC Core.usp_AddPotentialMergeContacts @ModifiedOn, 1, @ResultCode OUTPUT, @ResultMessage OUTPUT;
		END

	SELECT
		(SELECT COUNT(*) FROM Core.PotentialContactMatches WHERE IsActive = 1) AS PotentialContactMatchCount,
		(SELECT COUNT(*) FROM Core.MergedContactsMapping WHERE IsActive = 1) AS MergedContactCount
		 
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END