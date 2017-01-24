-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_GetCategoryTypeDetails
-- Author:		Sumana Sangapu
-- Date:		08/27/2015
--
-- Purpose:		Gets the list of  Category Type lookup Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/27/2015	Sumana Sangapu	1515	Refactor Finance Screen
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetCategoryTypeDetails]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		CategoryTypeID, CategoryType 
		FROM		[Reference].[CategoryType] 
		WHERE		IsActive = 1
		ORDER BY	CategoryType  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END


