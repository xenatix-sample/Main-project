-----------------------------------------------------------------------------------------------------------------------
-- procedure:	[usp_GetReferralsDetails]
-- Author:		Deepak Kumar
-- Date:		11/08/2016
--
-- Purpose:		Get referral details for referral grid
--
-- Notes:		
--
-- Depends:		
--
-- Revision History ---------------------------------------------------------------------------------------------------
-- 11/08/2016   Deepak Kumar  Initial Creation  EXEC [Registration].[usp_GetReferralsDetails]
-- 12/28/2016	Gurpreet Singh		Removed harcoded contactId Bug#21627
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Registration].[usp_GetReferralsDetails] 
	@ContactID BIGINT
	,@ResultCode INT OUTPUT
	,@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0
		,@ResultMessage = 'executed successfully'

	BEGIN TRY
		WITH ctephone
		AS (
			SELECT row_number() OVER (
					PARTITION BY c.ContactID ORDER BY cp.ModifiedOn DESC
					) rowno
				,c.ContactID
				,p.Number
			FROM Registration.ReferralAdditionalDetails rad
			INNER JOIN Registration.ReferralHeader rh ON rad.ReferralHeaderID = rh.ReferralHeaderID
			INNER JOIN Registration.Contact c ON c.ContactID = rh.ContactID
			LEFT JOIN Registration.ContactPhone cp ON cp.ContactID = c.ContactID
				AND cp.IsActive = 1
			LEFT JOIN Core.Phone p ON p.PhoneID = cp.PhoneID
				AND p.IsActive = 1
			WHERE rad.ContactID = @ContactID
				AND rad.IsActive = 1
				AND rh.IsActive = 1
				AND c.IsActive = 1
			)
			,ctereferrals
		AS (
			SELECT c.FirstName
				,c.LastName
				,p.Number AS PhoneNumber
				,rh.ReferralDate
				,rh.ReferralSourceID
				,rc.ReferralConcern
				,c.ContactID
				,rh.ReferralHeaderID
				,rad.ModifiedOn
				,rh.IsReferrerConvertedToCollateral
			FROM Registration.ReferralAdditionalDetails rad
			INNER JOIN Registration.ReferralHeader rh ON rad.ReferralHeaderID = rh.ReferralHeaderID
			INNER JOIN Registration.Contact c ON c.ContactID = rh.ContactID
			LEFT JOIN ctephone p ON p.ContactID = c.ContactID
				AND rowno = 1
			LEFT JOIN Registration.ReferralConcernDetails rcd ON rh.ReferralHeaderID = rcd.ReferralAdditionalDetailID
				AND rcd.IsActive = 1 --rcd has referralheader with the name of referraladditionaldetailid as per observation
			LEFT JOIN Reference.ReferralConcern rc ON rcd.ReferralConcernID = rc.ReferralConcernID
				AND rc.IsActive = 1
			WHERE rad.ContactID = @ContactID
				AND rad.IsActive = 1
				AND rh.IsActive = 1
				AND c.IsActive = 1
			)
		SELECT DISTINCT cr.FirstName
			,cr.LastName
			,PhoneNumber
			,cr.ReferralDate
			,cr.ReferralSourceID
			,cr.ContactID
			,cr.ReferralHeaderID
			,cr.ModifiedOn
			,cr.IsReferrerConvertedToCollateral
			,stuff((
					SELECT ',' + crs.ReferralConcern
					FROM ctereferrals crs
					WHERE crs.ReferralHeaderID = cr.ReferralHeaderID
					FOR XML path('')
					), 1, 1, '') AS ReferralConcern
		FROM ctereferrals cr
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = error_severity()
			,@ResultMessage = error_message()
	END CATCH
END