-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetServiceDefinitionByServicesID]
-- Author:		Kyle Campbell
-- Date:		01/04/2017
--
-- Purpose:		Gets the Service record for Service Config Service Definition screen
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/04/2017	Kyle Campbell	TFS #14007	Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetServiceDefinitionByServicesID]
	@ServicesID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT
			ServicesID,
			ServiceCode,
			ServiceName,
			ServiceConfigServiceTypeID,
			EffectiveDate,
			ExpirationDate,
			ExpirationReason,
			EncounterReportable,
			ServiceDefinition,
			Notes,
			IsActive,
			ModifiedBy,
			ModifiedOn
		FROM
			Reference.[Services]
		WHERE
			ServicesID = @ServicesID
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
