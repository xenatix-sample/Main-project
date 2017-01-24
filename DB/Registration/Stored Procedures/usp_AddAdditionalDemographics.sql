-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddAdditionalDemographics]
-- Author:		Rajiv Ranjan
-- Date:		07/23/2015
--
-- Purpose:		Add additional demography details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Rajiv Ranjan		Initial creation.
-- 7/31/2015    John Crossen    Change schema from dbo to Registration
-- 08/03/2015   Sumana Sangapu	1016 - Changed proc name schema qualifier from dbo to Registration/Core/Reference
-- 08/05/2015   Rajiv Ranjan	 - Removed MRN parameter
-- 08/24/2015   Rajiv Ranjan	 - Added SchoolDistrictID parameter
-- 08/27/2015   Rajiv Ranjan	 - Added FullCodeDNR, SmokingStatusID, OtherRace, LookingForWork into parameters
-- 08/27/2015   Rajiv Ranjan	 - Added Otherethnicity
-- 10/12/2015   Vipul Singhal	 - Added Interpreter 
-- 10/12/2015   Vipul Singhal	 - Added Interpreter 
-- 12/30/2015   Gaurav Gupta	 - Added OtherLegalStatus,OtherPreferredLanguage,OtherSecondaryLanguage,OtherCitizenship ,OtherEducation,OtherLivingArrangement,OtherVeteranStatus,OtherEmploymentStatus,OtherReligion 
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn field, added CreatedBy and CreatedOn to Insert
-- 03/08/2016	Scott Martin	Removed FullCodeDNR and replaced with AdvancedDirective, added AdvancedDirectiveTypeID
-- 03/16/2016    Arun Choudhary  Added SchoolAttended, SchoolBeginDate, SchoolEndDate
-- 03/18/2016  Lokesh Singhal   Removed @RaceID
-- 05/13/2016	Arun Choudhary	Added PlaceOfEmployment, EmploymentBeginDate, EmploymentEndDate fields
-- 11/29/2016	Rajiv Ranjan	Implemented a check to prevent duplication of Additional demographics for same record
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddAdditionalDemographics]
	@ContactID BIGINT,	
	@AdvancedDirective BIT,
	@AdvancedDirectiveTypeID INT,
	@SmokingStatusID INT,
	@OtherRace NVARCHAR(250),
	@OtherEthnicity NVARCHAR(250),
	@LookingForWork BIT,
	@SchoolDistrictID INT,
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
	@OtherPreferredLanguage  NVARCHAR(250),
	@OtherSecondaryLanguage  NVARCHAR(250),
	@OtherCitizenship  NVARCHAR(250),
	@OtherEducation  NVARCHAR(250),
	@OtherLivingArrangement  NVARCHAR(250),
	@OtherVeteranStatus  NVARCHAR(250),
	@OtherEmploymentStatus NVARCHAR(250),
	@OtherReligion  NVARCHAR(250),
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

	DECLARE @ID BIGINT = (SELECT TOP 1 AdditionalDemographicID FROM Registration.AdditionalDemographics WHERE ContactID=@ContactID AND IsActive=1)
	
	IF(@ID IS NOT NULL)
	BEGIN
		RETURN
	END

	INSERT INTO Registration.AdditionalDemographics
	(
		ContactID,
		AdvancedDirective,
		AdvancedDirectiveTypeID,
		SmokingStatusID,
		OtherRace,
		OtherEthnicity,
		LookingForWork,
		SchoolDistrictID,
		EthnicityID,
		PrimaryLanguageID,
		SecondaryLanguageID,
		LegalStatusID,
		LivingArrangementID,
		CitizenshipID,
		MaritalStatusID, 
		EmploymentStatusID, 
		PlaceOfEmployment,
		EmploymentBeginDate,
		EmploymentEndDate,
		ReligionID, 
		VeteranID,
		EducationID, 
		SchoolAttended,
		SchoolBeginDate,
		SchoolEndDate,
		LivingWill, 
		PowerOfAttorney, 
		InterpreterRequired,
		OtherLegalStatus,
		OtherPreferredLanguage,
		OtherSecondaryLanguage,
		OtherCitizenship ,
		OtherEducation,
		OtherLivingArrangement,
		OtherVeteranStatus,
		OtherEmploymentStatus,
		OtherReligion,
		IsActive,
		ModifiedBy, 
		ModifiedOn,
		CreatedBy,
		CreatedOn)
	VALUES
	(
		@ContactID,
		@AdvancedDirective,
		@AdvancedDirectiveTypeID,
		@SmokingStatusID,
		@OtherRace,
		@OtherEthnicity,
		@LookingForWork,
		@SchoolDistrictID,
		@EthnicityID,
		@PrimaryLanguageID,
		@SecondaryLanguageID,
		@LegalStatusID,
		@LivingArrangementID, 
		@CitizenshipID, 
		@MaritalStatusID, 
		@EmploymentStatusID, 
		@PlaceOfEmployment,
		@EmploymentBeginDate,
		@EmploymentEndDate,
		@ReligionID, 
		@VeteranID,
		@EducationID, 
		@SchoolAttended,
		@SchoolBeginDate,
		@SchoolEndDate,
		@LivingWill, 
		@PowerOfAttorney, 
		@InterpreterRequired,
		@OtherLegalStatus,
		@OtherPreferredLanguage,
		@OtherSecondaryLanguage,
		@OtherCitizenship ,
		@OtherEducation,
		@OtherLivingArrangement,
		@OtherVeteranStatus,
		@OtherEmploymentStatus,
		@OtherReligion,
		1, 
		@ModifiedBy, 
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);
				
	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'AdditionalDemographics', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'AdditionalDemographics', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
