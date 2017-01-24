-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_ValidateDataContact_Phone_Staging]
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

CREATE PROCEDURE [Synch].[usp_ValidateDataContact_Phone_Staging]
AS 
BEGIN


		/***********************************************  [Synch].[Contact_Phone_Staging]   ***********************************/

		--PhonePermissionID 
		INSERT INTO [Synch].[Contact_Phone_Staging_ErrorDetails]  
		SELECT *,'PhonePermissionID' FROM  [Synch].[Contact_Phone_Staging]   cp
		WHERE cp.PhonePermissionID NOT IN ( SELECT  PhonePermission FROM  Reference.PhonePermission  ) 
		AND cp.PhonePermissionID <> ''


END
 