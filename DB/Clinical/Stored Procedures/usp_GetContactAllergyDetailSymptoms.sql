-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetContactAllergyDetailSymptoms]
-- Author:		Justin Spalti
-- Date:		11/20/2015
--
-- Purpose:		Get all of the symptoms set for a bundle's detail records
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- -- 11/20/2015   Justin Spalti - Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Clinical.[usp_GetContactAllergyDetailSymptoms]
@ContactAllergyID BIGINT,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully'

		SELECT   cad.ContactAllergyDetailID
				,cads.AllergySymptomID
				,cads.[IsActive]
				,cads.[ModifiedBy]
				,cads.[ModifiedOn]
		FROM [Clinical].[ContactAllergyDetailSymptoms] cads
		JOIN [Clinical].[ContactAllergyDetail] cad
			ON cad.ContactAllergyDetailID = cads.ContactAllergyDetailID
		WHERE cad.ContactAllergyID = @ContactAllergyID 
			AND cads.IsActive=1  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END