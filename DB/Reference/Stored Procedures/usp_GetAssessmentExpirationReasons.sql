-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetAssessmentExpirationReasons]
-- Author:		Scott Martin
-- Date:		04/08/2016
--
-- Purpose:		Gets the list of Assessment Expiration Reasons
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/08/2016	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Reference].[usp_GetAssessmentExpirationReasons]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS

BEGIN
	SELECT	@ResultCode = 0,
			@ResultMessage = 'executed successfully';

	BEGIN TRY
	SELECT
		AssessmentExpirationReasonID,
		AssessmentExpirationReason,
		SortOrder
	FROM
		Reference.AssessmentExpirationReason
	WHERE
		IsActive = 1
		AND IsSystem = 0
	ORDER BY
		SortOrder
	END TRY

	BEGIN CATCH

	END CATCH
END

