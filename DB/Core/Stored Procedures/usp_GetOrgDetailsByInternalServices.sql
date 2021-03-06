-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetOrgDetailsByInternalServices]
-- Author:		Sumana Sangapu
-- Date:		04/01/2016
--
-- Purpose:		Get Program Units for all the Services that are defined internally for programming purposes.
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/01/2016	Sumana Sangapu   - Initial creation.
-- 12/27/2016	Scott Martin	Changed reference to a new lookup table for Service to Organization mapping
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetOrgDetailsByInternalServices]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	DECLARE @AuditPost XML,
		@AuditID BIGINT;

	SELECT @ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	BEGIN TRY

		SELECT
			s.ServicesID, 
			LTRIM(RTRIM(s.ServiceName)) AS ServiceName, 
			v.MappingID as OrganizationID, 
			v.Name
		FROM
			Reference.ServicesOrganizationDetails so 
			INNER JOIN Reference.[Services] s
				ON so.ServicesID = s.ServicesID
			INNER JOIN [Core].[vw_GetOrganizationStructureDetails] v
				ON so.DetailID = v.DetailID
		WHERE
			so.IsActive = 1
			AND	s.IsInternal = 1 -- Return the Services that are defined by Xenatix
		ORDER BY
			ServiceName ASC

	END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END