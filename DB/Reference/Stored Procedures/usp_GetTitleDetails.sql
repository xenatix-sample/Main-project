-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[Title]
-- Author:		Suresh Pandey
-- Date:		08/19/2015
--
-- Purpose:		Lookup for Title(Title)
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/19/2015	Sumana Sangapu TFS# 1514 - Initial creation.
 -----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Reference].[usp_GetTitleDetails]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = OBJECT_NAME(@@PROCID) + ' executed successfully'

	BEGIN TRY	
		SELECT		TitleID, Title
		FROM		[Reference].[Title] 
		WHERE		IsActive = 1
		ORDER BY	Title  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END


GO