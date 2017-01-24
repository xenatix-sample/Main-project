-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetDocumentType]
-- Author:		Karl Jablonski
-- Date:		03/15/2016
--
-- Purpose:		Gets the list of document types 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/15/2016	Karl Jablonski	-   Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetDocumentType]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		[DocumentTypeID], [DocumentType]
		FROM		[Reference].[DocumentType] 
		WHERE		IsActive = 1
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END