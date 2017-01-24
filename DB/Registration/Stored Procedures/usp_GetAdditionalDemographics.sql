-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetAdditionalDemographics]
-- Author:		Rajiv Ranjan
-- Date:		07/23/2015
--
-- Purpose:		Gets the additional demography details of contact
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Rajiv Ranjan		Initial creation.
-- 7/31/2015    John Crossen    Change schema from dbo to Registration
-- 08/03/2015   Sumana Sangapu	1016 - Changed proc name schema qualifier from dbo to Registration/Core/Reference
-- 08/05/2015   Rajiv Ranjan	- Added join with contact table for Name & MRN fields.
-- 08/25/2015   Rajiv Ranjan	- Added age & schoolDistrictID
-- 08/27/2015   Rajiv Ranjan	 - Added FullCodeDNR, SmokingStatusID, OtherRace, LookingForWork into reset set
-- 08/28/2015   Rajiv Ranjan	 - Added OtherEthnicity
-- 09/03/2015   Rajiv Ranjan	 - Used calculate age function
-- 09/11/2015   Rajiv Ranjan	 - Removed calculate age function, as age is calculating in UI for offline capability
-- 10/12/2015   Vipul Singhal	 - Added Interpreter 
-- 12/30/2015   Gaurav Gupta	 - Added OtherLegalStatus,OtherPreferredLanguage,OtherSecondaryLanguage,OtherCitizenship ,OtherEducation,OtherLivingArrangement,OtherVeteranStatus,OtherEmploymentStatus,OtherReligion 
-- 03/08/2016	Scott Martin	Removed FullCodeDNR and replaced with AdvancedDirective, added AdvancedDirectiveTypeID
-- 03/11/2016    Arun Choudhary  Updated LivingWill and PowerOfAttorney to store null if null is passed.
-- 03/16/2016    Arun Choudhary  Added SchoolAttended, SchoolBeginDate, SchoolEndDate
-- 03/18/2016  Lokesh Singhal   Removed @RaceID
-- 05/13/2016	Arun Choudhary	Added PlaceOfEmployment, EmploymentBeginDate, EmploymentEndDate fields
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetAdditionalDemographics]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	SELECT 
		ISNULL(AD.AdditionalDemographicID, 0) AS AdditionalDemographicID,
		C.ContactID,
		c.DOB,
		C.MRN,
		AD.AdvancedDirective,
		AD.AdvancedDirectiveTypeID,
		AD.SmokingStatusID,
		AD.OtherRace,
		AD.OtherEthnicity,
		AD.LookingForWork,
		AD.SchoolDistrictID,
		AD.EthnicityID,
		AD.PrimaryLanguageID,
		AD.SecondaryLanguageID,
		AD.LegalStatusID,
		AD.LivingArrangementID,
		AD.CitizenshipID,
		AD.MaritalStatusID,
		AD.EmploymentStatusID,
		AD.PlaceOfEmployment,
		AD.EmploymentBeginDate,
		AD.EmploymentEndDate,
		AD.ReligionID,
		AD.VeteranID AS VeteranStatusID,
		AD.EducationID AS EducationStatusID,
		AD.SchoolAttended,
		AD.SchoolBeginDate,
		AD.SchoolEndDate,
		AD.LivingWill,
		AD.PowerOfAttorney,
		CAST(ISNULL(AD.InterpreterRequired, 0) AS BIT) AS InterpreterRequired,
		AD.OtherLegalStatus,
		AD.OtherPreferredLanguage,
		AD.OtherSecondaryLanguage,
		AD.OtherCitizenship ,
		AD.OtherEducation,
		AD.OtherLivingArrangement,
		AD.OtherVeteranStatus,
		AD.OtherEmploymentStatus,
		AD.OtherReligion
	FROM 
		Registration.Contact C
		LEFT JOIN Registration.AdditionalDemographics AD ON C.ContactID = AD.ContactID AND AD.IsActive = 1
	WHERE 
		C.ContactID = @ContactID	
		AND C.IsActive = 1 		
			
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END