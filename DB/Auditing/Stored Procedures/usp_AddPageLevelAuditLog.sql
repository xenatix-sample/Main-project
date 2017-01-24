-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddPageLevelAuditLog]
-- Author:		Sumana Sangapu
-- Date:		12/12/2016
--
-- Purpose:		Add PageLevel Audit
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/12/2016	Sumana Sangapu	Initial Creation 
-- 12/15/2016	Sumana Sangapu	Added ModifiedBy as part of the DB table definition prototype
-- 12/20/2016	Sumana Sangapu	Add SearchText
-- 12/22/2016	Gaurav Gupta	Commented SearchText Field
-- 12/23/2016	Kishan Mootha	added SearchText feild back again.
-- 01/23/2017	Scott Martin	Returning only User Roles and Credentials that are Active
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Auditing].[usp_AddPageLevelAuditLog]
	@TransactionLogID bigint,
	@UserID int,
	@ContactID bigint=NULL,
	@DataKey nvarchar(250),
	@ActionTypeID int,
	@IsCareMember bit=NULL,
	@IsBreaktheGlassEnabled bit=NULL,
	@SearchText nvarchar(4000),
	@CreatedOn datetime,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
				If @ContactID = 0
				set @ContactID = null

				 DECLARE @UserCredentials nvarchar(4000)
				 DECLARE @UserRoles nvarchar(4000)
				 DECLARE @ModuleComponentID INT
				 
				 SELECT @ModuleComponentID = ModuleComponentID FROM Core.ModuleComponent WHERE DataKey = @DataKey
				 			 
				 SELECT @UserCredentials = COALESCE(@UserCredentials+', ' ,' ') + CredentialName 
				 FROM   Core.UserCredential uc
				 INNER JOIN Reference.[Credentials] c 
				 ON		uc.CredentialID = c.CredentialID
				 WHERE  uc.UserID = @UserID 
				 AND	uc.IsActive = 1
				 
				 SELECT @UserRoles = COALESCE(@UserRoles+', ' ,' ') + Name 
				 FROM   Core.UserRole ur
				 INNER JOIN Core.[Role] r
				 ON		ur.RoleID = r.RoleID
				 WHERE  ur.UserID = @UserID
				 AND	ur.IsActive = 1
				 
				 INSERT INTO Auditing.PageLevelAuditLog
				 (
					TransactionLogID,UserID, ContactID, ModuleComponentID, ActionTypeID, UserCredentials, UserRoles,SearchText,IsCareMember, IsBreaktheGlassEnabled, CreatedOn,ModifiedBy
				 ) 
				 VALUES
				 (
					@TransactionLogID,
					@UserID,
					@ContactID,
					@ModuleComponentID,
					@ActionTypeID,
					@UserCredentials,
					@UserRoles,
					@SearchText,
					@IsCareMember,
					@IsBreaktheGlassEnabled,
					@CreatedOn,
					@ModifiedBy
				 )
				
	END TRY
	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE();					
	END CATCH
END
