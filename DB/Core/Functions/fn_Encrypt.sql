
-----------------------------------------------------------------------------------------------------------------------
-- Function:	[fn_Encrypt]
-- Author:		Sumana Sangapu
-- Date:		04/01/2016
--
-- Purpose:		Encrypts an input value
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/01/2016	Sumana Sangapu	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE FUNCTION Core.fn_Encrypt
(
    @ValueToEncrypt varchar(max)
)
RETURNS varbinary(256)
AS
BEGIN
    -- Declare the return variable here
    DECLARE @Result varbinary(256)

    SET @Result = EncryptByKey(Key_GUID('SymmetricXenatixKey'), @ValueToEncrypt)

    -- Return the result of the function
    RETURN @Result
END