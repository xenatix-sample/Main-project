-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_ValidateDataAdditionalDemographics_Staging]
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

CREATE PROCEDURE [Synch].[usp_ValidateDataAdditionalDemographics_Staging_ErrorDetails]
AS 
BEGIN

		/*********************************************** [Synch].[AdditionalDemographics_Staging]   ***********************************/
		-- AdvancedDirectiveTypeID
		INSERT INTO [Synch].[AdditionalDemographics_Staging_ErrorDetails]
		SELECT *,'AdvancedDirectiveTypeID' FROM  [Synch].[AdditionalDemographics_Staging] ad
		WHERE ad.AdvancedDirectiveTypeID NOT IN ( SELECT  AdvancedDirectiveType FROM  Reference.AdvancedDirectiveType  ) 
		AND ad.AdvancedDirectiveTypeID <> '' 

		--SchoolDistrictID
		INSERT INTO [Synch].[AdditionalDemographics_Staging_ErrorDetails]
		SELECT *,'SchoolDistrictID' FROM  [Synch].[AdditionalDemographics_Staging] ad
		WHERE ad.SchoolDistrictID NOT IN ( SELECT  SchoolDistrictName FROM  Reference.SchoolDistrict  ) 
		AND ad.SchoolDistrictID <> '' 

		--SmokingStatusID
		INSERT INTO [Synch].[AdditionalDemographics_Staging_ErrorDetails]
		SELECT *,'SmokingStatusID' FROM  [Synch].[AdditionalDemographics_Staging] ad
		WHERE ad.SmokingStatusID NOT IN ( SELECT  SmokingStatus FROM  Reference.SmokingStatus  ) 
		AND ad.SmokingStatusID <> ''

		--EthnicityID
		INSERT INTO [Synch].[AdditionalDemographics_Staging_ErrorDetails]
		SELECT *,'EthnicityID' FROM  [Synch].[AdditionalDemographics_Staging] ad
		WHERE ad.EthnicityID NOT IN ( SELECT  Ethnicity FROM  Reference.Ethnicity  ) 
		AND ad.EthnicityID <> ''

		--PrimaryLanguageID
		INSERT INTO [Synch].[AdditionalDemographics_Staging_ErrorDetails]
		SELECT *,'PrimaryLanguageID' FROM  [Synch].[AdditionalDemographics_Staging] ad
		WHERE ad.PrimaryLanguageID NOT IN ( SELECT  LanguageName FROM  Reference.Languages  ) 
		AND ad.PrimaryLanguageID <> ''

		--SecondaryLanguageID
		INSERT INTO [Synch].[AdditionalDemographics_Staging_ErrorDetails]
		SELECT *,'SecondaryLanguageID' FROM  [Synch].[AdditionalDemographics_Staging] ad
		WHERE ad.SecondaryLanguageID NOT IN ( SELECT  LanguageName FROM  Reference.Languages  ) 
		AND ad.SecondaryLanguageID <> ''

		--LegalStatusID
		INSERT INTO [Synch].[AdditionalDemographics_Staging_ErrorDetails]
		SELECT *,'LegalStatusID' FROM  [Synch].[AdditionalDemographics_Staging] ad
		WHERE ad.LegalStatusID NOT IN ( SELECT  LegalStatus FROM  Reference.LegalStatus  ) 
		AND ad.LegalStatusID <> ''

		--LivingArrangementID
		INSERT INTO [Synch].[AdditionalDemographics_Staging_ErrorDetails]
		SELECT *,'LivingArrangementID' FROM  [Synch].[AdditionalDemographics_Staging] ad
		WHERE ad.LivingArrangementID NOT IN ( SELECT  LivingArrangement FROM  Reference.LivingArrangement  ) 
		AND ad.LivingArrangementID <> ''

		--CitizenshipID
		INSERT INTO [Synch].[AdditionalDemographics_Staging_ErrorDetails]
		SELECT *,'CitizenshipID' FROM  [Synch].[AdditionalDemographics_Staging] ad
		WHERE ad.CitizenshipID NOT IN ( SELECT  Citizenship FROM  Reference.Citizenship  ) 
		AND ad.CitizenshipID <> ''

		--MaritalStatusID 
		INSERT INTO [Synch].[AdditionalDemographics_Staging_ErrorDetails]
		SELECT *,'MaritalStatusID' FROM  [Synch].[AdditionalDemographics_Staging] ad
		WHERE ad.MaritalStatusID NOT IN ( SELECT  MaritalStatus FROM  Reference.MaritalStatus  ) 
		AND ad.MaritalStatusID <> ''

		--EmploymentStatusID 
		INSERT INTO [Synch].[AdditionalDemographics_Staging_ErrorDetails]
		SELECT *,'EmployementStatusID' FROM  [Synch].[AdditionalDemographics_Staging] ad
		WHERE ad.EmployementStatusID NOT IN ( SELECT  EmploymentStatus FROM  Reference.EmploymentStatus  ) 
		AND ad.EmployementStatusID <> ''

		--ReligionID 
		INSERT INTO [Synch].[AdditionalDemographics_Staging_ErrorDetails]
		SELECT *,'ReligionID' FROM  [Synch].[AdditionalDemographics_Staging] ad
		WHERE ad.ReligionID NOT IN ( SELECT  Religion FROM  Reference.Religion  ) 
		AND ad.ReligionID <> ''

		--VeteranID 
		INSERT INTO [Synch].[AdditionalDemographics_Staging_ErrorDetails]
		SELECT *,'VeteranID' FROM  [Synch].[AdditionalDemographics_Staging] ad
		WHERE ad.VeteranID NOT IN ( SELECT  VeteranStatus FROM  Reference.VeteranStatus  ) 
		AND ad.VeteranID <> ''

		--EducationID 
		INSERT INTO [Synch].[AdditionalDemographics_Staging_ErrorDetails]
		SELECT *,'EducationID' FROM  [Synch].[AdditionalDemographics_Staging] ad
		WHERE ad.EducationID NOT IN ( SELECT  EducationStatus FROM  Reference.EducationStatus  ) 
		AND ad.EducationID <> ''

END