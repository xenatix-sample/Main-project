-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetMedicalHistoryRelationshipTypes]
-- Author:		Scott Martin
-- Date:		12/2/2015
--
-- Purpose:		Gets the list of RelationshipType lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/8/2015	Scott Martin	 Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetMedicalHistoryRelationshipTypes]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
		SELECT
			RT.[RelationshipTypeID],
			[RelationshipType]
		FROM
			[Reference].[RelationshipGroupDetails] RGD
			INNER JOIN [Reference].[RelationshipType] RT
				ON RGD.RelationshipTypeID = RT.RelationshipTypeID
		WHERE
			RT.[IsActive] = 1
			AND RGD.RelationshipGroupID = 3
		ORDER BY
			[RelationshipType]  ASC
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


