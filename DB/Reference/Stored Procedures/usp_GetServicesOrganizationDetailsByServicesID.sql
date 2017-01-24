-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetServicesOrganizationDetailsByServicesID]
-- Author:		Kyle Campbell
-- Date:		01/04/2017
--
-- Purpose:		Gets the list of Organization Details assigned to a Service
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/04/2017	Kyle Campbell	TFS #14007	Initial Creation
-- 01/10/2017	Atul Chauhan	DetailID would be fetched as DetailID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetServicesOrganizationDetailsByServicesID]
	@ServicesID BIGINT,
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
			OD.Name As Name,
			SOD.EffectiveDate,
			SOD.ExpirationDate
		FROM
			Reference.ServicesOrganizationDetails SOD
			INNER JOIN Reference.Services S
				ON SOD.ServicesID = S.ServicesID
			INNER JOIN Core.OrganizationDetails OD 
				ON SOD.DetailID = OD.DetailID
		WHERE
			SOD.ServicesID = @ServicesID
		ORDER BY
			SOD.ExpirationDate,
			OD.Name
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END