-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateRelationshipContact]
-- Author:		Gurpreet Singh
-- Date:		08/25/2015
--
-- Purpose:		Update Contact Details for Emergency/Collateral
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/26/2015	-	Gurpreet Singh	-	Initial Creation
-- 09/01/2015	-	Gurpreet Singh	-	Added SSN, DriverLicense, AlternateID parameters
-- 09/03/2015	-	Gurpreet Singh	-	Added ParentContactID parameter, Removed Age
-- 09/04/2015	-	Gurpreet Singh	-	Updated IsLivingWithClient datatype to INT
-- 09/08/2015	Rajiv Ranjan		Adedd DriverLicenseStateID & ClientIdentifierTypeID
-- 09/08/2015	John Crossen		TFS#1957 -- Comment out update to ContactTypeID
-- 09/17/2015	Avikal Gupta		TFS#1983 -- Added LivingWithClientStatus, ReceiveCorrespondenceID, EmergencyContact and removed LivingWithClientStatusID

-- 09/25/2015 - John Crossen    - Refactor Proc to use PK for update
-- 10/28/2016   John Crossen    tfs#3070  Change EmergencyContact Field
-- 12/16/2015	Scott Martin	Added audit logging
-- 01/08/2015	Vishal Joshi 	Removed AlternateID and ClientIndentifierTypeID
-- 01/12/2016	Gaurav Gupta 	added EducationStatusID,EducationStatusID
-- 03/16/2016	Rajiv Ranjan	Added vetranstatusID
-- 03/17/2016   Arun Choudhary  Added SchoolAttended,SchoolBeginDate,SchoolEndDate
-- 04/12/2016   Arun Choudhary	Added IsPolicyHolder
-- 06/07/2016   Lokesh Singhal	Removed IsPolicyHolder,OtherRelationship,RelationshipTypeID
-- 09/14/2016    Arun Choudhary	Added CollateralEffectiveDate and CollateralExpirationDate
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_UpdateRelationshipContact]
	@ContactRelationshipID BIGINT,
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
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@EncryptedValue varbinary(2000)

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
	
	EXEC Core.usp_OpenEncryptionkeys @ResultCode OUTPUT, @ResultMessage OUTPUT 

	SELECT @EncryptedValue = Core.fn_Encrypt(@SSN)

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'Contact', @ContactID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;	

	UPDATE [Registration].[Contact]
	SET [FirstName] = @FirstName,
		[Middle] = @Middle,
		[LastName] = @LastName,
		[SuffixID] = @SuffixID,
		[GenderID] = @GenderID,
		[DOB] = @DOB,
		SSN = SUBSTRING(ISNULL(@SSN,''),6,LEN(@SSN)),
		[DriverLicense] = @DriverLicense,
		[DriverLicenseStateID]=@DriverLicenseStateID,
		[ModifiedBy] = @ModifiedBy,
		[ModifiedOn] = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE(),
		SSNEncrypted = @EncryptedValue
	WHERE
		ContactID = @ContactID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'Contact', @AuditDetailID, @ContactID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
---------------------------------------Contact Relationship------------------------------------------------------

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactRelationship', @ContactRelationshipID, NULL, NULL, NULL, @ParentContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;	

	UPDATE [Registration].[ContactRelationship]
	SET [LivingWithClientStatus] = @LivingWithClientStatus,
		[ReceiveCorrespondenceID] = @ReceiveCorrespondenceID,
		IsEmergency=@EmergencyContact,
		EducationStatusID = @EducationStatusID ,
		[SchoolAttended] = @SchoolAttended,
		[SchoolBeginDate] = @SchoolBeginDate,
		[SchoolEndDate] = @SchoolEndDate,
		EmploymentStatusID = @EmploymentStatusID ,
		[VeteranStatusID] = @VeteranStatusID,
		[CollateralEffectiveDate] = @CollateralEffectiveDate,
		[CollateralExpirationDate] = @CollateralExpirationDate,
		[ModifiedBy] = @ModifiedBy,
		[ModifiedOn] = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ContactRelationshipID = @ContactRelationshipID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactRelationship', @AuditDetailID, @ContactRelationshipID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END