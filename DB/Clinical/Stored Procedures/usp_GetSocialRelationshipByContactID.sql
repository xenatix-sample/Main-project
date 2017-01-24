-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetSocialRelationshipByContactID]
-- Author:		Scott Martin
-- Date:		11/20/2015
--
-- Purpose:		Get Social Relationship Data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/20/2015	Scott Martin	Initial creation.
-- 11/30/2015	Scott Martin	Changed column TakenOn to TakenTime
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_GetSocialRelationshipByContactID]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		[SocialRelationshipID],
		[EncounterID],
		[ContactID],
		ReviewedNoChanges,
		[TakenBy],
		[TakenTime],
		[ChildhoodHistory],
		[RelationshipHistory],
		[FamilyHistory],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn]
	FROM
		[Clinical].[SocialRelationship]
	WHERE
		ContactID = @ContactID
		AND IsActive=1
  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


