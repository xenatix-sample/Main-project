-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetRelationshipTypeDetails]
-- Author:		Gurpreet Singh
-- Date:		08/26/2015
--
-- Purpose:		Gets the list of RelationshipType lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/26/2015	Gurpreet Singh	 Initial creation.
-- 12/1/2015	Scott Martin	 Incorporated new table that identifies all pertinent relationships
-- 01/04/2016	Sumana Sangapu	 Added a parameter to get the relevant Relationships for a particular Relationship group	
-- 03/16/2016	Kyle Campbell	 TFS #5809 Removed RelationshipGroupID Parameter
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetRelationshipTypeDetails]
	@RelationshipGroupID INT=NULL,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
		SELECT
			RT.[RelationshipTypeID],
			[RelationshipType],
			RGD.RelationshipGroupID
		FROM
			[Reference].[RelationshipGroupDetails] RGD
			INNER JOIN [Reference].[RelationshipType] RT
				ON RGD.RelationshipTypeID = RT.RelationshipTypeID
		WHERE
			RT.[IsActive] = 1
			AND RGD.IsActive = 1
			AND (ISNULL(@RelationshipGroupID,0) = 0  OR RGD.RelationshipGroupID = @RelationshipGroupID)
		ORDER BY
			[RelationshipType]  ASC
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END