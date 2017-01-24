-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Reference].[usp_GetConfirmationStatement]
-- Author:		Scott Martin
-- Date:		04/09/2016
--
-- Purpose:		Get Confirmation Statements
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/09/2016	Scott Martin	Initial Creation
-- 04/12/2016	Scott Martin	Refactored the code so that all of the appropriate messages are returned for each Org Level and removed DocumentTypeID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetConfirmationStatement]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
	SELECT	@ResultCode = 0,
			@ResultMessage = 'executed successfully';

	;WITH Exceptions (MappingID, ParentID, DetailID)
	AS
	(
		SELECT
			MappingID,
			ParentID,
			DetailID
		FROM
			Core.vw_GetOrganizationStructureDetails OSD
		WHERE
			OSD.DetailID IN (SELECT DISTINCT OrganizationDetailID FROM Reference.ConfirmationStatementException)
		UNION ALL
		SELECT
			OSD.MappingID,
			OSD.ParentID,
			E.DetailID
		FROM
			Core.vw_GetOrganizationStructureDetails OSD
			INNER JOIN Exceptions E
				ON OSD.ParentID = E.MappingID
	)
	SELECT
		CS.ConfirmationStatementID,
		CS.ConfirmationStatement,
		CS.ConfirmationStatementGroupID,
		CS.DocumentTypeID,
		CS.IsSignatureRequired,
		OSD.MappingID AS OrganizationID
	FROM
		Core.vw_GetOrganizationStructureDetails OSD
		LEFT OUTER JOIN Exceptions E
			ON OSD.MappingID = E.MappingID
		CROSS JOIN Reference.ConfirmationStatement CS
	WHERE
		E.MappingID IS NULL
		AND CS.IsActive = 1
	UNION ALL
	SELECT
		CS.ConfirmationStatementID,
		CS.ConfirmationStatement,
		CS.ConfirmationStatementGroupID,
		CS.DocumentTypeID,
		CS.IsSignatureRequired,
		OSD.MappingID AS OrganizationID
	FROM
		Core.vw_GetOrganizationStructureDetails OSD
		INNER JOIN Exceptions E
			ON OSD.MappingID = E.MappingID
		CROSS JOIN Reference.ConfirmationStatement CS
		LEFT OUTER JOIN Reference.ConfirmationStatementException CSE
			ON CS.ConfirmationStatementID = CSE.ConfirmationStatementID
			AND E.DetailID = CSE.OrganizationDetailID
	WHERE
		CSE.ConfirmationStatementExceptionID IS NULL
		AND CS.IsActive = 1;

	END TRY

	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE();
	END CATCH
END
GO

