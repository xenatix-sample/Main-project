


-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_ValidateDataContactDischargeNote_staging_ErrorDetails]
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

CREATE PROCEDURE [Synch].[usp_ValidateDataContactDischargeNote_staging_ErrorDetails]
AS 
BEGIN

		/******************************************** [Synch].[ContactDischargeNote_Staging] ***********************************/
		-- [DischargeReason] 
		INSERT INTO  [Synch].[ContactDischargeNote_Staging_ErrorDetails]
		SELECT *,'DischargeReason' FROM [Synch].[ContactDischargeNote_Staging] c
		WHERE c.[DischargeReasonID]  NOT IN ( SELECT [DischargeReason] FROM  Reference.[DischargeReason]  ) 
		AND c.[DischargeReasonID]  <> ''

		-- [SignatureStatusID]
		INSERT INTO  [Synch].[ContactDischargeNote_Staging_ErrorDetails]
		SELECT *,'SignatureStatusID' FROM [Synch].[ContactDischargeNote_Staging] c
		WHERE c.[SignatureStatusID]  NOT IN ( SELECT [SignatureStatus] FROM  Reference.[SignatureStatus]  ) 
		AND c.[SignatureStatusiD]  <> ''
END