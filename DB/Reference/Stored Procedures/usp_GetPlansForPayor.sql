-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetPlansForPayor]
-- Author:		Arun Choudhary
-- Date:		09/21/2015
--
-- Purpose:		Get plans for payor.
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------

-- 09/20/2015	Arun Choudhary		- Initial creation.
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Reference].[usp_GetPlansForPayor]
	@PayorID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
		SELECT	
			 PayorPlan.PayorPlanID
			,ISNULL(PayorPlan.PlanID, '') AS PlanID
			,ISNULL(PayorPlan.PlanName, '') AS PlanName
		FROM 
			 [Reference].[PayorPlan]	AS PayorPlan 
		WHERE
			PayorPlan.[PayorID] = @PayorID
	END TRY
	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
