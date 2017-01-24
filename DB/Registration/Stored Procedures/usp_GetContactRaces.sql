-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetContactRaces]
-- Author:		Scott Martin
-- Date:		03/17/2016
--
-- Purpose:		Gets a list of contact races
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/17/2016	Scott Martin		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetContactRaces]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
		SELECT 
			[ContactRaceID],
			[ContactID],
			[RaceID],
			[IsActive],
			[ModifiedBy],
			[ModifiedOn]
		FROM 
			Registration.ContactRace CR
		WHERE 
			CR.ContactID = @ContactID	
			AND CR.IsActive = 1 		
			
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END