-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [usp_GetEligibilityTeamDisciplines]
-- Author:		Sumana Sangapu
-- Date:		10/16/2015
--
-- Purpose:		Get Contact's ECI Eligibility Team Discipline Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/16/2015	Sumana Sangapu	TFS:2700	Initial Creation
-- 10/20/2015   Justin Spalti - Updated the string concatenation to use CredentialAbbreviation

-- exec [ECI].[usp_GetEligibilityTeamDisciplines] 1,'',''
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_GetEligibilityTeamDisciplines]
       @ContactID BIGINT,
       @ResultCode int OUTPUT,
       @ResultMessage nvarchar(500) OUTPUT
AS
BEGIN
       SELECT
       @ResultCode = 0,
       @ResultMessage = 'Executed Successfully'

       BEGIN TRY

				  SELECT		e.ContactID as ContactID, e.EligibilityID as EligibilityID, etd.[UserID] as UserID, u.FirstName + ' ' + u.LastName AS Name,
								STUFF((SELECT ', ' + convert(nvarchar(100),C.[CredentialAbbreviation]) 
									   FROM [Core].[UserCredential] A
									   INNER JOIN Reference.[Credentials] C
										ON C.CredentialID = A.CredentialID
									   WHERE	 A.[UserID]=B.[UserID] FOR XML PATH('')),1,1,'') As CredentialAbbreviation
				  FROM			[ECI].[Eligibility] e
				  INNER JOIN	[ECI].[EligibilityTeamDiscipline] etd
				  ON			e.EligibilityID = etd.EligibilityID
				  INNER JOIN	[Core].Users u
				  ON			u.UserID = etd.UserID
	  			  LEFT JOIN		[Core].[UserCredential] B
				  ON			etd.UserID = b.UserID
				  WHERE			e.ContactID = @ContactID
				  AND			e.IsActive = 1
				  AND			etd.IsActive = 1
				  GROUP BY		e.ContactID,e.EligibilityID,etd.[UserID],b.UserID, u.FirstName, u.LastName
		

       END TRY
       BEGIN CATCH
              SELECT @ResultCode = ERROR_SEVERITY(),
                     @ResultMessage = ERROR_MESSAGE()
       END CATCH
END