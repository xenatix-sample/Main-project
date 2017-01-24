-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Core.fn_ValidatePhoneNumber
-- Author:		Sumana Sangapu
-- Date:		09/06/2016
--
-- Purpose:		Returns if the Phonenumber is in valid format
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/06/2016	Sumana Sangapu	Initial Creation
----------------------------------------------------------------------------------------------------------------------
CREATE FUNCTION Core.fn_ValidatePhoneNumber(@PhoneNumber varchar(50))
RETURNS BIT
AS
BEGIN 

	DECLARE @ValidFlag BIT

	SET @ValidFlag = 0
	IF(ISNULL(@PhoneNumber,'') = '')
	BEGIN
		SET @ValidFlag = 1
		RETURN (@ValidFlag)
	END

	IF @PhoneNumber LIKE '[0-9][0-9][0-9]-[0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]'
	BEGIN
	IF LEFT(@PhoneNumber,3) != '000' AND SUBSTRING(@PhoneNumber,5,3) != '000' AND RIGHT(@PhoneNumber,4) != '0000'
	BEGIN
		SET @ValidFlag = 1
		RETURN (@ValidFlag)
	END
	END

	IF @PhoneNumber LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'
	BEGIN

	IF LEFT(@PhoneNumber,3) != '000' AND SUBSTRING(@PhoneNumber,4,3) != '000' AND RIGHT(@PhoneNumber,4) != '0000'
	BEGIN
		SET @ValidFlag = 1
		RETURN (@ValidFlag)
	END
	END

RETURN (@ValidFlag)
END
GO