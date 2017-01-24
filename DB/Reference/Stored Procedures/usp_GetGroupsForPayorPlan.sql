-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetGroupsForPayorPlan]
-- Author:		Arun Choudhary
-- Date:		09/21/2015
--
-- Purpose:		Get Payor group details for payor.
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------

-- 09/20/2015	Arun Choudhary		- Initial creation.
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Reference].[usp_GetGroupsForPayorPlan]
	@PlanID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
		SELECT	
			 PayorGroup.PayorPlanID
			,PayorGroup.PayorGroupID
			,ISNULL(PayorGroup.GroupID, '') AS GroupID
			,ISNULL(PayorGroup.GroupName, '') AS GroupName
		FROM 
			[Reference].[PayorGroup]	AS PayorGroup
		WHERE
			PayorGroup.PayorPlanID = @PlanID
	END TRY
	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END

