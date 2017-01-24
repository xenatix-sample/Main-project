----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetPotentialMergeContactsLastRun]
-- Author:		Scott Martin
-- Date:		01/05/2016
--
-- Purpose:		Gets the list of contacts that have the potential to be merged together
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/05/2016	Scott Martin		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetPotentialMergeContactsLastRun]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	SELECT
		PCM.CreatedBy AS UserID,
		CONCAT(U.FirstName, ' ', U.LastName) AS LastRunBy,
		MAX(PCM.CreatedOn) AS LastRunDate

	FROM
		Core.PotentialContactMatches PCM
		INNER JOIN Core.Users U
			ON PCM.CreatedBy = U.UserID
	GROUP BY
		PCM.CreatedBy,
		CONCAT(U.FirstName, ' ', U.LastName)	
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO