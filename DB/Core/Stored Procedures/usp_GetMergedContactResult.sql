----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetMergedContactResult]
-- Author:		Scott Martin
-- Date:		12/29/2015
--
-- Purpose:		Gets the final results of the client merge
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/16/2016	Scott Martin		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetMergedContactResult]
	@MergedContactsMappingID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	SELECT
		MCR.MergedContactsMappingID,
		MCR.IsSuccessful,
		ISNULL(MCR.ModuleComponentID, 0) AS ModuleComponentID,
		ISNULL(M.Name, 'System') AS ModuleName,
		ISNULL(C.Name, 'System Related') AS ComponentName,
		SUM(MCR.TotalRecords) AS TotalRecords,
		SUM(MCR.TotalRecordsMerged) AS TotalRecordsMerged,
		MCR.ResultMessage
	FROM
		Core.MergedContactResult MCR
		LEFT OUTER JOIN Core.ModuleComponent MC
			ON MCR.ModuleComponentID = MC.ModuleComponentID
		LEFT OUTER JOIN Core.Module M
			ON MC.ModuleID = M.ModuleID
		LEFT OUTER JOIN Core.Component C
			ON MC.ComponentID = C.ComponentID
	WHERE
		MCR.MergedContactsMappingID = @MergedContactsMappingID
		AND MC.ModuleComponentID IS NOT NULL
	GROUP BY
		MCR.MergedContactsMappingID,
		MCR.IsSuccessful,
		ISNULL(M.Name, 'System'),
		ISNULL(MCR.ModuleComponentID, 0),
		ISNULL(C.Name, 'System Related'),
		MCR.ResultMessage;
			
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO