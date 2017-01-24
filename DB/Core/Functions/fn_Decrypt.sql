
-----------------------------------------------------------------------------------------------------------------------
-- Function:	[fn_Decrypt]
-- Author:		Sumana Sangapu
-- Date:		04/01/2016
--
-- Purpose:		Decrypts an input value
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/01/2016	Sumana Sangapu	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE FUNCTION Core.fn_Decrypt
(
    @ValueToDecrypt varchar(max)
)
RETURNS varchar(max)
AS
BEGIN
    -- Declare the return variable here
    DECLARE @Result varchar(max)

    SET @Result = DecryptByKey(@ValueToDecrypt)

    -- Return the result of the function
    RETURN @Result
END