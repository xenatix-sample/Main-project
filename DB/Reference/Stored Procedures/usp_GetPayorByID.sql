-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Reference.[usp_GetPayorByID]
-- Author:		Atul Chauhan
-- Date:		12/07/2016
--
-- Purpose:		Get Payor Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/07/2016	Atul Chauhan  		Initial creation.
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Reference].[usp_GetPayorByID]
@PayorID INT,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	


SELECT  PayorID,
		PayorCode,
		PayorName,
		PayorTypeID,
		EffectiveDate,
		ExpirationDate,
		IsActive,
		ModifiedBy,
		ModifiedOn
  FROM [Reference].[Payor]
  WHERE [IsActive] = 1
		AND PayorID =@PayorID

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
