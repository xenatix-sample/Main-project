
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetAddressesForPayor]
-- Author:		Arun Choudhary
-- Date:		09/21/2015
--
-- Purpose:		Get Payor Address
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------

-- 09/21/2015	Arun Choudhary		- Initial creation.
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Reference].[usp_GetAddressesForPayor]
	@PayorPlanID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT

AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
		SELECT TOP 1
			 CoreAddress.AddressID 
			,CoreAddress.Line1 
			,CoreAddress.Line2 
			,CoreAddress.City
			,CoreAddress.County
			,CoreAddress.StateProvince
			,CoreAddress.Zip
		FROM 
			[Registration].[PayorAddress]	AS PayorAddress	
			INNER JOIN [Core].[Addresses]	AS CoreAddress	ON PayorAddress.AddressID = CoreAddress.AddressID
		WHERE
			PayorAddress.[PayorPlanID] = @PayorPlanID
		ORDER BY CoreAddress.AddressID DESC

	END TRY
	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END

