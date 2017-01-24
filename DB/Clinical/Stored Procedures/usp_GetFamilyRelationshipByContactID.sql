-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetFamilyRelationshipByContactID]
-- Author:		Scott Martin
-- Date:		11/20/2015
--
-- Purpose:		Get Family Relationship Data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/20/2015	Scott Martin	Initial creation.
-- 12/03/2015	Gurpreet Singh	Removed ModifiedBy Parameter
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_GetFamilyRelationshipByContactID]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		[FamilyRelationshipID],
		[ContactID],
		[RelationshipTypeID],
		[FirstName],
		[LastName],
		[IsDeceased],
		[IsInvolved],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn]
	FROM
		[Clinical].[FamilyRelationship]
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


