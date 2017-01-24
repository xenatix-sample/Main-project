-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_GetReferralAdditionalDetailsByContact]
-- Author:		Lokesh Singhal
-- Date:		01/15/2016
--
-- Purpose:		Get a specific referral additiobal details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/15/2016	Lokesh Singhal	Initial creation.
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Registration].[usp_GetReferralAdditionalDetailsByContact]
       @ContactID BIGINT,
       @ResultCode INT OUTPUT,
       @ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

       BEGIN TRY
       SELECT @ResultCode = 0,
                 @ResultMessage = 'executed successfully'

       SELECT	RD.ReferralAdditionalDetailID, RD.ReferralHeaderID, RD.ContactID, 
				RD.ReasonforCare, RD.IsTransferred, RD.IsHousingProgram, RD.HousingDescription, RD.IsEligibleforFurlough, 
				RD.IsReferralDischargeOrTransfer, RD.IsConsentRequired, 
				RD.Comments, RD.AdditionalConcerns,RH.ContactID as [HeaderContactID]
       FROM
			  [Registration].[ReferralAdditionalDetails] RD 
			  JOIN [Registration].[ReferralHeader] RH 
			  ON RD.ReferralHeaderID=Rh.ReferralHeaderID
       WHERE
              RD.ContactID = @ContactID
              AND RD.IsActive = 1
      END TRY
       BEGIN CATCH
              SELECT @ResultCode = ERROR_SEVERITY(),
                     @ResultMessage = ERROR_MESSAGE()
       END CATCH
END