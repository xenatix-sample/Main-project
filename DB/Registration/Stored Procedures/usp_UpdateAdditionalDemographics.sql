-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateAdditionalDemographics]
-- Author:		Rajiv Ranjan
-- Date:		07/23/2015
--
-- Purpose:		Update additional demography details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Rajiv Ranjan		Initial creation.
-- 7/31/2015    John Crossen    Change schema from dbo to Registration
-- 08/03/2015   Sumana Sangapu	1016 - Changed proc name schema qualifier from dbo to Registration/Core/Reference
-- 08/04/2015 - John Crossen -- Data type mismatches resolved
-- 08/05/2015   Rajiv Ranjan	 - Removed MRN parameter
-- 08/24/2015   Rajiv Ranjan	 - Added SchoolDistrictID parameter
-- 08/27/2015   Rajiv Ranjan	 - Added FullCodeDNR, SmokingStatusID, OtherRace, LookingForWork parameters
-- 08/28/2015   Rajiv Ranjan	 - Added OtherEthnicity
-- 09/03/2015   Rajiv Ranjan	 - Reset other Race, Ethnicity and employment Status fields on update. Also used calculate age function.
-- 09/11/2015   Rajiv Ranjan	 - Removed calculate age function
-- 09/25/2015 - John Crossen    - Refactor Proc to use PK for update
-- 10/12/2015   Vipul Singhal	- Added Interpreter 
-- 12/31/2015   Gaurav Gupta	- Added OtherLegalStatus,OtherPreferredLanguage,OtherSecondaryLanguage,OtherCitizenship,OtherEducation,OtherLivingArrangement,OtherVeteranStatus,OtherEmploymentStatus,OtherReligion,
-- 03/08/2016	Scott Martin	Removed FullCodeDNR and replaced with AdvancedDirective, added AdvancedDirectiveTypeID
--03/16/2016    Arun Choudhary  Added SchoolAttended, SchoolBeginDate, SchoolEndDate
-- 03/18/2016  Lokesh Singhal   Removed @RaceID
-- 05/13/2016	Arun Choudhary	Added PlaceOfEmployment, EmploymentBeginDate, EmploymentEndDate fields
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE  PROCEDURE [Registration].[usp_UpdateAdditionalDemographics]
	@AdditionalDemographicID BIGINT,
	@ContactID BIGINT,
	@AdvancedDirective BIT,
	@AdvancedDirectiveTypeID INT,
	@SmokingStatusID INT = NULL,
	@OtherRace NVARCHAR(250) = NULL,
	@OtherEthnicity NVARCHAR(250) = NULL,
	@LookingForWork BIT = 0,
	@SchoolDistrictID INT = NULL,
	@EthnicityID INT,
	@PrimaryLanguageID INT,
	@SecondaryLanguageID INT,
	@LegalStatusID INT,
	@LivingArrangementID INT,
	@CitizenshipID INT,
	@MaritalStatusID INT,
	@EmploymentStatusID INT,
	@PlaceOfEmployment NVARCHAR(250),
	@EmploymentBeginDate DATE,
	@EmploymentEndDate DATE,
	@ReligionID INT,
	@VeteranID INT,
	@EducationID INT,
	@SchoolAttended NVARCHAR(250),
	@SchoolBeginDate DATE,
	@SchoolEndDate DATE,
	@LivingWill BIT,
	@PowerOfAttorney BIT,
	@InterpreterRequired BIT,
	@OtherLegalStatus NVARCHAR(250),
	@OtherPreferredLanguage NVARCHAR(250),
	@OtherSecondaryLanguage NVARCHAR(250),
	@OtherCitizenship NVARCHAR(250),
	@OtherEducation NVARCHAR(250),
	@OtherLivingArrangement NVARCHAR(250),
	@OtherVeteranStatus NVARCHAR(250),
	@OtherEmploymentStatus NVARCHAR(250),
	@OtherReligion NVARCHAR(250),
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY

	IF EXISTS (SELECT 1 FROM Registration.AdditionalDemographics WHERE AdditionalDemographicID = @AdditionalDemographicID AND IsActive = 1)
		BEGIN
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'AdditionalDemographics', @AdditionalDemographicID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE  
			Registration.AdditionalDemographics 
		SET 
			AdvancedDirective = @AdvancedDirective,
			AdvancedDirectiveTypeID = @AdvancedDirectiveTypeID,
			SmokingStatusID = @SmokingStatusID,
			OtherEthnicity= CASE WHEN @EthnicityID = 6 THEN @OtherEthnicity ELSE NULL END,
			LookingForWork= CASE WHEN @EmploymentStatusID = 5 THEN @LookingForWork ELSE 0 END,
			SchoolDistrictID = @SchoolDistrictID,
			EthnicityID = @EthnicityID,
			PrimaryLanguageID = @PrimaryLanguageID,
			SecondaryLanguageID = @SecondaryLanguageID,
			LegalStatusID = @LegalStatusID,
			LivingArrangementID = @LivingArrangementID, 
			CitizenshipID = @CitizenshipID, 
			MaritalStatusID = @MaritalStatusID, 
			EmploymentStatusID = @EmploymentStatusID, 
			PlaceOfEmployment = @PlaceOfEmployment,
			EmploymentBeginDate = @EmploymentBeginDate,
			EmploymentEndDate = @EmploymentEndDate,
			ReligionID = @ReligionID, 
			VeteranID = @VeteranID,
			EducationID = @EducationID, 
			SchoolAttended = @SchoolAttended,
			SchoolBeginDate = @SchoolBeginDate,
			SchoolEndDate = @SchoolEndDate,
			LivingWill = @LivingWill, 
			PowerOfAttorney = @PowerOfAttorney,
			InterpreterRequired = @InterpreterRequired,
			OtherLegalStatus = CASE WHEN @LegalStatusID = 8 THEN @OtherLegalStatus ELSE NULL END,
			OtherPreferredLanguage = CASE WHEN @PrimaryLanguageID = 12 THEN @OtherPreferredLanguage ELSE NULL END,
			OtherSecondaryLanguage = CASE WHEN @SecondaryLanguageID = 12 THEN @OtherSecondaryLanguage ELSE NULL END,
			OtherCitizenship  = CASE WHEN @CitizenshipID  = 3 THEN @OtherCitizenship  ELSE NULL END,
			OtherEducation = CASE WHEN @EducationID = 9 THEN @OtherEducation ELSE NULL END,
			OtherLivingArrangement = CASE WHEN @LivingArrangementID = 9 THEN @OtherLivingArrangement ELSE NULL END,
			OtherVeteranStatus = CASE WHEN @VeteranID = (SELECT TOP 1 VeteranStatusID FROM Reference.VeteranStatus WHERE VeteranStatus = 'Other') THEN @OtherVeteranStatus ELSE NULL END,
			OtherEmploymentStatus = CASE WHEN @EmploymentStatusID = 6 THEN @OtherEmploymentStatus ELSE NULL END,
			OtherReligion = CASE WHEN @ReligionID = 11 THEN @OtherReligion ELSE NULL END,
			ModifiedBy = @ModifiedBy, 
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		FROM 
			Registration.AdditionalDemographics 
		WHERE 
			AdditionalDemographicID = @AdditionalDemographicID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'AdditionalDemographics', @AuditDetailID, @AdditionalDemographicID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END