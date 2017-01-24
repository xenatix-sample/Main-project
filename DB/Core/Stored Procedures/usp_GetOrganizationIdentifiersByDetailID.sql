-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetOrganizationIdentifiersByDetailID]
-- Author:		Scott Martin
-- Date:		12/28/2016
--
-- Purpose:		Get Organization Identifier Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/28/2016	Scott Martin	Initial creation
-- 01/03/2017	Scott Martin	Modified the query to pull the default identifier types if no detail exists
-- 01/11/2017	Scott Martin	Added sorting
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Core.usp_GetOrganizationIdentifiersByDetailID
	@DataKey NVARCHAR(50),
	@DetailID BIGINT = 0,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully'
		;WITH ID_CTE (OrganizationIdentifierID, DetailID, OrganizationIdentifierTypeID, OrganizationIdentifier, EffectiveDate, ExpirationDate, IsActive)
		AS
		(
			SELECT
				OI.OrganizationIdentifierID,
				OI.DetailID,
				OI.OrganizationIdentifierTypeID,
				OI.OrganizationIdentifier,
				OI.EffectiveDate,
				OI.ExpirationDate,
				OI.IsActive
			FROM
				Core.OrganizationIdentifiers OI
			WHERE
				OI.DetailID = @DetailID
		)

		SELECT
			OrganizationIdentifierID,
			ID_CTE.DetailID,
			OIT.OrganizationIdentifierTypeID,
			OIT.OrganizationIdentifierType,
			ID_CTE.OrganizationIdentifier,
			ID_CTE.EffectiveDate,
			ID_CTE.ExpirationDate,
			ID_CTE.IsActive
		FROM
			Reference.OrganizationAttributesIdentifierType OAIT
			INNER JOIN Reference.OrganizationIdentifierType OIT
				ON OAIT.OrganizationAttributesIdentifierTypeID = OIT.OrganizationIdentifierTypeID
			INNER JOIN Core.OrganizationAttributes OA
				ON OAIT.AttributeID = OA.AttributeID
			LEFT OUTER JOIN ID_CTE
				ON OAIT.OrganizationAttributesIdentifierTypeID = ID_CTE.OrganizationIdentifierTypeID
		WHERE
			OA.DataKey = @DataKey
  	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO