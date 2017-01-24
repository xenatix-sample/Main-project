-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetContactAllergy]
-- Author:		John Crossen
-- Date:		11/13/2015
--
-- Purpose:		Get Contact Allergy
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/13/2015	John Crossen	TFS# 3566 - Initial creation.
-- 11/20/2015   Justin Spalti - Reformatted the code
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Clinical.[usp_GetContactAllergy]
@ContactAllergyID BIGINT,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully'

		SELECT   [ContactAllergyID]
				,[EncounterID]
				,[ContactID]
				,[AllergyTypeID]
				,[NoKnownAllergy]
				,[TakenBy]
				,[TakenTime]
				,[IsActive]
				,[ModifiedBy]
				,[ModifiedOn]
		FROM [Clinical].[ContactAllergy]
		WHERE ContactAllergyID = @ContactAllergyID 
			AND IsActive=1  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END