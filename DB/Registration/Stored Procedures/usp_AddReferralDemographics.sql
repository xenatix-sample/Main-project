-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddReferralDemographics]
-- Author:		Sumana Sangapu
-- Date:		12/15/2015
--
-- Purpose:		Add Referral Demographics Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015	Sumana Sangapu		Initial Creation .
-- 12/16/2015	Scott Martin		Added audit logging
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddReferralDemographics]
	@ContactTypeID INT,
	@FirstName NVARCHAR(200),
	@Middle NVARCHAR(200),
	@LastName NVARCHAR(200),
	@SuffixID INT,
	@TitleID INT,
	@PreferredContactMethodID INT,
	@GestationalAge INT,
	@OrganizationName	nvarchar(100),
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID INT OUTPUT
AS
BEGIN
DECLARE @AuditPost XML,
		@AuditID BIGINT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';
		BEGIN TRY

		INSERT INTO Registration.ReferralDemographics
		(
			ContactTypeID, 
			FirstName, 
			Middle, 
			LastName, 
			SuffixID, 
			TitleID, 
			PreferredContactMethodID, 
			GestationalAge,
			OrganizationName,
			IsActive, 
			ModifiedBy, 
			ModifiedOn
		)
		VALUES
		(
			@ContactTypeID, 
			@FirstName, 
			@Middle, 
			@LastName, 
			@SuffixID, 
			@TitleID, 
			@PreferredContactMethodID, 
			@GestationalAge,
			@OrganizationName,
			1, 
			@ModifiedBy, 
			GETUTCDATE()												
		);
		
		SELECT @ID =  SCOPE_IDENTITY();

		INSERT INTO Core.Audits
		(
			ModuleID ,
			AuditTypeID ,
			UserId ,
			AuditTimeStamp ,
			IsArchivable
		)
		VALUES
		(
			[Core].[fn_GetModuleID]('Registration'), 
			[Core].[fn_GetAuditType]('Insert'),
			@ModifiedBy , 
			GETUTCDATE() , 
			0  
		);
			
		SELECT @AuditID = SCOPE_IDENTITY();

		INSERT INTO Core.AuditDetail
		(
			AuditID,
			[AuditPost],
			AuditPrimaryKeyValue,
			SchemaName,
			TableName
		)
		SELECT
			@AuditID,
			(SELECT * FROM Registration.ReferralDemographics WHERE ReferralID = tbl.ReferralID FOR XML AUTO ),
			tbl.ReferralID,
			'Registration',
			'ReferralDemographics'
		FROM
			Registration.ReferralDemographics tbl
		WHERE
			tbl.ReferralID = @ID;

		END TRY
		BEGIN CATCH
			SELECT 
				   @ID = 0,
				   @ResultCode = ERROR_SEVERITY(),
				   @ResultMessage = ERROR_MESSAGE()
		END CATCH
END