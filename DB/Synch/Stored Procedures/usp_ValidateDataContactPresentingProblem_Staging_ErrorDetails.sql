-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_ValidateDataContactPresentingProblem_Staging_ErrorDetails]
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

CREATE PROCEDURE [Synch].[usp_ValidateDataContactPresentingProblem_staging_ErrorDetails]
AS 
BEGIN

		/******************************************** [Synch].[ContactPresentingProblem_staging]  ***********************************/

		--PresentingProblemTypeID
		INSERT INTO  [Synch].[ContactPresentingProblem_Staging_ErrorDetails]  
		SELECT *,'PresentingProblemTypeID' FROM [Synch].[ContactPresentingProblem_staging] c
		WHERE c.PresentingProblemTypeID  NOT IN ( SELECT PresentingProblemType   FROM  Reference.PresentingProblemType  ) 
		AND c.PresentingProblemTypeID  <> ''

END
 