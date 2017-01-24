-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_AddUser
-- Author:		Justin Spalti
-- Date:		07/21/2015
--
-- Purpose:		Adds a user into the User table
--
-- Notes:		N/A
--
-- Depends:		fn_GenerateHash
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/21/2015 - Added the procedure header
-- 07/23/2015 - Added dbo to the table
-- 07/23/2015 - demetrios.christopher@xenatix.com - Added password hashing
-- 07/30/2015   John Crossen     Change schema from dbo to Core		
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-- 08/11/2015 - Justin Spalti - Added a new parameter for user roles so that they can be saved while adding a new user record
-- 09/02/2015 - Justin Spalti - Added additional parameters to handle the new email field on the user admin screen
-- 09/08/2015 - Justin Spalti - Updated the code to return back the new identity values in the additional result output parameter
-- 09/22/2015   John Crossen        Add Audit
-- 10/02/2015 - Justin Spalti - Added the IsTemporaryPassword column to the insert statement
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedOn and CreatedBy fields
-- 01/27/2016   Lokesh Singhal Removed encrypted password to be stored in auditdetail as was creating issue during setting in @auditpre as XML
-- 02/09/2016 - Justin Spalti - Removed the roles and credentials parameters since they will be added on a different screen
-- 02/24/2016 - Justin Spalti - Added the new audit logging logic
-- 02/24/2016 - Justin Spalti - Added the GenderID parameter
-- 03/27/2016 - Vamshi Kammari - added logic for validating duplicate Email ID.
-- 05/11/2016 - Kyle Campbell	- TFS #10645	Add user to userfacility table
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddUser]
@UserName NVARCHAR(100),
@FirstName NVARCHAR(50),
@LastName NVARCHAR(50),
@MiddleName NVARCHAR(50),
@GenderID INT,
@Password NVARCHAR(128),
@IsActive BIT,
@EffectiveFromDate DATETIME,
@EffectiveToDate DATETIME,
@PrimaryEmail NVARCHAR(50),
@ModifiedOn DATETIME,
@ModifiedBy INT,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT,
@AdditionalResult XML OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
		DECLARE @NewUserID INT,
				@NewEmailID BIGINT,
				-- Audit Params
				@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
				@AuditDetailID BIGINT,
				@NewID BIGINT,
				@NewUserFacilityID BIGINT


				if (select count(1) from Core.[Email] where Email=@PrimaryEmail) =0 begin
		INSERT INTO Core.[Users](UserName, FirstName, LastName, MiddleName, GenderID, [Password], IsTemporaryPassword, IsActive, EffectiveFromDate, EffectiveToDate, LoginAttempts, ModifiedOn, ModifiedBy, CreatedBy, CreatedOn)
		VALUES(@UserName, @FirstName, @LastName, @MiddleName, @GenderID, Core.fn_GenerateHash(@Password), 1, @IsActive, @EffectiveFromDate, @EffectiveToDate, 0, @ModifiedOn, @ModifiedBy, @ModifiedBy, @ModifiedOn)

		SELECT @NewUserID = SCOPE_IDENTITY()
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'Users', @NewUserID, NULL, NULL, NULL, @NewUserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'Users', @AuditDetailID, @NewUserID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		INSERT INTO [Core].[UserFacility] ([UserID],[FacilityID],[IsActive],[ModifiedBy],[ModifiedOn], [CreatedBy], [CreatedOn])
		VALUES (@NewUserID, 1, 1, @ModifiedBy, @ModifiedOn, @ModifiedBy, @ModifiedOn)
		
		SELECT @NewUserFacilityID = SCOPE_IDENTITY()
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'UserFacility', @NewUserFacilityID, NULL, NULL, NULL, @NewUserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'UserFacility', @AuditDetailID, @NewUserFacilityID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		INSERT INTO Core.[Email](Email, IsActive, ModifiedBy, ModifiedOn, CreatedBy, CreatedOn)
		VALUES(@PrimaryEmail, 1, @ModifiedBy, @ModifiedOn, @ModifiedBy, @ModifiedOn)

		SELECT @NewEmailID = SCOPE_IDENTITY()
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'Email', @NewEmailID, NULL, NULL, NULL, @NewUserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'Email', @AuditDetailID, @NewEmailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		INSERT INTO Core.[UserEmail](UserID, EmailID, IsPrimary, ModifiedOn, ModifiedBy, IsActive, CreatedBy, CreatedOn)
		VALUES(@NewUserID, @NewEmailID, 1, @ModifiedOn, @ModifiedBy, 1, @ModifiedBy, @ModifiedOn)

		SELECT @NewID = SCOPE_IDENTITY()
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'UserEmail', @NewID, NULL, NULL, NULL, @NewUserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'UserEmail', @AuditDetailID, @NewID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		SELECT @AdditionalResult = ( SELECT @NewUserID AS UserIdentifier,
											@NewEmailID AS EmailIdentifier
									 FOR
									 XML PATH('OutParameters'),
									 TYPE
								   )
								   end
								   else
								   begin
								   SELECT @ResultCode = 2,@ResultMessage='Duplicate Email'
								   end

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END