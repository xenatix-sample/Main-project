-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateContactDemographics]
-- Author:		Saurabh Sahu
-- Date:		07/23/2015
--
-- Purpose:		Update Contact Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Saurabh Sahu		Modification.
-- 7/31/2015    John Crossen    Change schema from dbo to Registration
-- 08/04/2015 - John Crossen -- Data type mismatches resolved
-- 08/04/2015   Sumana Sangapu  875 - Input IsPregnant parameter

-- exec Registration.usp_UpdateContactDemographics 1,1,2,'Mike','','Charles','Sr.',1,1,1,'12/15/1959',1,123456789,1,'123456','1',1,1,'12/01/1988',1,1,1,1,'',78,'asdfd',1,1,'','',''
-- 08/05/2015	Saurabh Sahu		1) Added SchoolDistrict 2) Modify DriversLicense to AlternateID  .
-- 08/13/2015	Sumana Sangapu		TAsk 1227 - Refactor Schema
-- 08/15/2015	Rajiv Ranjan		Parameter sequence of @IsPregnant & @SchoolDistrictID corrected
-- 08/19/2015	Sumana Sangapu		1514	- SuffixID and TitleID - corrected
-- 08/27/2015	Arun Choudhary		Added a new column for Driver License and removed [SmokingStatusID], [FullCodeDNR] and [SchoolDistrictID] 
-- 08/31/2015	Sumana Sangapu		Added a case to handle if gender is male then default IsPregnant to false
-- 09/03/2015	Rajiv Ranjan		Removed marital status & age
-- 09/08/2015	Rajiv Ranjan		Adedd DriverLicenseStateID & ClientIdentifierTypeID
-- 10/28/2015	Sumana Sangapu		Added GestationalAge
-- 12/16/2015	Scott Martin		Added audit logging
-- 12/23/2015	Rajiv Ranjan		Added @PreferredGenderID parameter
-- 01/04/2015	Gurpreet Singh		Updated contact type
-- 12/23/2015	Scott Martin		Removed AlternateID and ClientIndentifierTypeID
-- 03/08/2016	Kyle Campbell		TFS #7099	Renamed "Alias" to "PreferredName"
-- 03/15/2016	Sumana Sangapu		SSN Encryption and display last four SSN digits
-- 04/29/2016	Scott Martin		Added code to insert Company Admission record if one doesn't exist or if currently discharged from company
-- 09/16/2016	Scott Martin		Added TransactionLogID and ModuleComponentID and modified Audit proc call
-- 09/16/2016	Kyle Campbell		TFS #14793	Add Change Log proc call
-- 10/05/2016   Vishal Joshi		Added IsDeceased and CauseOfDeath parameters
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Registration].[usp_UpdateContactDemographics]
	@TransactionLogID BIGINT,
	@ModuleComponentID BIGINT,
	@ContactID BIGINT,
	@ContactTypeID INT,
	@ClientTypeID INT,
	@FirstName NVARCHAR(200),
	@Middle NVARCHAR(200),
	@LastName NVARCHAR(200),
	@SuffixID int,
	@GenderID INT,
	@PreferredGenderID INT,
	@TitleID INT,
	@SequesteredByID INT,
	@DOB DATE,
	@DOBStatusID INT,
	@SSN NVARCHAR(9),
	@SSNStatusID INT,
	@DriverLicense NVARCHAR(50),
	@DriverLicenseStateID INT,
	@IsPregnant BIT = 0,
	@PreferredName NVARCHAR(200),
	@IsDeceased BIT = 0,
	@DeceasedDate DATETIME,
	@CauseOfDeath INT,
	@PreferredContactMethodID INT,
	@ReferralSourceID INT,
	@GestationalAge DECIMAL(3,1),
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@EncryptedValue varbinary(2000),
		@OrganizationID BIGINT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY

	EXEC Core.usp_OpenEncryptionkeys @ResultCode OUTPUT, @ResultMessage OUTPUT

	SELECT @EncryptedValue = Core.fn_Encrypt(@SSN)
	
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'Contact', @ContactID, NULL, @TransactionLogID, @ModuleComponentID, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
					
	UPDATE Registration.Contact
	SET 
		ClientTypeID = @ClientTypeID,
		FirstName = @FirstName,
		Middle = @Middle,
		LastName = @LastName,
		SuffixID = @SuffixID,
		GenderID = @GenderID,
		PreferredGenderID = @PreferredGenderID,
		TitleID = @TitleID,
		SequesteredByID = @SequesteredByID,
		DOB = @DOB,
		DOBStatusID = @DOBStatusID,
		SSN = SUBSTRING(ISNULL(@SSN,''),6,LEN(@SSN)),
		SSNStatusID = @SSNStatusID,
		DriverLicense = @DriverLicense,
		DriverLicenseStateID=@DriverLicenseStateID,
		PreferredName = @PreferredName,
		IsDeceased = @IsDeceased,
		DeceasedDate = @DeceasedDate,
		CauseOfDeath = @CauseOfDeath,
		PreferredContactMethodID = @PreferredContactMethodID,
		ReferralSourceID = @ReferralSourceID,
		IsPregnant = CASE WHEN @GenderID = 1 THEN 0 ELSE @IsPregnant END, -- If Male default @IsPregnant to 0
		GestationalAge = @GestationalAge,
		ContactTypeID = @ContactTypeID,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE(),
		SSNEncrypted = @EncryptedValue

	WHERE 
		ContactID = @ContactID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'Contact', @AuditDetailID, @ContactID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddContactDemographicChangeLog @TransactionLogID, @ContactID, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT
			
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
