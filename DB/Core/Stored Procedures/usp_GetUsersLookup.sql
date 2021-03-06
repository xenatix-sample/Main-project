-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetUsersLookup]
-- Author:		Scott Martin
-- Date:		04/27/2016
--
-- Purpose:		Gets a User List with limited data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/27/2016	Scott Martin  Initial creation
-- 08/16/2016	Vishal Yadav Removed IsActive check.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetUsersLookup]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
		SELECT
			U.UserID,
			U.FirstName + ' ' + U.LastName AS Name,
			P.PhoneID,
			P.Number,
			STUFF((SELECT ', ' + CONVERT(NVARCHAR(100), C.CredentialAbbreviation)
						  FROM Reference.[Credentials] C
						  JOIN Core.UserCredential UC
							ON UC.CredentialID = C.CredentialID
						  WHERE UC.UserID = U.UserID FOR XML PATH('')),1,1,'') AS CredentialAbbreviation,
			U.IsActive,
			U.ModifiedOn,
			U.ModifiedBy
		FROM
			Core.Users U 
			LEFT OUTER JOIN [Core].[UserPhone] UP
				ON U.UserID=UP.UserID
			LEFT OUTER JOIN [Core].[Phone] P
				ON UP.PhoneID=P.PhoneID
		
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END