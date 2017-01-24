-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetPayorDetails]
-- Author:		Avikal
-- Date:		08/20/2015
--
-- Purpose:		Gets the list of Payor lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
--				Satish Singh		Added PayorCode
-- 12/13/2016	Sumana Sangapu		Added PayorTypeID and other fields
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetPayorDetails]
	--@PayorID	INT, -- Commented as a temporary fix until UI passes the parameter. Uncomment once done by UI.
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
			  FROM  [Reference].[Payor]
			  WHERE --PayorID = @PayorID AND	
			  [IsActive] = 1

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


