-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetContactSignatures]
-- Author:		Sumana Sangapu
-- Date:		08/18/2015
--
-- Purpose:		Get the list of Signatures  
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/18/2015	Sumana Sangapu	914 - Initial Creation
-- 08/20/2015   Justin Spalti - Updated the schema to ESignature
-- 08/23/2015   Justin Spalti - Added a new parameter for ContactID to retrieve the latest signature
-- 08/26/2015   Justin Spalti - Added the additional parameters needed for the medical consent form and restructured logic to return contact information even if no signatures exist
-- 08/31/2015   Justin Spalti - Added the proper ordering needed in the result set using the ModifiedOn column
-- 09/04/2015   Justin Spalti - Added additional checking to ensure that the minimum requirements of registration have been met
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ESignature].[usp_GetContactSignatures]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		CREATE TABLE #tmpContactSignatures(ID INT PRIMARY KEY IDENTITY(1, 1), ContactID BIGINT, ContactName NVARCHAR(401), ContactDateofBirth DATE, 
										   SignatureID BIGINT, SignatureBLOB VARBINARY(6000), AuthorizedBy NVARCHAR(101), IsActive BIT, ModifiedBy INT, ModifiedOn DATETIME)

		INSERT INTO #tmpContactSignatures(ContactID, ContactName, ContactDateofBirth)
		SELECT c.ContactID, c.FirstName + ' ' + c.LastName, c.DOB
		FROM Registration.Contact c
		WHERE c.ContactID = @ContactID
			AND c.DOB IS NOT NULL

		IF NOT EXISTS
		(
			SELECT 1
			FROM #tmpContactSignatures
			WHERE ContactName IS NOT NULL
		)
		BEGIN
			RAISERROR('No records found!', 16, 1)
		END

		UPDATE t
		SET SignatureID = s.SignatureID,
			SignatureBLOB = s.SignatureBLOB,
			AuthorizedBy = u.FirstName + ' ' + u.LastName,
			IsActive = cs.IsActive,
			ModifiedBy = cs.ModifiedBy,
			ModifiedOn = cs.ModifiedOn
		FROM #tmpContactSignatures t
		JOIN [Registration].Contact c
			ON c.ContactID = t.ContactID
		JOIN [ESignature].ContactSignatures cs
			ON cs.ContactID = c.ContactID
		JOIN [ESignature].Signatures s
			ON s.SignatureID = cs.SignatureID
		JOIN [Core].Users u
			ON u.UserID = cs.ModifiedBy
		WHERE s.IsActive = 1
			AND cs.IsActive = 1

		SELECT t.ContactID, t.ContactName, t.ContactDateofBirth, 
			   t.SignatureID, t.SignatureBLOB, t.AuthorizedBy,
			   t.IsActive, t.ModifiedBy, t.ModifiedOn
		FROM #tmpContactSignatures t
		ORDER BY t.ModifiedBy DESC

		DROP TABLE #tmpContactSignatures
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
