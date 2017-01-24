-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [Reference].[usp_GetPayors]
-- Author:		Atul Chauhan
-- Date:		12/12/2016
--
-- Purpose:		Gets the list of  Payors
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/12/2016	Atul Chauhan		 Initial creation.
-- 12/27/2016	Sumana Sangapu		 Sort expired Payor records 
-----------------------------------------------------------------------------------------------------------------------

create PROCEDURE [Reference].[usp_GetPayors]
@SearchText NVARCHAR(4000)=Null,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
			SELECT 
				PayorID, 
				PayorCode, 
				PayorName, 
				PayorTypeID, 
				EffectiveDate, 
				ExpirationDate,
				CASE WHEN ExpirationDate <= GETDATE() THEN 1 ELSE 0 END AS IsExpired, 
				ModifiedBy, 
				ModifiedOn, 
				CreatedBy, 
				CreatedOn 		  
			FROM     
				Reference.Payor AS P
			WHERE
			P.IsActive=1
			AND (P.PayorName LIKE '%'+@SearchText +'%' OR @SearchText IS NULL)
			ORDER BY IsExpired ASC 
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
