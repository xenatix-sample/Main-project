-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddRelationshipContact]
-- Author:		Gurpreet Singh
-- Date:		08/26/2015
--
-- Purpose:		Add Contact Details for Emergency/Collateral
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/26/2015	-	Gurpreet Singh	-	Initial Creation
-- 08/26/2015   -   Gurpreet Singh  -	Updated Insert query for ContactRelationship table
-- 08/27/2015   -   Gurpreet Singh	-	Added ID output parameter
-- 09/01/2015	-	Gurpreet Singh	-	Added SSN, DriverLicense, AlternateID parameters
-- 09/03/2015	-	Gurpreet Singh	-	Added case to update contacts and create new relation in case of existing contact, Remove Age
-- 09/04/2015	-	Gurpreet Singh	-	Updated IsLivingWithClient datatype to INT
-- 09/08/2015	Rajiv Ranjan		Adedd DriverLicenseStateID & ClientIdentifierTypeID
-- 09/17/2015	Avikal Gupta		TFS#1983 -- Added LivingWithClientStatus, ReceiveCorrespondenceID, EmergencyContact and removed LivingWithClientStatusID
-- 10/28/2016   John Crossen    tfs#3070  Change EmergencyContact Field
-- 12/16/2015	Scott Martin	Added audit logging
-- 01/12/2016 - Gaurav Gupta   - Added EmploymentStatusID,EducationStatusID column in [Registration].[ContactRelationship] insert 
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn field, added CreatedBy and CreatedOn to Insert
-- 03/16/2016	Rajiv Ranjan	Added @VeteranStatusID
-- 03/17/2016   Arun Choudhary  Added SchoolAttended,SchoolBeginDate,SchoolEndDate
-- 04/12/2016   Arun Choudhary	Added IsPolicyHolder
-- 05/059/2016	Scott Martin	Added encryption of ssn to update portion of the code
-- 06/07/2016   Lokesh Singhal	Removed IsPolicyHolder,OtherRelationship,RelationshipTypeID
-- 09/14/2016    Arun Choudhary	Added CollateralEffectiveDate and CollateralExpirationDate
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddRelationshipContact]
	@ParentContactID BIGINT,
	@ContactID BIGINT,
	@ContactTypeID INT,
	@FirstName NVARCHAR(200),
	@Middle NVARCHAR(200),
	@LastName NVARCHAR(200),
	@SuffixID INT,
	@GenderID INT,
	@DOB DATE,
	@LivingWithClientStatus BIT,
	@ReceiveCorrespondenceID INT,
	@SSN NVARCHAR(9),
	@DriverLicense NVARCHAR(50),
	@DriverLicenseStateID INT,
	@EmergencyContact BIT,
    @EducationStatusID INT,
	@SchoolAttended NVARCHAR(250),
	@SchoolBeginDate DATE,
	@SchoolEndDate DATE,
	@EmploymentStatusID INT,
	@VeteranStatusID INT,
	@CollateralEffectiveDate DATE,
	@CollateralExpirationDate DATE,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ContactRelationshipID BIGINT,
		@EncryptedValue varbinary(2000)

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
	
	EXEC Core.usp_OpenEncryptionkeys @ResultCode OUTPUT, @ResultMessage OUTPUT 

	SELECT @EncryptedValue = Core.fn_Encrypt(@SSN)

	IF (@ContactID <= 0)
		BEGIN
		INSERT INTO [Registration].[Contact]
		(
			[ContactTypeID],
			[FirstName],
			[Middle],
			[LastName],
			[SuffixID],
			[GenderID],
			[DOB],
			[SSN],
			[DriverLicense],
			[DriverLicenseStateID],
			[ModifiedBy],
			[ModifiedOn],
			[IsActive],
			CreatedBy,
			CreatedOn,
			SSNEncrypted
		)
		VALUES
		(
			@ContactTypeID,
			@FirstName,
			@Middle,
			@LastName,
			@SuffixID,
			@GenderID,
			@DOB,
			SUBSTRING(ISNULL(@SSN,''),6,LEN(@SSN)),
			@DriverLicense,
			@DriverLicenseStateID,
			@ModifiedBy,
			@ModifiedOn,
			1,
			@ModifiedBy,
			@ModifiedOn,
			@EncryptedValue
		);
			
		SET @ContactID =  SCOPE_IDENTITY();

		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'Contact', @ContactID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'Contact', @AuditDetailID, @ContactID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;	
		END
	ELSE
		BEGIN
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'Contact', @ContactID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;		

		UPDATE [Registration].[Contact]
		SET [FirstName] = @FirstName,
			[Middle] = @Middle,
			[LastName] = @LastName,
			[SuffixID] = @SuffixID,
			[GenderID] = @GenderID,
			[DOB] = @DOB,
			[SSN] = SUBSTRING(ISNULL(@SSN,''),6,LEN(@SSN)),
			[SSNEncrypted] = @EncryptedValue,
			[DriverLicense] = @DriverLicense,
			[DriverLicenseStateID]=@DriverLicenseStateID,
			[ModifiedBy] = @ModifiedBy,
			[ModifiedOn] = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE
			ContactID = @ContactID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'Contact', @AuditDetailID, @ContactID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END

	IF NOT EXISTS (SELECT TOP 1 * FROM Registration.ContactRelationship WHERE ParentContactID = @ParentContactID AND ChildContactID = @ContactID)
		BEGIN
		INSERT INTO [Registration].[ContactRelationship]
		(
			[ParentContactID],
			[ChildContactID],
			[LivingWithClientStatus],
			[ReceiveCorrespondenceID],
			[ContactTypeID],
			IsEmergency,
			[EducationStatusID],
			[SchoolAttended],
			[SchoolBeginDate],
			[SchoolEndDate],
			[EmploymentStatusID],
			[VeteranStatusID],
			[CollateralEffectiveDate],
			[CollateralExpirationDate],
			[ModifiedBy],
			[ModifiedOn],
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			@ParentContactID,
			@ContactID,
			@LivingWithClientStatus,
			@ReceiveCorrespondenceID,
			@ContactTypeID,
			@EmergencyContact,
			@EducationStatusID ,
			@SchoolAttended,
			@SchoolBeginDate,
			@SchoolEndDate,
			@EmploymentStatusID,
			@VeteranStatusID,
			@CollateralEffectiveDate,
			@CollateralExpirationDate,
			@ModifiedBy,
			@ModifiedOn,
			@ModifiedBy,
			@ModifiedOn
		);

		SELECT @ContactRelationshipID = SCOPE_IDENTITY();

		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactRelationship', @ContactRelationshipID, NULL, NULL, NULL, @ParentContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactRelationship', @AuditDetailID, @ContactRelationshipID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END
	ELSE
		BEGIN
		SET @ContactRelationshipID = (SELECT ContactRelationshipID FROM Registration.ContactRelationship WHERE ParentContactID = @ParentContactID AND ChildContactID = @ContactID);

		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactRelationship', @ContactRelationshipID, NULL, NULL, NULL, @ParentContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;		

		UPDATE [Registration].[ContactRelationship]
		SET [LivingWithClientStatus] = @LivingWithClientStatus,
			[ReceiveCorrespondenceID] = @ReceiveCorrespondenceID,
			[ContactTypeID] = @ContactTypeID,
			IsEmergency = @EmergencyContact,
			[EducationStatusID] = @EducationStatusID,
			[SchoolAttended] = @SchoolAttended,
			[SchoolBeginDate] = @SchoolBeginDate,
			[SchoolEndDate] = @SchoolEndDate,
			[EmploymentStatusID] = @EmploymentStatusID,
			[VeteranStatusID] = @VeteranStatusID,
			[IsActive] = 1,
			[ModifiedBy] = @ModifiedBy,
			[ModifiedOn] = @ModifiedOn,
			[SystemModifiedOn] = GETUTCDATE()
		WHERE
			ContactRelationshipID = @ContactRelationshipID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactRelationship', @AuditDetailID, @ContactRelationshipID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END

	SELECT @ID = @ContactID;

	END TRY

	BEGIN CATCH
		SELECT @ID = 0,
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE();
	END CATCH
END