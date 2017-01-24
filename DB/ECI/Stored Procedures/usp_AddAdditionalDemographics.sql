----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetAdditionalDemographics]
-- Author:		Scott Martin
-- Date:		1/5/2015
--
-- Purpose:		Adds ECI additional demographic info
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/5/2015	Scott Martin		Initial creation.
-- 03/10/2016  Lokesh Singhal   Passed Missing Parameter.
-- 03/18/2016  Lokesh Singhal   Removed @RaceID and added extra parameters passed to registration additioanl demographic sproc
-- 05/11/2016  Arun				Added null for PlaceofEmployment, BeginDate and End Date
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_AddAdditionalDemographics]
	@ContactID BIGINT,
	@ReferralDispositionStatusID INT,
	@SchoolDistrictID INT,
	@OtherRace NVARCHAR(250),
	@EthnicityID INT,
	@OtherEthnicity NVARCHAR(250),
	@PrimaryLanguageID INT,
	@OtherPreferredLanguage NVARCHAR(250),
	@InterpreterRequired BIT,
	@IsCPSInvolved BIT,
	@IsChildHospitalized BIT,
	@ExpectedHospitalDischargeDate DATE,
	@BirthWeightLbs SMALLINT,
	@BirthWeightOz SMALLINT,
	@IsTransfer BIT,
	@TransferFrom NVARCHAR(250),
	@TransferDate DATE,
	@IsOutOfServiceArea BIT,
	@ReportingUnitID INT,
	@ServiceCoordinatorID INT,
	@ServiceCoordinatorPhoneID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@AdditionalDemographicID BIGINT,
		@ID BIGINT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
	IF EXISTS (SELECT TOP 1 * FROM Registration.AdditionalDemographics WHERE ContactID = @ContactID AND IsActive = 1)
		BEGIN
		SELECT TOP 1 @AdditionalDemographicID = AdditionalDemographicID FROM Registration.AdditionalDemographics WHERE ContactID = @ContactID AND IsActive = 1;

		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'ECI', 'AdditionalDemographics', @AdditionalDemographicID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE	AD
		SET	IsActive = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		FROM
			Registration.AdditionalDemographics AD
		WHERE
			AdditionalDemographicID = @AdditionalDemographicID;

		EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'ECI', 'AdditionalDemographics', @AuditDetailID, @AdditionalDemographicID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		END

	EXEC Registration.usp_AddAdditionalDemographics @ContactID, NULL, NULL,NULL, @OtherRace, @OtherEthnicity, NULL, @SchoolDistrictID,
			 @EthnicityID, @PrimaryLanguageID, NULL, NULL, NULL, NULL, NULL, NULL ,NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,NULL, NULL, NULL, @InterpreterRequired,
			NULL, @OtherPreferredLanguage, NULL, NULL, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode, @ResultMessage;

	SELECT @AdditionalDemographicID = AdditionalDemographicID FROM Registration.AdditionalDemographics WHERE ContactID = @ContactID AND IsActive = 1;

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
	VALUES
	(
		@AdditionalDemographicID,
		@ReferralDispositionStatusID,
		@IsCPSInvolved,
		@IsChildHospitalized,
		@ExpectedHospitalDischargeDate,
		@BirthWeightLbs,
		@BirthWeightOz,
		@IsTransfer,
		@TransferFrom,
		@TransferDate,
		@IsOutOfServiceArea,
		@ReportingUnitID,
		@ServiceCoordinatorID,
		@ServiceCoordinatorPhoneID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'ECI', 'AdditionalDemographics', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ECI', 'AdditionalDemographics', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END