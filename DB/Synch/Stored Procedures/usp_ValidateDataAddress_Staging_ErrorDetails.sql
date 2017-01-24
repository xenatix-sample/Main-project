-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_ValidateDataAddress_Staging_ErrorDetails]
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

CREATE PROCEDURE [Synch].[usp_ValidateDataAddress_Staging_ErrorDetails]
AS 
BEGIN

		 /*********************************************** [Synch].[Address_Staging]    ***********************************/

		--MailPermissionID 
		INSERT INTO [Synch].[Address_Staging_ErrorDetails]
		SELECT *,'AddressTypeID' FROM [Synch].[Address_Staging] a
		WHERE a.AddressTypeID NOT IN ( SELECT  AddressType FROM  Reference.AddressType  ) 
		AND a.AddressTypeID <> ''


END
 