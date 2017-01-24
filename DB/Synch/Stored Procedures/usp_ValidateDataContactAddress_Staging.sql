-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_ValidateDataContactAddress_Staging]
-- Author:		Sumana Sangapu
-- Date:		05/29/2016
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

CREATE PROCEDURE [Synch].[usp_ValidateDataContactAddress_Staging]
AS 
BEGIN

	/*********************************************** [Synch].[ContactAddress_Staging]    ***********************************/

	--MailPermissionID 
	INSERT INTO [Synch].[ContactAddress_Staging_ErrorDetails] 
	SELECT *,'MailPermissionID' FROM [Synch].[ContactAddress_Staging] ca
	WHERE ca.MailPermissionID NOT IN ( SELECT  MailPermission FROM  Reference.MailPermission  ) 
	AND ca.MailPermissionID <> ''


END
 