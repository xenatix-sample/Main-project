-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_GetReferralHeader]
-- Author:		Arun Choudhary
-- Date:		12/18/2015
--
-- Purpose:		Get a specific referral header
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/18/2015	Arun Choudhary	Initial creation.
-- 04/12/2016   Lokesh Singhal  Added OtherSource column
-- 01/04/2016	Sumana Sangapu	Added IsLinkedContact and IsReferrerConvertedToContact
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Registration].[usp_GetReferralHeader]
	@ReferralHeaderID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		rh.[ReferralHeaderID],
		rh.ContactID,
		[ReferralStatusID],
		[ReferralTypeID],
		[ResourceTypeID],
		[ReferralCategorySourceID],
		[ReferralOriginID],
		[ProgramID],
		[ReferralOrganizationID],
		[OtherOrganization],
		[ReferralSourceID],
		[OtherSource],
		[ReferralDate],
		[IsLinkedToContact],
		[IsReferrerConvertedToCollateral],
		rad.ContactID as ParentContactID,
		rh.[IsActive],
		rh.[ModifiedBy],
		rh.[ModifiedOn]
	FROM
		[Registration].[ReferralHeader] rh
			INNER JOIN Registration.ReferralAdditionalDetails rad ON rad.ReferralHeaderID = rh.ReferralHeaderID
	WHERE
		rh.ReferralHeaderID = @ReferralHeaderID
		AND rh.IsActive = 1;
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

