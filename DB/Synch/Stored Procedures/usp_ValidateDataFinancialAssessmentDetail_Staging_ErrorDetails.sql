
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_ValidateDataFinancialAssessmentDetail_Staging_ErrorDetails]
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

CREATE PROCEDURE [Synch].[usp_ValidateDataFinancialAssessmentDetail_staging_ErrorDetails]
AS 
BEGIN

			/******************************************** [Synch].[FinancialAssessment_Staging] ***********************************/
			--CategoryTypeID 
			INSERT INTO  [Synch].[FinancialAssessmentDetail_Staging_ErrorDetails]
			SELECT *,'CategoryTypeID' FROM [Synch].[FinancialAssessmentDetail_Staging] c
			WHERE c.CategoryTypeID  NOT IN ( SELECT CategoryType  FROM  Reference.CategoryType  ) 
			AND c.CategoryTypeID  <> ''

			--CategoryID 
			INSERT INTO  [Synch].[FinancialAssessmentDetail_Staging_ErrorDetails]
			SELECT *,'CategoryID' FROM [Synch].[FinancialAssessmentDetail_Staging] c
			WHERE c.CategoryID  NOT IN ( SELECT Category  FROM  Reference.Category  ) 
			AND c.CategoryID  <> ''



END
 