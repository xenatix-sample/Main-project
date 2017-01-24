-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetSocialRelationshipDetail]
-- Author:		Scott Martin
-- Date:		11/20/2015
--
-- Purpose:		Get Social Relationship Detail Data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/20/2015	Scott Martin	Initial creation.
-- 12/3/2015	Scott Martin	Medical History proc was being created and not Social Relationship
-- 02/02/2016   Lokesh Singhal  Filter the data based on SocialRelationshipID instead of SocialRelationshipDetailID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_GetSocialRelationshipDetail]
	@SocialRelationshipID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		[SocialRelationshipDetailID],
		SocialRelationshipID,
		FR.[FamilyRelationshipID],
		FR.RelationshipTypeID,
		FR.FirstName,
		FR.LastName,
		FR.IsDeceased,
		FR.IsInvolved,
		SRD.[IsActive],
		SRD.[ModifiedBy],
		SRD.[ModifiedOn]
	FROM
		[Clinical].[SocialRelationshipDetail] SRD
		INNER JOIN [Clinical].[FamilyRelationship] FR
			ON SRD.FamilyRelationshipID = FR.FamilyRelationshipID
	WHERE
		SocialRelationshipID = @SocialRelationshipID
		AND SRD.IsActive=1
  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


