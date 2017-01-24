-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_GetContactPresentingProblem]
-- Author:		Scott Martin
-- Date:		03/28/2016
--
-- Purpose:		Get Contact Presenting Problem Data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/28/2016	Scott Martin	Initial creation.
-- 03/31/2016	Scott Martin	Removed OrganizationID and replaced it with PresentingProblemTypeID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetContactPresentingProblem]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	SELECT TOP 1
		ContactPresentingProblemID,
		[ContactID],
		[PresentingProblemTypeID],
		[EffectiveDate],
		[ExpirationDate],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn]
	FROM
		Registration.ContactPresentingProblem
	WHERE
		ContactID = @ContactID
		AND IsActive = 1
	ORDER BY
		ContactPresentingProblemID DESC;
  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


