
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_ValidateDataUserEmail_Staging_ErrorDetails]
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

CREATE PROCEDURE [Synch].[usp_ValidateDataUserEmail_staging_ErrorDetails]
AS 
BEGIN

		/******************************************** [Synch].[useremail_Staging]   ***********************************/
		--[EmailPermissionID]
		INSERT INTO  [Synch].[Useremail_Staging_Details]
		SELECT *,'EmailPermissionID' FROM [Synch].[useremail_Staging]  c
		WHERE c.EmailPermissionID  NOT IN ( SELECT  EmailPermission  FROM  Reference.EmailPermission ) 
		AND c.EmailPermissionID  <> ''

END
 