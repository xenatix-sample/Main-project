-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Reference].[usp_GetPlanAddresses]
-- Author:		Atul Chauhan
-- Date:		12/13/2016
--
-- Purpose:		Get list of Plan Addresses
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/13/2016	Atul Chauhan -        Initial creation

-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetPlanAddresses]
@PayorPlanID INT,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

			SELECT  PA.PayorAddressID, 
					PA.ContactID, 
					PA.ElectronicPayorID,
					A.AddressID,
					A.Line1,
					A.Line2,
					A.City,
					A.StateProvince,
					A.Zip
			FROM     Registration.PayorAddress PA
			INNER JOIN core.addresses A ON A.AddressID=PA.AddressID
			WHERE PA.PayorPlanID=@PayorPlanID

  	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END