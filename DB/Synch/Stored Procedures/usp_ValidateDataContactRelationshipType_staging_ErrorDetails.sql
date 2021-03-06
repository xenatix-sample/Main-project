
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_ValidateDataContactRelationshipType_staging_ErrorDetails]
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

CREATE PROCEDURE [Synch].[usp_ValidateDataContactRelationshipType_staging_ErrorDetails]
AS 
BEGIN

		/******************************************** [Synch].[ContactDischargeNote_Staging] ***********************************/
		-- RelationshipTypeID 
		INSERT INTO  [Synch].[ContactRelationshipType_Staging_ErrorDetails]
		SELECT *,'RelationshipTypeID ' FROM [Synch].[ContactRelationshipType_Staging] c
		WHERE c.RelationshipTypeID  NOT IN ( SELECT RelationshipType FROM  Reference.RelationshipType  ) 
		AND c.RelationshipTypeID  <> ''

END