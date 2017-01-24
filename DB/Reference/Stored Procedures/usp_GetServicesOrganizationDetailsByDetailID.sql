-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetServicesOrganizationDetailsByDetailID]
-- Author:		Scott Martin
-- Date:		12/27/2016
--
-- Purpose:		Gets the list of Services for an Organization Detail
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/27/2016	Scott Martin	- Initial creation.
-- 12/30/2016	Scott Martin	Added sorting
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetServicesOrganizationDetailsByDetailID]
	@DetailID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	SELECT
		SOD.ServicesOrganizationDetailsID,
		S.ServicesID AS ServiceID,
		S.ServiceName,
		SOD.DetailID,
		SOD.EffectiveDate,
		SOD.ExpirationDate
	FROM
		Reference.ServicesOrganizationDetails SOD
		INNER JOIN Reference.Services S
			ON SOD.ServicesID = S.ServicesID
	WHERE
		SOD.DetailID = @DetailID
	ORDER BY
		SOD.ExpirationDate,
		S.ServiceName
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END