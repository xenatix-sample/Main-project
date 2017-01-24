-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetMessageTemplate]
-- Author:		Rajiv Ranjan
-- Date:		08/05/2015
--
-- Purpose:		Get Email Template details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/05/2015	Rajiv Ranjan		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetMessageTemplate]
	@TemplateID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT 
			ET.MessageTemplateID,
			ET.TemplateID,
			ET.EmailSubject,
			ET.MessageBody,
			ET.IsHtmlBody
	    FROM 
			Core.MessageTemplate ET
		WHERE
		    ET.TemplateID = @TemplateID
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO


