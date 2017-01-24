-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_MergeClientContact
-- Author:		John Crossen
-- Date:		08/03/2016
--
-- Purpose:		Main procedure for Client Contact
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/03/2016 - Initial procedure creation
-- 08/16/2016	Scott Martin	Refactored proc to include auditing and storing results of merging
-- 09/06/2016	Scott Martin	Audit logging was not setting the PK correctly
-- 12/01/2016	Scott Martin	Refactored the proc so that it creates a new contact based on the values from a Parent and Child record and inactivates the Parent/Child
-- 01/19/2017	Scott Martin	Added ECI Additional Demographics
-- 01/23/2017	Scott Martin	Added a condition of Inserting IsCPSInvolved = 0 if all other values are NULL
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_MergeClientContact]
(
	@TransactionLogID BIGINT,
	@ParentID BIGINT,
	@ChildID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
)
AS
BEGIN
	SELECT @ResultCode = 0,
	
	@ResultMessage = 'Data saved successfully'

	BEGIN TRY	

	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
			@ModuleComponentID BIGINT = 23,
			@AdditionalDemographicsID BIGINT,
			@PresentingProblemID BIGINT,
			@ECIAdditionalDemographicID BIGINT;

	--Create a new contact record merging the field values from Parent and Child records
	INSERT INTO Registration.Contact
	(
		MRN,
		MPI,
		ContactTypeID,
		ClientTypeID,
		FirstName,
		Middle,
		LastName,
		SuffixID,
		GenderID,
		PreferredGenderID,
		TitleID,
		SequesteredByID,
		DOB,
		DOBStatusID,
		SSN,
		SSNStatusID,
		DriverLicense,
		DriverLicenseStateID,
		PreferredName,
		IsDeceased,
		DeceasedDate,
		CauseOfDeath,
		PreferredContactMethodID,
		ReferralSourceID,
		IsPregnant,
		GestationalAge,
		IsActive,
		SSNEncrypted,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT 
		C.MRN,
		COALESCE(C.MPI, CC.MPI),
		C.ContactTypeID,
		C.ClientTypeID,
		COALESCE(C.FirstName, CC.FirstName),
		COALESCE(C.Middle, CC.Middle),
		COALESCE(C.LastName, CC.LastName),
		COALESCE(C.SuffixID, CC.SuffixID),
		COALESCE(C.GenderID, CC.GenderID),
		COALESCE(C.PreferredGenderID, CC.PreferredGenderID),
		COALESCE(C.TitleID, CC.TitleID),
		COALESCE(C.SequesteredByID, CC.SequesteredByID),
		COALESCE(C.DOB, CC.DOB),
		COALESCE(C.DOBStatusID, CC.DOBStatusID),
		COALESCE(C.SSN, CC.SSN),
		COALESCE(C.SSNStatusID, CC.SSNStatusID),
		COALESCE(C.DriverLicense, CC.DriverLicense),
		COALESCE(C.DriverLicenseStateID, CC.DriverLicenseStateID),
		COALESCE(C.PreferredName, CC.PreferredName),
		COALESCE(C.IsDeceased, CC.IsDeceased),
		COALESCE(C.DeceasedDate, CC.DeceasedDate),
		COALESCE(C.CauseOfDeath, CC.CauseOfDeath),
		COALESCE(C.PreferredContactMethodID, CC.PreferredContactMethodID),
		COALESCE(C.ReferralSourceID, CC.ReferralSourceID),
		COALESCE(C.IsPregnant, CC.IsPregnant),
		COALESCE(C.GestationalAge, CC.GestationalAge),
		1 AS IsActive,
		COALESCE(C.SSNEncrypted, CC.SSNEncrypted),
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Registration.Contact C
		CROSS JOIN Registration.Contact CC
	WHERE
		C.ContactID = @ParentID
		AND CC.ContactID = @ChildID;

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'Contact', @ID, NULL, @TransactionLogID, @ModuleComponentID, @ID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'Contact', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddContactDemographicChangeLog @TransactionLogID, @ID, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT

	--Create new Additional Details record for new contact merging the field values from Parent and Child records
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
		InterpreterRequired,
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
		OtherLegalStatus,
		OtherPreferredLanguage,
		OtherSecondaryLanguage,
		OtherCitizenship,
		OtherEducation,
		OtherLivingArrangement,
		OtherVeteranStatus,
		OtherEmploymentStatus,
		OtherReligion,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		@ID,
		COALESCE(AD.AdvancedDirective, CAD.AdvancedDirective),
		COALESCE(AD.AdvancedDirectiveTypeID, CAD.AdvancedDirectiveTypeID),
		COALESCE(AD.SmokingStatusID, CAD.SmokingStatusID),
		COALESCE(AD.OtherRace, CAD.OtherRace),
		COALESCE(AD.OtherEthnicity, CAD.OtherEthnicity),
		COALESCE(AD.LookingForWork, CAD.LookingForWork),
		COALESCE(AD.SchoolDistrictID, CAD.SchoolDistrictID),
		COALESCE(AD.EthnicityID, CAD.EthnicityID),
		COALESCE(AD.PrimaryLanguageID, CAD.PrimaryLanguageID),
		COALESCE(AD.SecondaryLanguageID, CAD.SecondaryLanguageID),
		COALESCE(AD.InterpreterRequired, CAD.InterpreterRequired),
		COALESCE(AD.LegalStatusID, CAD.LegalStatusID),
		COALESCE(AD.LivingArrangementID, CAD.LivingArrangementID),
		COALESCE(AD.CitizenshipID, CAD.CitizenshipID),
		COALESCE(AD.MaritalStatusID, CAD.MaritalStatusID),
		COALESCE(AD.EmploymentStatusID, CAD.EmploymentStatusID),
		COALESCE(AD.PlaceOfEmployment, CAD.PlaceOfEmployment),
		COALESCE(AD.EmploymentBeginDate, CAD.EmploymentBeginDate),
		COALESCE(AD.EmploymentEndDate, CAD.EmploymentEndDate),
		COALESCE(AD.ReligionID, CAD.ReligionID),
		COALESCE(AD.VeteranID, CAD.VeteranID),
		COALESCE(AD.EducationID, CAD.EducationID),
		COALESCE(AD.SchoolAttended, CAD.SchoolAttended),
		COALESCE(AD.SchoolBeginDate, CAD.SchoolBeginDate),
		COALESCE(AD.SchoolEndDate, CAD.SchoolEndDate),
		COALESCE(AD.LivingWill, CAD.LivingWill),
		COALESCE(AD.PowerOfAttorney, CAD.PowerOfAttorney),
		COALESCE(AD.OtherLegalStatus, CAD.OtherLegalStatus),
		COALESCE(AD.OtherPreferredLanguage, CAD.OtherPreferredLanguage),
		COALESCE(AD.OtherSecondaryLanguage, CAD.OtherSecondaryLanguage),
		COALESCE(AD.OtherCitizenship, CAD.OtherCitizenship),
		COALESCE(AD.OtherEducation, CAD.OtherEducation),
		COALESCE(AD.OtherLivingArrangement, CAD.OtherLivingArrangement),
		COALESCE(AD.OtherVeteranStatus, CAD.OtherVeteranStatus),
		COALESCE(AD.OtherEmploymentStatus, CAD.OtherEmploymentStatus),
		COALESCE(AD.OtherReligion, CAD.OtherReligion),
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Registration.AdditionalDemographics AD
		CROSS JOIN Registration.AdditionalDemographics CAD
	WHERE
		AD.ContactID = @ParentID
		AND CAD.ContactID = @ChildID;

	SELECT @AdditionalDemographicsID = SCOPE_IDENTITY()

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'AdditionalDemographics', @AdditionalDemographicsID, NULL, @TransactionLogID, 24, @ID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'AdditionalDemographics', @AuditDetailID, @AdditionalDemographicsID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	INSERT INTO ECI.AdditionalDemographics
	(
		RegistrationAdditionalDemographicID,
		ReferralDispositionStatusID,
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
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		@AdditionalDemographicsID,
		COALESCE(PEAD.ReferralDispositionStatusID, CEAD.ReferralDispositionStatusID),
		COALESCE(PEAD.IsCPSInvolved, CEAD.IsCPSInvolved, 0),
		COALESCE(PEAD.IsChildHospitalized, CEAD.IsChildHospitalized),
		COALESCE(PEAD.ExpectedHospitalDischargeDate, CEAD.ExpectedHospitalDischargeDate),
		COALESCE(PEAD.BirthWeightLbs, CEAD.BirthWeightLbs),
		COALESCE(PEAD.BirthWeightOz, CEAD.BirthWeightOz),
		COALESCE(PEAD.IsTransfer, CEAD.IsTransfer),
		COALESCE(PEAD.TransferFrom, CEAD.TransferFrom),
		COALESCE(PEAD.TransferDate, CEAD.TransferDate),
		COALESCE(PEAD.IsOutOfServiceArea, CEAD.IsOutOfServiceArea),
		COALESCE(PEAD.ReportingUnitID, CEAD.ReportingUnitID),
		COALESCE(PEAD.ServiceCoordinatorID, CEAD.ServiceCoordinatorID),
		COALESCE(PEAD.ServiceCoordinatorPhoneID, CEAD.ServiceCoordinatorPhoneID),
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Registration.AdditionalDemographics PRAD
		LEFT OUTER JOIN ECI.AdditionalDemographics PEAD
			ON PRAD.AdditionalDemographicID = PEAD.RegistrationAdditionalDemographicID
		CROSS JOIN Registration.AdditionalDemographics CRAD
		LEFT OUTER JOIN ECI.AdditionalDemographics CEAD
			ON CRAD.AdditionalDemographicID = CEAD.RegistrationAdditionalDemographicID
	WHERE
		PRAD.ContactID = @ParentID
		AND CRAD.ContactID = @ChildID;

	SELECT @ECIAdditionalDemographicID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'ECI', 'AdditionalDemographics', @ECIAdditionalDemographicID, NULL, @TransactionLogID, 24, @ID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ECI', 'AdditionalDemographics', @AuditDetailID, @ECIAdditionalDemographicID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	--Create new presenting problem record for new contact
	INSERT INTO [Registration].[ContactPresentingProblem]
	(
		[ContactID],
		[PresentingProblemTypeID],
		[EffectiveDate],
		[ExpirationDate],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		CreatedBy,
		CreatedOn
	)
	SELECT
		@ID,
		PresentingProblemTypeID,
		EffectiveDate,
		ExpirationDate,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Registration.ContactPresentingProblem CPP
	WHERE
		CPP.ContactID = @ParentID
		AND CPP.IsActive = 1;

	SELECT @PresentingProblemID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactPresentingProblem', @PresentingProblemID, NULL, @TransactionLogID, @ModuleComponentID, @ID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactPresentingProblem', @AuditDetailID, @PresentingProblemID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	EXEC Auditing.usp_AddContactDemographicChangeLog @TransactionLogID, @ID, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT

	--Create MRN record for new contact using MRN of Parent
	SET IDENTITY_INSERT [Registration].[ContactMRN] ON;

	INSERT INTO [Registration].[ContactMRN]
	(
		ContactID,
		MRN,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		@ID,
		MRN,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Registration.ContactMRN
	WHERE
		ContactID = @ParentID

	SET IDENTITY_INSERT [Registration].[ContactMRN] OFF;

	--Inactivate Parent record
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'Contact', @ParentID, NULL, @TransactionLogID, @ModuleComponentID, @ID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Registration.Contact
	SET IsActive = 0,
		ModifiedOn = @ModifiedOn,
		ModifiedBy = @ModifiedBy,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ContactID = @ParentID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'Contact', @AuditDetailID, @ParentID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'ContactMRN', @ParentID, NULL, @TransactionLogID, @ModuleComponentID, @ID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Registration.ContactMRN
	SET IsActive = 0,
		ModifiedOn = @ModifiedOn,
		ModifiedBy = @ModifiedBy,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ContactID = @ParentID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'ContactMRN', @AuditDetailID, @ParentID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	--Inactivate Child record
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'Contact', @ChildID, NULL, @TransactionLogID, @ModuleComponentID, @ID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Registration.Contact
	SET IsActive = 0,
		ModifiedOn = @ModifiedOn,
		ModifiedBy = @ModifiedBy,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ContactID = @ChildID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'Contact', @AuditDetailID, @ChildID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'ContactMRN', @ChildID, NULL, @TransactionLogID, @ModuleComponentID, @ID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Registration.ContactMRN
	SET IsActive = 0,
		ModifiedOn = @ModifiedOn,
		ModifiedBy = @ModifiedBy,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ContactID = @ChildID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'ContactMRN', @AuditDetailID, @ChildID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END