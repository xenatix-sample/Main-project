

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_ValidateDataUserCredential_Staging_ErrorDetails]
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

CREATE PROCEDURE [Synch].[usp_ValidateDataUserCredential_Staging_ErrorDetails]
AS 
BEGIN

			/******************************************** [Synch].[UserCredential_Staging]  ***********************************/
			--[CredentialID]
			INSERT INTO [Synch].[UserCredential_Staging_ErrorDetails] 
			SELECT *,'CredentialID' FROM [Synch].[UserCredential_Staging]   c
			WHERE c.CredentialID  NOT IN ( SELECT  CredentialNAme  FROM  Reference.[Credentials]  ) 
			AND c.CredentialID  <> ''

			--[StateIssuedByID] 
			INSERT INTO [Synch].[UserCredential_Staging_ErrorDetails] 
			SELECT *,'StateIssuedByID' FROM [Synch].[UserCredential_Staging]   c
			WHERE c.StateIssuedByID  NOT IN ( SELECT  StateProvinceCode FROM  Reference.[StateProvince]  ) 
			AND c.StateIssuedByID  <> ''



END
 