-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddContactDemographics]
-- Author:		Saurabh Sahu
-- Date:		07/23/2015
--
-- Purpose:		Add Contact Demographics Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Saurabh Sahu		Modification .
-- 7/31/2015    John Crossen    Change schema from dbo to Registration
-- 08/03/2015   Sumana Sangapu	1016 - Changed proc name schema qualifier from dbo to Registration/Core/Reference
-- 08/05/2015	Saurabh Sahu		1) Added SchoolDistrict 2) Modify DriversLicense to AlternateID  .
-- 08/14/2015	Sumana Sangapu	1227 Refactor schema for contactmethods
-- 08/19/2015	Sumana Sangapu		1514	- SuffixID and TitleID - corrected
-- 08/27/2015	Arun Choudhary		Added a new column for Driver License and removed [SmokingStatusID], [FullCodeDNR] and [SchoolDistrictID] 
-- 09/03/2015	Rajiv Ranjan		Removed marital status & age parameter
-- 09/08/2015	Rajiv Ranjan		Adedd DriverLicenseStateID & ClientIdentifierTypeID
-- 10/28/2015	Sumana Sangapu		Added GestationalAge
-- 12/23/2015	Rajiv Ranjan		Added @PreferredGenderID parameter
-- 12/23/2015	Scott Martin		Removed AlternateID and ClientIndentifierTypeID
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-- 03/08/2016	Kyle Campbell		TFS #7099	Renamed "Alias" to "PreferredName"
-- 03/15/2016	Sumana Sangapu		SSN Encryption and display last four SSN digits
-- 09/16/2016	Scott Martin		Added TransactionLogID and ModuleComponentID and modified Audit proc call
-- 09/16/2016	Kyle Campbell		TFS #14793	Add Change Log proc call
-- 10/05/2016   Vishal Joshi		Added IsDeceased and CauseOfDeath parameters
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddContactDemographics]
	@TransactionLogID BIGINT,
	@ModuleComponentID BIGINT,
	@ContactID BIGINT,
	@ContactTypeID INT,
	@ClientTypeID INT,
	@FirstName NVARCHAR(200),
	@Middle NVARCHAR(200),
	@LastName NVARCHAR(200),
	@SuffixID INT,
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
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID INT OUTPUT
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

	INSERT INTO Registration.Contact
	(
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
		DeceasedDate, 
		PreferredContactMethodID, 
		ReferralSourceID, 
		IsActive, 
		ModifiedBy, 
		ModifiedOn,
		IsPregnant,
		GestationalAge,
		CreatedBy,
		CreatedOn,
		SSNEncrypted,
		IsDeceased,
		CauseOfDeath
	)
	VALUES
	(
		@ContactTypeID, 
		@ClientTypeID, 
		@FirstName, 
		@Middle, 
		@LastName, 
		@SuffixID, 
		@GenderID, 
		@PreferredGenderID,
		@TitleID, 
		@SequesteredByID, 
		@DOB, 
		@DOBStatusID, 
		SUBSTRING(ISNULL(@SSN,''),6,LEN(@SSN)),
		@SSNStatusID, 
		@DriverLicense,
		@DriverLicenseStateID,
		@PreferredName,
		@DeceasedDate, 
		@PreferredContactMethodID, 
		@ReferralSourceID,
		1, 
		@ModifiedBy, 
		@ModifiedOn,
		@IsPregnant,
		@GestationalAge,
		@ModifiedBy,
		@ModifiedOn,
		@EncryptedValue,
		@IsDeceased,
		@CauseOfDeath

	);
		
	SELECT @ID =  SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'Contact', @ID, NULL, @TransactionLogID, @ModuleComponentID, @ID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'Contact', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddContactDemographicChangeLog @TransactionLogID, @ID, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT

	END TRY

	BEGIN CATCH
		SELECT 
				@ID = 0,
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
