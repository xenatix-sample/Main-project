
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_GetCategoryDetails
-- Author:		Suresh Pandey
-- Date:		08/03/2015
--
-- Purpose:		Gets the list of  Category lookup Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/03/2015	Suresh Pandey		TFS#  - Initial creation.
-- 08/03/2015   Sumana Sangapu	1016 - Changed schema from dbo to Registration/Core/Reference
-- 08/30/2015   Suresh Pandey	change CategoryType to CategoryTypeID in select 
-- 08/03/2015   Sumana Sangapu	1016 -	Changed schema from dbo to Registration/Core/Reference
-- 08/27/2015	Sumana Sangapu	1515	Refactor Finance Screen
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetCategoryDetails]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		CategoryID, Category , CategoryTypeID
		FROM		[Reference].[Category] 
		WHERE		IsActive = 1
		ORDER BY	Category  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END