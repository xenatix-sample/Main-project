

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetPayorPlanDetailsByPayorID]
-- Author:		Sumana Sangapu
-- Date:		12/13/2016
--
-- Purpose:		Gets the list of PayorPlan details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/13/2016	Sumana Sangapu	Initial Creation
-- 12/16/2016	Atul Chauhan	Will fetch CreatedBy as well
-- 12/27/2016	Sumana Sangapu	Sort expired Payor Plan records 
-- 1/2/2017     Atul Chauhan	Sort expired Payor Plan records
-- 1/18/2017    Atul Chauhan	Will fetch CreatedOn as well
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetPayorPlanDetailsByPayorID]
	@PayorID	INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
			SELECT [PayorPlanID]
				  ,[PayorID]
				  ,[PlanName]
				  ,[PlanID]
				  ,[EffectiveDate]
				  ,[ExpirationDate]
				  ,CASE WHEN ExpirationDate <= GETDATE() THEN 1 ELSE 0 END AS IsExpired
				  ,[IsActive]
				  ,[CreatedBy]
				  ,[ModifiedBy]
				  ,[ModifiedOn]
				  ,[CreatedOn]
			  FROM  [Reference].[PayorPlan]
			  WHERE PayorID = @PayorID 
			  AND	[IsActive] = 1
			  ORDER BY IsExpired ASC 
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


