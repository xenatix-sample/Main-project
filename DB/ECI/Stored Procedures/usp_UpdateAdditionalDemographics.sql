----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateAdditionalDemographics]
-- Author:		Scott Martin
-- Date:		1/5/2015
--
-- Purpose:		Updates ECI additional demographic info
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/05/2015 - Scott Martin - Initial creation.
-- 01/14/2016 - Scott Martin - Added ModifiedOn parameter, Added SystemModifiedOn field
-- 02/01/2016 - Justin Spalti - Reversed the order of the ModifiedOn and ModifiedBy parameters
-- 10032016     Lokesh Singhal  Passed Missing Parameter.
-- 03/18/2016  Lokesh Singhal   Removed @RaceID and added extra parameters passed to registration additioanl demographic sproc
-- 05/11/2016  Arun				Added null for PlaceofEmployment, BeginDate and End Date
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_UpdateAdditionalDemographics]
	@AdditionalDemographicID BIGINT,
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
		@ECIAdditionalDemographicID BIGINT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
	EXEC Registration.usp_UpdateAdditionalDemographics @AdditionalDemographicID, @ContactID, NULL, NULL,NULL, @OtherRace, @OtherEthnicity, NULL, @SchoolDistrictID,
			@EthnicityID, @PrimaryLanguageID, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,NULL, NULL, NULL, @InterpreterRequired, NULL,
			@OtherPreferredLanguage, NULL, NULL, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode, @ResultMessage;

	SELECT @ECIAdditionalDemographicID = AdditionalDemographicID FROM ECI.AdditionalDemographics WHERE RegistrationAdditionalDemographicID = @AdditionalDemographicID AND IsActive = 1;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'ECI', 'AdditionalDemographics', @ECIAdditionalDemographicID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE ECI.AdditionalDemographics
	SET	RegistrationAdditionalDemographicID = @AdditionalDemographicID,
		ReferralDispositionStatusID = @ReferralDispositionStatusID,
		IsCPSInvolved = @IsCPSInvolved,
		IsChildHospitalized = @IsChildHospitalized,
		ExpectedHospitalDischargeDate = @ExpectedHospitalDischargeDate,
		BirthWeightLbs = @BirthWeightLbs,
		BirthWeightOz = @BirthWeightOz,
		IsTransfer = @IsTransfer,
		TransferFrom = @TransferFrom,
		TransferDate = @TransferDate,
		IsOutOfServiceArea = @IsOutOfServiceArea,
		ReportingUnitID = @ReportingUnitID,
		ServiceCoordinatorID = @ServiceCoordinatorID,
		ServiceCoordinatorPhoneID = @ServiceCoordinatorPhoneID,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		AdditionalDemographicID = @ECIAdditionalDemographicID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'ECI', 'AdditionalDemographics', @AuditDetailID, @ECIAdditionalDemographicID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY
			
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END