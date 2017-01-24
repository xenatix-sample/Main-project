-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_ValidateDataContactRelationship_staging_ErrorDetails]
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

CREATE PROCEDURE [Synch].[usp_ValidateDataContactRelationship_staging_ErrorDetails]
AS 
BEGIN

		/******************************************** [Synch].[ContactRelationship_Staging] ***********************************/
		--ContactTypeID
		INSERT INTO [Synch].[ContactRelationship_Staging_ErrorDetails]
		SELECT *,'ContactTypeID' FROM   [Synch].[ContactRelationship_Staging] c
		WHERE c.ContactTypeID NOT IN ( SELECT  ct.contacttype FROM  Reference.ContactType ct ) 
		AND c.ContactTypeID <> '' 

		--PhonePermissionID 
		INSERT INTO [Synch].[ContactRelationship_Staging_ErrorDetails]  
		SELECT *,'PhonePermissionID' FROM  [Synch].[ContactRelationship_Staging] c
		WHERE c.PhonePermissionID NOT IN ( SELECT  PhonePermission FROM  Reference.PhonePermission  ) 
		AND c.PhonePermissionID <> ''

		--ReceiveCorrespondenceID  
		INSERT INTO [Synch].[ContactRelationship_Staging_ErrorDetails] 
		SELECT *,'ReceiveCorrespondenceID' FROM [Synch].[ContactRelationship_Staging] cc
		WHERE cc.ReceiveCorrespondenceID  NOT IN ( SELECT LivingWithClientStatus FROM  [Reference].[LivingWithClientStatus]  ) 
		AND cc.ReceiveCorrespondenceID  <> ''

		--EducationStatusID
		INSERT INTO [Synch].[ContactRelationship_Staging_ErrorDetails] 
		SELECT *,'EducationStatusID' FROM  [Synch].[ContactRelationship_Staging] c
		WHERE c.EducationStatusID NOT IN ( SELECT  EducationStatus FROM  Reference.EducationStatus  ) 
		AND c.EducationStatusID <> ''

		--EmploymentStatusID 
		INSERT INTO [Synch].[ContactRelationship_Staging_ErrorDetails] 
		SELECT *,'EmployementStatusID' FROM  [Synch].[ContactRelationship_Staging] c
		WHERE c.EmploymentStatusID NOT IN ( SELECT  EmploymentStatus FROM  Reference.EmploymentStatus  ) 
		AND c.EmploymentStatusID <> ''

		----VeteranStatusID 
		--INSERT INTO [Synch].[AdditionalDemographics_Staging_ErrorDetails]
		--SELECT *,'VeteranStatusID' FROM  [Synch].[ContactRelationship_Staging] c
		--WHERE c.VeteranStatusID NOT IN ( SELECT  VeteranStatus FROM  Reference.VeteranStatus  ) 
		--AND c.VeteranStatusID <> ''


END
 