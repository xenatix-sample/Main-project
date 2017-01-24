-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetUserProgramFacility]
-- Author:		John Crossen
-- Date:		10/14/2015
--
-- Purpose:		Get User Program based on Facility Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/06/2015 - John Crossen  TFS 2708 - Initial draft
-- 10/31/2015 - Justin Spalti - Corrected hard-coded ProgramID
-- 07/01/2015 - Gaurav Gupta  - Added Phone id and phone Number 
----------------------------------------------------------------------------------------------------------------------- 
CREATE PROCEDURE [Core].[usp_GetUserProgramFacility]
	@ProgramID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
		SELECT U.UserID, U.FirstName + ' ' + U.LastName AS Name,
		P.PhoneID,P.Number,  STUFF((SELECT ', ' + CONVERT(NVARCHAR(100), C.CredentialAbbreviation)
						  FROM Reference.[Credentials] C
						  JOIN Core.UserCredential UC
							ON UC.CredentialID = C.CredentialID
						  WHERE UC.UserID = U.UserID FOR XML PATH('')),1,1,'') AS CredentialAbbreviation,
			   UF.FacilityID, FP.ProgramID,
			   U.IsActive,U.ModifiedOn,U.ModifiedBy
		FROM Core.Users U 
		JOIN Core.UserFacility UF 
			ON U.UserID=UF.UserID
		JOIN Core.FacilityProgram FP 
			ON UF.FacilityID=FP.FacilityID
	    LEFT OUTER JOIN [Core].[UserPhone] UP
			ON U.UserID=UP.UserID
	     LEFT OUTER JOIN [Core].[Phone] P
			ON UP.PhoneID=P.PhoneID
		WHERE FP.ProgramID = @ProgramID
			AND U.IsActive=1
		GROUP BY U.UserID, U.FirstName, U.LastName,
			     UF.FacilityID, FP.ProgramID,P.PhoneID,P.Number,
				 U.IsActive, U.ModifiedOn, U.ModifiedBy
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END