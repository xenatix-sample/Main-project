----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetAdditionalDemographics]
-- Author:		Scott Martin
-- Date:		1/5/2015
--
-- Purpose:		Gets ECI additional demographic info
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/5/2015	Scott Martin		Initial creation.
-- 01/18/2016 - Justin Spalti - Added MRN to the result set by joining to the contact table
-- 03/18/2016  Lokesh Singhal   Removed @RaceID
-- 06/02/2016 - Justin Spalti - Updated the query to return non-null values as needed by the data models
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_GetAdditionalDemographics]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
		SELECT
			c.MRN,
			ISNULL(RAD.AdditionalDemographicID, 0) AS AdditionalDemographicID,
			c.ContactID,
			EAD.ReferralDispositionStatusID,
			RAD.SchoolDistrictID,
			RAD.OtherRace,
			RAD.EthnicityID,
			RAD.OtherEthnicity,
			RAD.PrimaryLanguageID As LanguageID,
			RAD.OtherPreferredLanguage,
			CAST(ISNULL(RAD.InterpreterRequired, 0) AS BIT) AS InterpreterRequired,
			IsCPSInvolved,
			IsChildHospitalized,
			ExpectedHospitalDischargeDate,
			BirthWeightLbs,
			BirthWeightOz,
			IsTransfer,
			TransferFrom,
			TransferDate,
			IsOutOfServiceArea,
			ReportingUnitID,
			ServiceCoordinatorID,
			ServiceCoordinatorPhoneID,
			RAD.ModifiedBy,
			RAD.ModifiedOn
		FROM 
			Registration.Contact c
			LEFT OUTER JOIN Registration.AdditionalDemographics RAD
				ON RAD.ContactID = c.ContactID
			LEFT OUTER JOIN ECI.AdditionalDemographics EAD
				ON RAD.AdditionalDemographicID = EAD.RegistrationAdditionalDemographicID
				AND EAD.IsActive = 1
		WHERE 
			c.ContactID = @ContactID
			AND c.IsActive = 1
			
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END