

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetPayorAddressDetailsByPayorPlanID]
-- Author:		Sumana Sangapu
-- Date:		12/13/2016
--
-- Purpose:		Gets the list of PayorAddress details by PayorPlanID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/13/2016	Sumana Sangapu	Initial Creation
-- 12/20/2016	Atul Chauhan	Will fetch all address related fields
-- 12/22/2016	Atul Chauhan	return EffectiveDate,ExpirationDate fields.
-- 12/27/2016	Sumana Sangapu	Sort expired Payor Address records 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetPayorAddressDetailsByPayorPlanID]
	@PayorPlanID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
			SELECT  PA.PayorAddressID, 
					PA.PayorPlanID, 
					PA.AddressID, 
					PA.ContactID, 
					PA.ElectronicPayorID, 
					A.AddressTypeID,
					A.Line1,
					A.Line2,
					A.City,
					A.StateProvince,
					A.County,
					A.Zip,
					PA.ExpirationDate,
					CASE WHEN PA.ExpirationDate <= GETDATE() THEN 1 ELSE 0 END AS IsExpired,
					PA.EffectiveDate,
					PA.IsActive, 
					PA.ModifiedBy, 
					PA.ModifiedOn
			  FROM  [Registration].[PayorAddress] PA
			  LEFT JOIN [Core].Addresses A ON A.AddressID=PA.AddressID
			  WHERE PayorPlanID = @PayorPlanID 
			  AND	PA.[IsActive] = 1
			  ORDER BY IsExpired ASC 
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


