-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetAllergyTypes]
-- Author:		Justin Spalti
-- Date:		11/16/2015
--
-- Purpose:		Get list of allergy types
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/16/2015	Justin Spalti	TFS# 2892 - Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Clinical.[usp_GetAllergyTypes]

@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY

		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully'

		SELECT [AllergyTypeID],
			   [AllergyType],
			   [IsActive],
			   [ModifiedBy],
			   [ModifiedOn]
		FROM Clinical.[AllergyType]

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END