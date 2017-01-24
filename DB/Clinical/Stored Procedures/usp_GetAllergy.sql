
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetAllergy]
-- Author:		John Crossen
-- Date:		10/27/2015
--
-- Purpose:		Get list of Allergies
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/27/2015	John Crossen	TFS# 2892 - Initial creation.
-- 11/30/2015 - Justin Spalti - Added the IsActive flag to the where clause
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Clinical.[usp_GetAllergy]

@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

SELECT [AllergyID]
      ,[AllergyName]
      ,[IsActive]
      ,[ModifiedBy]
      ,[ModifiedOn]
 FROM Clinical.[Allergy]
 WHERE [IsActive] = 1
 ORDER BY [AllergyName]


 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END