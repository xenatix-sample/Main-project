-----------------------------------------------------------------------------------------------------------------------
-- PROCEDURE:	[USP_DELETEREFERRAL]
-- AUTHOR:		Deepak Kumar
-- DATE:		11/09/2016
--
-- PURPOSE:		DELETE REFERRAL FROM REFERRAL GRID
--
-- NOTES:		
--
-- DEPENDS:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/08/2016   Deepak Kumar  Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_DeleteReferral]
		@ContactID BIGINT,
		@ModifiedOn	DATE,
		@ModifiedBy	INT,
		@ResultCode	INT OUTPUT,
		@ResultMessage	NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0
		,@ResultMessage = 'executed successfully'

	BEGIN TRY

		UPDATE RAD
		SET IsActive = 0
			,ModifiedBy=@ModifiedBy
			,ModifiedOn=@ModifiedOn
		From Registration.ReferralHeader RH
		Inner Join Registration.ReferralAdditionalDetails RAD 
			ON RAD.ReferralHeaderID=RH.ReferralHeaderID
		WHERE RH.ContactID = @ContactID

		UPDATE Registration.ReferralHeader
		SET IsActive = 0
			,ModifiedBy=@ModifiedBy
			,ModifiedOn=@ModifiedOn
		WHERE ContactID = @ContactID

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = error_severity()
			,@ResultMessage = error_message()
	END CATCH
END


