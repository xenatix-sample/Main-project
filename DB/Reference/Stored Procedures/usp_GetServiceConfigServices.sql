-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetServiceConfigServices]
-- Author:		Kyle Campbell
-- Date:		01/04/2017
--
-- Purpose:		Gets the list of Services for Services grid in Service Configuration
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/04/2017	Kyle Campbell	TFS #14007	Initial Creation
-- 01/10/2017	Atul Chauhan	Will fetch EncounterReportable as well
-- 01/12/2017	Kyle Campbell	TFS #14007 Sorted expired records at bottom of result set
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetServiceConfigServices]
	@ServiceName NVARCHAR(255) NULL,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT
			S.ServicesID,
			S.ServiceName,
			S.ServiceCode,
			S.ServiceConfigServiceTypeID,
			SCST.ServiceConfigServiceType,
			S.EncounterReportable,
			S.EffectiveDate,
			S.ExpirationDate,
			CASE WHEN IsNull(ExpirationDate, '12-31-2999') <= GETDATE() THEN 1 ELSE 0 END AS IsExpired,
			S.CreatedBy,
			S.IsActive,
			S.ModifiedBy,
			S.ModifiedOn
		FROM
			Reference.[Services] S 
			LEFT JOIN Reference.ServiceConfigServiceType SCST ON S.ServiceConfigServiceTypeID = SCST.ServiceConfigServiceTypeID
		WHERE IsNull(@ServiceName, '') = '' OR S.ServiceName LIKE ('%' + @ServiceName + '%')
		ORDER BY IsExpired, ServiceName

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END