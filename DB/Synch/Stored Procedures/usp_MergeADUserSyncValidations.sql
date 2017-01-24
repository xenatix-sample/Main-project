-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_MergeADUserSyncValidations]
-- Author:		Sumana Sangapu
-- Date:		09/09/2016
--
-- Purpose:		Validations on datapoints for AD Sync
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/09/2016	Sumana Sangapu	Initial creation.
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Synch].[usp_MergeADUserSyncValidations]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY 

		-- Validation for Username excededing 100 characters 
		UPDATE ADUS
		SET ErrorMessage = ISNULL(ErrorMessage,'') + 'Username exceeds 100 characters. '
		FROM [Synch].[ActiveDirectoryRefData] ADUS
		WHERE LEN([samaccountname]) > 100
		--This won't be needed as its already handled while bring data over from AD

		-- Validation for FirstName excededing 50 characters 
		UPDATE ADUS
		SET ErrorMessage = ISNULL(ErrorMessage,'') + 'FirstName exceeds 50 characters. '
		FROM [Synch].[ActiveDirectoryRefData] ADUS
		WHERE LEN([givenName]) > 50
		--This won't be needed as its already handled while bring data over from AD

		-- Validation for LastName excededing 50 characters 
		UPDATE ADUS
		SET ErrorMessage = ISNULL(ErrorMessage,'') + 'LastName exceeds 50 characters. '
		FROM [Synch].[ActiveDirectoryRefData] ADUS
		WHERE LEN([sn]) > 50
		--This won't be needed as its already handled while bring data over from AD

		-- Validation for MiddleName excededing 50 characters 
		UPDATE ADUS
		SET ErrorMessage = ISNULL(ErrorMessage,'') + 'MiddleName exceeds 50 characters. '
		FROM [Synch].[ActiveDirectoryRefData] ADUS
		WHERE LEN([middleName]) > 50
		--This won't be needed as its already handled while bring data over from AD

		-- Validation for Initials excededing 50 characters 
		UPDATE ADUS
		SET ErrorMessage = ISNULL(ErrorMessage,'') + 'Initials exceeds 50 characters. '
		FROM  [Synch].[ActiveDirectoryRefData] ADUS
		WHERE LEN(Initials) > 50
		--This won't be needed as its already handled while bring data over from AD

		-- Validation for PhoneNumber excededing 15 characters 
		UPDATE ADRD
		SET ErrorMessage = ISNULL(ErrorMessage,'') + 'PhoneNumber exceeds 15 characters. '
		FROM [Synch].[ActiveDirectoryRefData] ADRD
		WHERE LEN([telephoneNumber] ) > 15
		--This won't be needed as its already handled while bring data over from AD

		-- Validation for valid PhoneNumber in xxx-xxx-xxxx/xxxxxxxxxx format and no consecutive blocks of 3 or more zeroes
		UPDATE ADRD
		SET ErrorMessage = ISNULL(ErrorMessage,'') + 'Invalid PhoneNumber format. '
		FROM [Synch].[ActiveDirectoryRefData] ADRD
		WHERE  Core.fn_ValidatePhoneNumber(telephoneNumber) = 0
		

		-- Validation for valid Emaild in %@%.% format 
		UPDATE ADRD
		SET ErrorMessage = ISNULL(ErrorMessage,'') + 'Invalid Email format. '
		FROM [Synch].[ActiveDirectoryRefData] ADRD
		WHERE  Core.fn_ValidateEmail(mail) = 0

		-- Validation for EmailID excededing 255 characters 
		UPDATE ADRD
		SET ErrorMessage = ISNULL(ErrorMessage,'') + 'Email exceeds 255 characters. '
		FROM [Synch].[ActiveDirectoryRefData] ADRD
		WHERE LEN([mail] ) > 255
		--This won't be needed as its already handled while bring data over from AD

		-- Validation for StreetAddress exceeding 255 characters
		UPDATE ADRD
		SET ErrorMessage = ISNULL(ErrorMessage,'') + 'Street Address exceeds 255 characters. '
		FROM [Synch].[ActiveDirectoryRefData] ADRD
		WHERE  LEN(streetAddress) > 255 
		--This won't be needed as its already handled while bring data over from AD

		-- Validation for City exceeding 255 characters
		UPDATE ADRD
		SET ErrorMessage = ISNULL(ErrorMessage,'') + 'City exceeds 255 characters. '
		FROM [Synch].[ActiveDirectoryRefData] ADRD
		WHERE  LEN(l) > 255 
		--This won't be needed as its already handled while bring data over from AD

		-- Validation for State 
		UPDATE ADRD
		SET ErrorMessage = ISNULL(ErrorMessage,'') + 'Invalid State. '
		FROM [Synch].[ActiveDirectoryRefData] ADRD
		WHERE st NOT IN (SELECT StateProvinceCode FROM Reference.StateProvince )

		-- Validation for PostalCode
		UPDATE ADRD
		SET ErrorMessage = ISNULL(ErrorMessage,'') + 'Invalid ZipCode Format.'
		FROM [Synch].[ActiveDirectoryRefData] ADRD
		WHERE PostalCode NOT like '[0-9][0-9][0-9][0-9][0-9]'
		AND postalcode  NOT LIKE '[0-9][0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9][0-9]'

	END TRY
	BEGIN CATCH
			SELECT  @ResultCode = ERROR_SEVERITY(),
					@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
