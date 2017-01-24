-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_ValidateDataContactRace_staging_ErrorDetails]
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

CREATE PROCEDURE [Synch].[usp_ValidateDataContactRace_staging_ErrorDetails]
AS 
BEGIN

		/******************************************** [Synch].[ContactRace_staging]  ***********************************/

		--[RaceID]  
		INSERT INTO  [Synch].[ContactRace_Staging_ErrorDetails]
		SELECT *,'RaceID' FROM [Synch].[ContactRace_Staging] c
		WHERE c.RaceID  NOT IN ( SELECT  Race  FROM  Reference.Race  ) 
		AND c.RaceID  <> ''



END
 