

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_ValidateDataUserPhone_Staging_ErrorDetails]
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
-- 08/29/2016	Rahul Vats			This stored proc also needs to be updated. 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Synch].[usp_ValidateDataUserPhone_Staging_ErrorDetails]
AS 
BEGIN

			/******************************************** [Synch].[UserPhone_Staging]    ***********************************/
			--[PhonePermissionID]
			INSERT INTO [Synch].[UserPhone_Staging_ErrorDetails]  
			SELECT *,'PhonePermissionID' FROM  [Synch].[UserPhone_Staging] c
			WHERE c.PhonePermissionID  NOT IN ( SELECT  PhonePermission  FROM  Reference.PhonePermission ) 
			AND c.PhonePermissionID  <> ''



END
 