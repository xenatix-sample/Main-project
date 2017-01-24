-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_ValidateDataContactClientIdentifier_Staging_ErrorDetails]
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

CREATE PROCEDURE [Synch].[usp_ValidateDataContactClientIdentifier_Staging_ErrorDetails]
AS 
BEGIN

		 /*********************************************** [Synch].[Address_Staging]    ***********************************/

		--ClientIdentiferTypeID 
		INSERT INTO  Synch.ContactClientIdentifier_Staging_ErrorDetails
		SELECT *,'ClientIdentiferTypeID' FROM [Synch].[ContactClientIdentifier_Staging] a
		WHERE a.ClientIdentiferTypeID NOT IN ( SELECT  ClientIdentifierType FROM  Reference.ClientIdentifierType  ) 
		AND a.ClientIdentiferTypeID <> ''


END
 