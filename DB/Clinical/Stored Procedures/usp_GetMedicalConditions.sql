-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetMedicalCondition]
-- Author:		Scott Martin
-- Date:		11/20/2015
--
-- Purpose:		Get Medical Condition Data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/20/2015	Scott Martin	Initial creation.
-- 12/2/2015	Scott Martin	Refactored code to accommodate schema change (Table name/columns)
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_GetMedicalConditions]
	@IsSystem BIT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		[MedicalConditionID],
		[MedicalCondition],
		[IsSystem],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn]
	FROM
		[Clinical].[MedicalCondition]
	WHERE
		IsSystem = @IsSystem
		AND IsActive = 1
  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


