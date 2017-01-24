-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetReportGroupDetails]
-- Author:		Scott Martin
-- Date:		05/03/2016
--
-- Purpose:		Gets the list of Relationship groups
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/03/2016	Scott Martin	 Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetReportGroupDetails]
	@ReportGroupID INT = NULL,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	SELECT
		ReportGroupID,
		ReportGroup
	FROM
		Reference.ReportGroup
	WHERE
		IsActive = 1
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END