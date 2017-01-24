-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_GetUserSupervisor
-- Author:		Scott Martin
-- Date:		03/01/2016
--
-- Purpose:		Gets a list of Supervisors associated with User
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/01/2016	Scott Martin		Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetUserSupervisor]
	@UserID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'Users retrieved successfully'

	BEGIN TRY	
	SELECT
		U.UserID,
		U.FirstName,
		U.LastName,
		U.UserName,
		E.Email
	FROM
		Core.UsersHierarchyMapping UHM
		INNER JOIN Core.Users U
			ON UHM.ParentID = U.UserID
		INNER JOIN Core.UserEmail UE
			ON U.UserID = UE.UserID
		INNER JOIN Core.Email E
			ON UE.EmailID = E.EmailID
	WHERE
		UHM.UserID = @UserID
		AND UHM.IsActive = 1
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END