
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_ValidateDataUsers_staging_ErrorDetails]
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

CREATE PROCEDURE [Synch].[usp_ValidateDataUsers_staging_ErrorDetails]
AS 
BEGIN

		/******************************************** [Synch].[Users_staging]    ***********************************/
		--GenderID
		INSERT INTO  [Synch].[Users_Staging_ErrorDetails]
		SELECT *,'GenderID' FROM [Synch].[Users_Staging] cs
		WHERE cs.GenderID NOT IN ( SELECT  Gender FROM  Reference.Gender  ) 
		AND cs.GenderID <> '' 



END
 