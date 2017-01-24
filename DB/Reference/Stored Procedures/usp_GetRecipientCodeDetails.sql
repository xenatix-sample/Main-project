
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetRecipientCodeDetails]
-- Author:		Sumana Sangapu
-- Date:		01/18/2016
--
-- Purpose:		Gets the list of RecipientCode lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/18/2016	Sumana Sangapu	- Initial creation.
-- 01/24/2017	Kyle Campbell	TFS #22100	Changed to sort alphabetically
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetRecipientCodeDetails]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		CodeID, [Code]  , CodeDescription
		FROM		[Reference].[RecipientCode] 
		WHERE		IsActive = 1
		ORDER BY	[CodeDescription] ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END