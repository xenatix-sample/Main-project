
----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetDocumentTypeGroup]
-- Author:		Sumana Sangapu
-- Date:		04/09/2016
--
-- Purpose:		Gets the lookup values for DocumentTypeGroup
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/09/2016	Sumana Sangapu    Initial creation. 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetDocumentTypeGroup]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		DocumentTypeGroupID, DocumentTypeGroup
		FROM		[Reference].[DocumentTypeGroup] 
		WHERE		IsActive = 1
		ORDER BY	DocumentTypeGroup ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END