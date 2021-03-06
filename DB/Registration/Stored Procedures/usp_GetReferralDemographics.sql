
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetReferralDemographics]
-- Author:		Sumana Sangapu
-- Date:		12/15/2015
--
-- Purpose:		Get Referral Demographics Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015	Sumana Sangapu	Initial Creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetReferralDemographics]
@ReferralID BIGINT,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY

		SELECT 
			ReferralID, ContactTypeID,  FirstName, Middle, LastName, SuffixID, TitleID, cm.ContactMethodID AS ContactMethodID, GestationalAge, OrganizationName
		FROM 
			Registration.ReferralDemographics c
			LEFT OUTER JOIN Reference.ContactMethod cm ON cm.ContactMethodID = c.PreferredContactMethodID
		WHERE 
			c.ReferralID = @ReferralID AND c.IsActive = 1

	END TRY
	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END