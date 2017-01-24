

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_ValidateDataECIAdditionalDemo_staging_ErrorDetails]
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
-- 06/24/2016	Sumana Sangapu		Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Synch].[usp_ValidateDataECIAdditionalDemo_staging_ErrorDetails]
AS 
BEGIN

		/******************************************** [Synch].[ECIAdditionalDemo_Staging] ***********************************/
		-- [ReferralDispositionStatusID] 
		INSERT INTO  [Synch].[ECIAdditionalDemographics_Staging_Error_Details]
		SELECT *,'ReferralDispositionStatusID' FROM [Synch].[ECIAdditionalDemographics_Staging] c
		WHERE c.[ReferralDispositionStatusID]  NOT IN ( SELECT ReferralDispositionStatus FROM  Reference.ReferralDispositionStatus  ) 
		AND c.[ReferralDispositionStatusID]  <> ''

END