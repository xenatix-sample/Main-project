-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_ValidateDataFinancialAssessment_Staging_ErrorDetails]
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

CREATE PROCEDURE [Synch].[usp_ValidateDataFinancialAssessment_Staging_ErrorDetails]
AS 
BEGIN

		/******************************************** [Synch].[FinancialAssessment_Staging] ***********************************/
		--ExpirationReasonID  
		INSERT INTO  [Synch].[FinancialAssessment_Staging_ErrorDetails]
		SELECT *,'ExpirationReasonID' FROM [Synch].[FinancialAssessment_Staging] c
		WHERE c.ExpirationReasonID  NOT IN ( SELECT ExpirationReason FROM  Reference.ExpirationReason  ) 
		AND c.ExpirationReasonID  <> ''



END
 