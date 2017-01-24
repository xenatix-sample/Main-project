

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetPayorPlanDetailsByPayorPlanID]
-- Author:		Sumana Sangapu
-- Date:		12/13/2016
--
-- Purpose:		Gets the list of PayorPlan details by PayorPlanID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/13/2016	Sumana Sangapu	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetPayorPlanDetailsByPayorPlanID]
	@PayorPlanID	INT,
	@ResultCode		INT OUTPUT,
	@ResultMessage	NVARCHAR(500) OUTPUT
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
				  ,[IsActive]
				  ,[ModifiedBy]
				  ,[ModifiedOn]
			  FROM  [Reference].[PayorPlan]
			  WHERE PayorPlanID = @PayorPlanID 
			  AND	[IsActive] = 1
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


