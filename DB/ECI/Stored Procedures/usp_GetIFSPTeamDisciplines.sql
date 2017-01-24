
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [usp_GetIFSPTeamDisciplines]
-- Author:		Gurpreet Singh
-- Date:		10/16/2015
--
-- Purpose:		Get Contact's ECI IFSP Team Discipline Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/26/2015	Gurpreet Singh	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_GetIFSPTeamDisciplines]
       @ContactID BIGINT,
       @ResultCode int OUTPUT,
       @ResultMessage nvarchar(500) OUTPUT
AS
BEGIN
		SELECT @ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

		BEGIN TRY

			SELECT
				ifsp.[ContactID], 
				ifsp.[IFSPID], 
				itd.[UserID],
				u.FirstName + ' ' + u.LastName AS Name,
				STUFF((SELECT ', ' + convert(nvarchar(100), C.[CredentialAbbreviation]) 
						FROM [Core].[UserCredential] A
						INNER JOIN Reference.[Credentials] C ON C.CredentialID = A.CredentialID
						WHERE	 A.[UserID]=B.[UserID] FOR XML PATH('')), 1, 1, '') 
				AS CredentialAbbreviation
			FROM [ECI].[IFSP] ifsp
			INNER JOIN	[ECI].[IFSPTeamDiscipline] itd ON ifsp.[IFSPID] = itd.[IFSPID]
			INNER JOIN	[Core].Users u ON u.UserID = itd.UserID
			LEFT JOIN [Core].[UserCredential] B ON itd.UserID = b.UserID
			WHERE ifsp.ContactID = @ContactID
			AND ifsp.IsActive = 1 AND  itd.IsActive = 1
			GROUP BY 
				ifsp.[ContactID], ifsp.[IFSPID], itd.[UserID], b.[UserID], u.FirstName, u.LastName		

       END TRY
       BEGIN CATCH
              SELECT @ResultCode = ERROR_SEVERITY(),
                     @ResultMessage = ERROR_MESSAGE()
       END CATCH
END