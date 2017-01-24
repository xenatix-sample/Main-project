-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_ValidateDataPhone_staging_ErrorDetails]
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

CREATE PROCEDURE [Synch].[usp_ValidateDataPhone_staging_ErrorDetails]
AS 
BEGIN
			/******************************************** [Synch].[Phone_Staging] ***********************************/
			--[PhoneTypeID]
			INSERT INTO  [Synch].[Phone_Staging_ErrorDetails]
			SELECT *,'PhoneTypeID' FROM [Synch].[Phone_Staging] c
			WHERE c.PhoneTypeID  NOT IN ( SELECT  PhoneType  FROM  Reference.PhoneType  ) 
			AND c.PhoneTypeID  <> ''


END
 