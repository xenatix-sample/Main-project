-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_ValidateDataContactpayor_staging_ErrorDetails]
-- Author:		Sumana Sangapu
-- Date:		05/19/2016
--
-- Purpose:		Validate lookup data in the staging files used for Data Conversion
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/19/2016	Sumana Sangapu		Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Synch].[usp_ValidateDataContactpayor_staging_ErrorDetails]
AS 
BEGIN

		/******************************************** [Synch].[Contactpayor_staging] ***********************************/

		--PayorID
		INSERT INTO [Synch].[Contactpayor_staging_ErrorDetails]
		SELECT *,'PayorID' FROM [Synch].[contactpayor_staging] cc
		WHERE cc.PayorID  NOT IN ( SELECT [PayorName] FROM [Reference].[Payor]  ) 
		AND cc.PayorID <> ''

		--PayorPlanID
		INSERT INTO [Synch].[Contactpayor_staging_ErrorDetails]
		SELECT *,'PayorPlanID' FROM [Synch].[contactpayor_staging] cc
		WHERE cc.PayorPlanID  NOT IN ( SELECT PlanName FROM Reference.PayorPlan  ) 
		AND cc.PayorPlanID  <> ''

		--PolicyHolderSuffixID  
		INSERT INTO [Synch].[Contactpayor_staging_ErrorDetails] 
		SELECT *,'PolicyHolderSuffixID' FROM  [Synch].[Contactpayor_staging]  c
		WHERE c.PolicyHolderSuffixID  NOT IN ( SELECT Suffix FROM  Reference.Suffix  ) 
		AND c.PolicyHolderSuffixID  <> ''



END
 