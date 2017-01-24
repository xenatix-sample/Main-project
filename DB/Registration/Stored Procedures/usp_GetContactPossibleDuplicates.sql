-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_GetContactPossibleDuplicates]
-- Author:		Scott Martin
-- Date:		03/08/2016
--
-- Purpose:		Get list of possible contact duplicates
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/08/2016	Scott Martin		Initial creation.
-- 03/29/2016	Scott Martin		Tweaked query to couple DOB with FirstName & LastName search, limited result set to 50, and changed sorting
-- 04/01/2016	Scott Martin		Refined the search query
-- 04/18/2016	Scott Martin		Added decryption for SSN
-- 01/12/2017	Scott Martin		Changed the query to only compare Active contacts
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetContactPossibleDuplicates]
	@FirstName NVARCHAR(200),
	@LastName NVARCHAR(200),
	@GenderID INT,
	@SSN NVARCHAR(9),
	@DOB DATE,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		@ResultMessage = 'executed successfully'

	BEGIN TRY
	EXEC Core.usp_OpenEncryptionKeys @ResultCode OUTPUT, @ResultMessage OUTPUT;

	IF ISNULL(@SSN, '') = ''
		BEGIN
		SET @SSN = NULL
		END

	CREATE TABLE #Duplicates
	(	
		ContactID BIGINT,
		MRN BIGINT,
		FirstName NVARCHAR(400),
		Middle nvarchar(400),
		LastName NVARCHAR(400),
		GenderID INT,
		SSN NVARCHAR(9),
		DOB DATE,
		FirstNameMatch INT,
		LastNameMatch INT,
		GenderMatch INT,
		SSNMatch INT,
		DOBMatch INT,
		MatchTotal INT
	);

	INSERT INTO #Duplicates
	(	
		ContactID,
		MRN,
		FirstName,
		Middle,
		LastName,
		GenderID,
		SSN,
		DOB,
		FirstNameMatch,
		LastNameMatch,
		GenderMatch,
		SSNMatch,
		DOBMatch
	)
	SELECT
		C.ContactID,
		C.MRN,
		C.FirstName,
		C.Middle,
		C.LastName,
		C.GenderID,
		C.SSN,
		C.DOB,
		CASE WHEN C.FirstName = @FirstName THEN 15 WHEN SOUNDEX(FirstName) = SOUNDEX(@FirstName) AND C.FirstName <> @FirstName THEN 10 WHEN FirstName LIKE @FirstName + '%' AND C.FirstName <> @FirstName THEN 5 ELSE 0 END,
		CASE WHEN C.LastName = @LastName THEN 15 WHEN SOUNDEX(LastName) = SOUNDEX(@LastName) AND C.LastName <> @LastName THEN 10 WHEN LastName LIKE @LastName + '%' AND C.LastName <> @LastName THEN 5 ELSE 0 END,
		CASE WHEN C.GenderID = @GenderID THEN 5 ELSE 0 END,
		CASE WHEN C.SSN = SUBSTRING(@SSN, 6, LEN(@SSN)) THEN 45 ELSE 0 END,
		CASE WHEN C.DOB = @DOB THEN 15 ELSE 0 END
	FROM
		Registration.Contact C
	WHERE
		(
			SSN = SUBSTRING(@SSN, 6, LEN(@SSN))
			AND Core.fn_Decrypt(SSNEncrypted) = @SSN
			AND IsActive = 1
		)
		OR
		(
			DATEDIFF(DAY, DOB, @DOB) = 0
			AND
			(
				(FirstName = @FirstName)
				OR (SOUNDEX(FirstName) = SOUNDEX(@FirstName) AND FirstName <> @FirstName)
				OR (FirstName LIKE @FirstName + '%' AND C.FirstName <> @FirstName)
			)
			AND
			(
				(LastName = @LastName)
				OR (SOUNDEX(LastName) = SOUNDEX(@LastName) AND LastName <> @LastName)
				OR (LastName LIKE @LastName + '%' AND C.LastName <> @LastName)
			)
			AND GenderID = @GenderID
			AND IsActive = 1
		);

	UPDATE #Duplicates
	SET MatchTotal = FirstNameMatch + LastNameMatch + SSNMatch + DOBMatch;

	SELECT TOP 50
		ContactID,
		MRN,
		FirstName,
		Middle,
		LastName,
		D.GenderID,
		SSN,
		DOB
	FROM
		#Duplicates D
		LEFT OUTER JOIN Reference.Gender G
			ON D.GenderID = G.GenderID
	ORDER BY
		MatchTotal DESC,
		SSN DESC,
		FirstName,
		LastName;

	DROP TABLE #Duplicates;
 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END