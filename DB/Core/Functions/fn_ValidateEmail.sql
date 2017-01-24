-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Core.fn_ValidateEmail
-- Author:		Sumana Sangapu
-- Date:		09/06/2016
--
-- Purpose:		Returns if the Email is in valid format
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/06/2016	Sumana Sangapu	Initial Creation
----------------------------------------------------------------------------------------------------------------------

Create FUNCTION Core.fn_ValidateEmail (@email varChar(255))

RETURNS bit
AS
begin
return
(
select 
	Case 
		When 	@Email is null then 0	        --NULL Email is invalid
		When	charindex(' ', @email) 	<> 0 or	--Check for invalid character
				charindex('/', @email) 	<> 0 or --Check for invalid character
				charindex(':', @email) 	<> 0 or --Check for invalid character
				charindex(';', @email) 	<> 0 then 0 --Check for invalid character
		When len(@Email)-1 <= charindex('.', @Email) then 0--check for '%._' at end of string
		When 	@Email like '%@%@%'or 
				@Email Not Like '%@%.%'  then 0--Check for duplicate @ or invalid format
		Else 1
	END
	)
END
 

