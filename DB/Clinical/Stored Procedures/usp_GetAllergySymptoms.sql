
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetAllergySymptoms]
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
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Clinical.[usp_GetAllergySymptoms]

@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'


SELECT [AllergySymptomID]
      ,[AllergySymptom]
      ,[IsActive]
      ,[ModifiedBy]
      ,[ModifiedOn]
  FROM Clinical.[AllergySymptom]





 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END