
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetUserFacility]
-- Author:		Suamna Sangapu
-- Date:		12/17/2015
--
-- Purpose:		Get Users based on Facility Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/17/2015 - Sumana Sangapu   - Initial creation
-- 12/21/2015	Scott Martin	Added User name
----------------------------------------------------------------------------------------------------------------------- 

CREATE PROCEDURE [Core].[usp_GetUserFacility] 
	@FacilityID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
			SELECT	UserFacilityID, UF.UserID, CONCAT(U.FirstName, ' ', U.LastName) as UserName, FacilityID
			FROM	[Core].[UserFacility] UF
					INNER JOIN Core.Users U
						ON UF.UserID = U.UserID
			WHERE	FacilityID = @FacilityID
			AND		UF.IsActive = 1 
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END