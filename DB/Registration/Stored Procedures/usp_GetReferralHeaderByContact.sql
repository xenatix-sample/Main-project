-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_GetReferralHeaderByContact]
-- Author:		Lokesh Singhal
-- Date:		01/06/2016
--
-- Purpose:		Get a specific referral header
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/06/2016	Lokesh Singhal	Initial creation.
-- 01/08/2016   Lokesh Singhal  Join with ReferralAdditionalDetails for contactdid search
-- 01/18/2017	Sumana Sangapu	Added join to Registration.ContactRelationship
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Registration].[usp_GetReferralHeaderByContact]
       @ContactID BIGINT,
       @ResultCode INT OUTPUT,
       @ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

       BEGIN TRY
       SELECT @ResultCode = 0,
                 @ResultMessage = 'executed successfully'

       SELECT RH.[ReferralHeaderID],
              RH.ContactID,
              RH.ReferralDate,
              [ReferralStatusID],
              [ReferralTypeID],
              [ResourceTypeID],
              RCS.[ReferralCategoryID],
              RH.[ReferralCategorySourceID],
              [ReferralOriginID],
              [ProgramID],
              RH.[ReferralOrganizationID],
              RH.[OtherOrganization],
              RH.[IsActive],
              RH.[ModifiedBy],
              RH.[ModifiedOn],
			  CASE WHEN cr.ContactRelationshipID IS NOT NULL THEN 1 ELSE 0 END AS IsCollateral
       FROM
			  [Registration].[ReferralHeader] RH 
			  JOIN [Registration].[ReferralAdditionalDetails] RD ON RH.ReferralHeaderID = RD.ReferralHeaderID
              LEFT JOIN [Reference].[ReferralCategorySource] RCS ON RH.ReferralCategorySourceID = RCS.ReferralCategorySourceID
			  LEFT JOIN Registration.ContactRelationship cr  ON	rh.ReferralHeaderID = cr.ReferralHeaderID 
       WHERE
              RD.ContactID = @ContactID
              AND RD.IsActive = 1
      END TRY
       BEGIN CATCH
              SELECT @ResultCode = ERROR_SEVERITY(),
                     @ResultMessage = ERROR_MESSAGE()
       END CATCH
END
